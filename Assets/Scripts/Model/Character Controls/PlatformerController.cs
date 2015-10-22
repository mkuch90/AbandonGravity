using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Model.Character_Controls;

// Require a character controller to be attached to the same game object
[AddComponentMenu("2D Platformer/Platformer Controller")]

public class PlatformerController : MonoBehaviour, MassObject
{
    #region Variables & Structs  

    private const int raycast_count = 5;

    private SpawnPoint spawnPoint;
    private PlatformerControllerMovement movement;
    private LiftableObject holding;
    private Transform oldParentOfHeldObject;
    private PlatformerWorldModel model;
    private PlatformerPlayerAnimation ani;
    private bool fixedUpdateHasRun = false;
    float levelStartTime = 0f;

    #endregion

    #region Applicators
    void ApplyMovement(float horizontalInput)
    {
        // Grounded controls
        if(movement.jumpedOnUpdate)
        {
            movement.jumpedOnUpdate = false;
            movement.velocity += (model.GravityDirectionUp() * PlatformerControllerMovement.jumpSpeed); //perhaps needs to be migrated to fixedupdate
            movement.jumpTime = Time.time;
            ani.PlayJump();
        }
        if(movement.jumpTime + PlatformerControllerMovement.jumpOffset < Time.time)  //Wait an offset of time after a jump to allow movement (allows time to unground)
        {
            float targetSpeed = Mathf.Min(Mathf.Abs(horizontalInput), 1.0f);

            if(Math.Abs(horizontalInput) > 0)  //TODO: move to its own listener waitinf for run and idle
            {
                ani.PlayRun();
            }
            else
            {
                ani.PlayIdle();
            }
            if(movement.isWalking)
            {

                Debug.Log(Time.time + " " + "walking");
                targetSpeed *= PlatformerControllerMovement.walkSpeed;
            }
            else
            {

                targetSpeed *= PlatformerControllerMovement.runSpeed;
            }

            //used to "step up/down" slopes
            movement.velocity = (model.SurfaceSlope() * Mathf.Sign(Vector3.Dot(model.SurfaceSlope(), ForwardDirection()))).normalized * targetSpeed;


        }
    }
    void ApplyThrusters(float horizontalInput)
    {
        Vector3 thrustMod = Vector3.zero;
        horizontalInput = Mathf.Abs(horizontalInput);
        thrustMod = (ForwardDirection() * horizontalInput).normalized * movement.InAirControlPower;
        movement.acceleration += thrustMod; //thrusters use the same component as gravity for velocity

    }
    void ApplyGravity()
    {
        movement.acceleration += model.GravityAcceleration() * PlatformerControllerMovement.GravityFactor;//add the acceleration to the global acceleration (which is reset every update)

    }
    #endregion

    #region Updators
    /// <summary>
    /// Update handles updates between frames. No physics should be placed here or unexpected
    /// behaviors may occur.
    /// </summary>
    void Update() //Nothing related to physics should be updated here
    {
        if(!HasInitialPositionBeenEstablished())
        {
            return;
        }
        if(model.IsGrounded() && InputWatcher.IsJumping())
        {
            movement.jumpedOnUpdate = true;
        }
    }
    /// <summary>
    /// Called after every fixed X time configurable within Unity. Almost all actions related
    /// to physics and movement should be placed here.
    /// </summary>
    void FixedUpdate()
    {
        SetVelocityToActualDistanceMovedOverTime();

        InitialPositionEstablished();

        ManageAnyHeldObjects();

        /*
         * This updates the current model view, such as whether or not the player 
         * is grounded, 
         * the current gravity at this location, and the slope of the nearest ground.
         * */

        //TODO I don't like how we grab the position by looking at the position of the rigidbody. It seems error prone to use rather than jsut looking at an internal decision about where we are.
        // We measure the current world in relation to the character.
        model.TakeGravityMeasurement(GetComponent<Rigidbody>().position); 
        
        ApplyDeathIfNoGravityDetected();
        bool wasGounded = model.IsGrounded();
        model.TakePositionMeasurements(GetComponent<Rigidbody>().position, ForwardDirection(), 5);  //measure the player's surroundings

        /*
         * The follosing blocks figure out if we've recently landed and therefore should show an animation
         * */
        if(wasGounded)
        {
            movement.lastTimeOnGround = Time.time;
        }
        if(!wasGounded && model.IsGrounded())
        {

            if(holding != null || InputWatcher.horizontalInput() == 0)
            {
                ani.PlayLand();
            }
            else
            {
                ani.PlayRoll(Time.time - movement.lastTimeOnGround);
            }
        }

        /*
         * This keeps the player slightly above the ground so we don't fall through
         * */
        float closeGroundDistance = 0.2f; //Needs to be llower than 0.4 to press buttons

        if(model.IsGrounded() && model.DistanceToGround() < closeGroundDistance)
        {
            GetComponent<Rigidbody>().position += model.GravityDirectionUp() * (closeGroundDistance - model.DistanceToGround());
        }
        float closeWallDistance = 1.3f;
        if(holding != null && model.GetForwardDistance() < closeWallDistance)
        {
            GetComponent<Rigidbody>().position -= ForwardDirection() * (closeWallDistance - model.GetForwardDistance());
        }

        /*
         * //must get zeroed every frame (otherwise the system won't work)
         * All sets to movement.acceleration beyond this point should be relative (+= or -=) but not set it.
         * If the're not relative, order matters.
         */
        movement.acceleration = Vector3.zero;

        /*
         * Gets the horizontal movement input.
         * */
        float horizontalInput = InputWatcher.horizontalInput();
        movement.isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        if(horizontalInput != 0)
        {
            movement.directionMod = horizontalInput;
        }
        if(model.IsGrounded())
        {
            ApplyMovement(horizontalInput);
        }
        /*
         * Must be done second in the order, anything after movement (but before controller.move) can't directly set velocity or bad things will happen
         * If grounded, the velovity will be set to the movement 
         * or 0 in the direction of movement.
         * BUG: Going downhill, the player will just move forward and gravity will bring it down. Its choppy.
         * Solution: Move in the direction of the ground normal and not the direction we are facing
         */

        if(model.HighAngleNotGrounded())
        {
            ani.PlaySlide();
        }
        else if(!model.IsGrounded())
        {
            ani.PlayFalling();
            ApplyThrusters(horizontalInput);
        }
        /*
         * Gravity is applied by getting a value for the acceleration from the world model.
         * Gravity is applied by adding the acceleration to movement.acceleration.
         */
        ApplyGravity();

        /*
         * This does teh actual movement.
         * */
        MoveCharacter();

    }
    /// <summary>
    /// Orientation fails when no gravity is detected. The most likely cause is drifting too far from a point source of gravity. There is a performance shortcut in the gravity code that ignores objects too far away for gravity to make sense.
    /// </summary>
    private void ApplyDeathIfNoGravityDetected()
    {
        //HACK This is a work-around so we don't check objects when there is no gravity. For example, if we were to float away from a planet the gravity eventually becomes 0 and therefore we die. There should be a more elegant way of doing this. The world ceases to work when there is no gravity because we can't determine orientation.
        if(model.GravityDirection() == Vector3.zero)
        {
            ResponseManager manager = (ResponseManager) FindObjectOfType(typeof(ResponseManager));
            manager.Death();
        }
    }

    private void ManageAnyHeldObjects()
    {
        if(holding == null)
        {
            return;
        }
        //HACK I don't like the way we get our curent location by looking up our rigidbody and then accessing position.
        //TODO If I don't replace getting the rigidbody with a new paradigm, at least we should encapsulate it with a function rather than getting the component all over the code.
        //holding.SetTargetPosition(rigidbody.position + (rigidbody.rotation * Vector3.up).normalized * 3.5f + (rigidbody.rotation * Vector3.forward) * 1f);
        holding.SetTargetPosition(GetComponent<Rigidbody>().position + (GetComponent<Rigidbody>().rotation * Vector3.up).normalized * 0.3f + (GetComponent<Rigidbody>().rotation * Vector3.forward) * .8f);
        holding.SetTargetRotation((GetComponent<Rigidbody>().rotation * Vector3.up).normalized, (GetComponent<Rigidbody>().rotation * Vector3.forward));

    }

    private void InitialPositionEstablished()
    {
        fixedUpdateHasRun = true;
    }

    private bool HasInitialPositionBeenEstablished()
    {
        return fixedUpdateHasRun;
    }

    private bool InitialPositionUnestablished()
    {
        return fixedUpdateHasRun;
    }

    /// <summary>
    /// When a user collides with a surface and slides, 
    /// the velocity should get reset to the actual speed moved.
    /// </summary>
    private void SetVelocityToActualDistanceMovedOverTime()
    {
        if(!HasInitialPositionBeenEstablished())
        {
            return;
        }
        if(!model.IsGrounded() && Time.time - levelStartTime > 2f)
        {
            Vector3 newVelocity = (transform.position - movement.oldPosition) / Time.deltaTime;
            //By assigning a weight proportional to deltatime (work out to be 0.1f) to the averaging of velocity, the system's velocity approches
            //the actual velocity moved without getting obstructing movement. I.E. - Hit the ceiling and you begin to fall, hit the top
            // of a wall while jumping and you still make it over.

            newVelocity = Constants.Min(newVelocity, movement.velocity); //Prevents "Kicks" from objects. The new velocity is always slower than current velocity in any direction 

            /*
             * Basically, when rotating, your head can push against the wall pushing your velocity. 
             * Proposal: Don't do this when rotating. 
             * Second proposal: This can only ever slow a component of velocity down
             * */
            movement.velocity = Constants.Average(movement.velocity, newVelocity, Time.deltaTime * 10);
            //rigidbody.velocity = Vector3.zero;
            //}
        }
    }

    private void MoveCharacter()
    {
        //calculate the final velocity from any acceleration on the object
        movement.velocity += movement.acceleration * Time.deltaTime;
        // Calculate actual motion, speed limit
        limitMaxSpeed(Constants.MaxSpeed);
        // Move our character!
        movement.oldPosition = transform.position;
        Vector3 currentMovementOffset = movement.velocity * Time.deltaTime;
        //If we're not grounded and the vector of movement is locally dowdwards, move along the slope rather than down.
        if(model.HighAngleNotGrounded() && Vector3.Dot(currentMovementOffset.normalized, model.GravityDirection()) > 0) //needs to make sure we're actually moving into the slope
        {
            Vector3 SurfaceSlope = model.SurfaceSlope();
            currentMovementOffset = SurfaceSlope * Vector3.Dot(currentMovementOffset, SurfaceSlope);

            //TODO : Add slide animation
        }
        GetComponent<Rigidbody>().velocity = currentMovementOffset / Time.deltaTime;
        // rigidbody.MovePosition(rigidbody.position + currentMovementOffset);

        // rigidbody.velocity = Vector3.zero;

        Quaternion oldrotation = GetComponent<Rigidbody>().rotation;
        GetComponent<Rigidbody>().rotation = Quaternion.Lerp(oldrotation, Quaternion.LookRotation((ForwardDirection()), model.GravityDirectionUp()), Time.deltaTime * 8);

    }
    #endregion

    #region Accessors & Startup

    void limitMaxSpeed(float speed)
    {
        if(movement.velocity.sqrMagnitude > speed * speed) //50 should be replaces by a variable
        {
            movement.velocity = movement.velocity.normalized * speed;
        }
    }
    public void fUpdate()
    {
        //Vector3 newPos = transform.position;
        // newPos.z = 0f;
        //  transform.position = newPos;

    }
    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Player Collision");
    }
    public void GrabObject(LiftableObject obj)
    {
        ani.PickUpBox();
        holding = obj;
    }
    public void DropObject()
    {
        if(holding == null)
        {
            return;
        }
        ani.DropBox();
        holding.Deactivate();
        holding = null;
    }
    public bool IsHoldingObject()
    {
        return holding != null;
    }
    public Vector3 GetVelocity()
    {
        return movement.velocity;
    }

    public bool IsMoving()
    {
        return movement.isMoving;
    }

    public Vector3 ForwardDirection()
    {
        if(model == null)
        {
            return Vector3.forward;
        }
        if(movement == null)
        {
            return Vector3.forward;
        }
        return Constants.Rotate90(model.GravityDirection() * Mathf.Sign(movement.directionMod));
    }

    void Start()
    {
        if(movement == null)
        {
            movement = new PlatformerControllerMovement();
        }
        ani = (PlatformerPlayerAnimation) GetComponent("PlatformerPlayerAnimation");
        if(model == null)
        {
            model = new PlatformerWorldModel();
        }
        InitialPositionUnestablished();
    }
    void OnLevelWasLoaded()
    {
        InitialPositionUnestablished();
        movement.velocity = Vector3.zero;
        levelStartTime = Time.time;

    }
    void Awake()
    {
        Spawn();
    }
    void Spawn()
    {
        InitialPositionUnestablished();
        if(movement == null)
        {
            movement = new PlatformerControllerMovement();
        }
        movement.velocity = Vector3.zero;

        //HACK Hacky way of getting the spawn point.
        spawnPoint = (SpawnPoint) FindObjectOfType(typeof(SpawnPoint));
        GetComponent<Rigidbody>().position = spawnPoint.transform.position;
        if(model == null)
        {
            model = new PlatformerWorldModel();
        }

        model.TakeGravityMeasurement(GetComponent<Rigidbody>().position);
        model.TakePositionMeasurements(GetComponent<Rigidbody>().position, ForwardDirection(), raycast_count);
    }
    void OnDeath()
    {
        Spawn();
    }

    void Reset()
    {
        gameObject.tag = "Player";
    }

    public Vector3 GetUpDirection()
    {
        if(model == null)
        {
            return Vector3.up;
        }
        return model.GravityDirectionUp();
    }
    public Vector3 GetDownDirection()
    {
        if(model == null)
        {
            return Vector3.down;
        }
        return model.GravityDirection();
    }
    public List<Vector3> SourceLocations()
    {
        return model.SourceLocations();
    }
    public List<Vector3> HitLocations()
    {
        return model.HitLocations();
    }
    public Vector3 SourceForward()
    {
        return model.SourceForward();
    }
    public Vector3 HitForward()
    {
        return model.HitForward();
    }
    public float GetMass()
    {
        if(holding != null)
        {
            return GetComponent<Rigidbody>().mass + holding.GetMass();
        }
        return GetComponent<Rigidbody>().mass;
    }
    public void PushButton()
    {
        ani.PlayButtonPress();
    }
    public void IsWalking(bool isWalking)
    {
        movement.isWalking = isWalking;
    }



    #endregion
}
