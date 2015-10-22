using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScrolling : MonoBehaviour
{
    // The object in our scene that our camera is currently tracking.
    private PlatformerController targetPlatformer;
    private Transform target
    {
        get { return targetPlatformer.transform; }
    }
    // How far back should the camera be from the target?
    public float distance = 15.0f;
    public float diffFactor = 5f;
    // How strict should the camera follow the target?  Lower values make the camera more lazy.
    public float springiness = 1.0f;

    public float MinZoomLevel = 8f;
    public float MaxZoomLevel = 30f;


    void Start()
    {
        targetPlatformer = (PlatformerController)FindObjectOfType(typeof(PlatformerController));
    }
    void Awake()
    {
        // Set up our convenience references.
    }

	public void SetToActiveCamera(){
		this.GetComponent<Camera>().enabled=true;
	}
	public void Disable(){
		this.GetComponent<Camera>().enabled=false;
	}




    // This is a simple accessor function, sometimes called a "getter".  It is a publically callable
    // function that returns a private variable.  Notice how target defined at the top of the script
    // is marked "private"?  We can not access it from other scripts directly.  Therefore, we just
    // have a function that returns it.  Sneaky!
    public Transform GetTarget()
    {
        return target;
    }

    // You almost always want camera motion to go inside of LateUpdate (), so that the camera follows
    // the target _after_ it has moved.  Otherwise, the camera may lag one frame behind.
    void LateUpdate()
    {
		if(!this.GetComponent<Camera>().enabled){
			return;
		}
		// Where should our camera be looking right now?
        Vector3 goalPosition = GetGoalPosition();

        // Interpolate between the current camera position and the goal position.
        // See the documentation on Vector3.Lerp () for more information.
        transform.position = Vector3.Lerp(transform.position, goalPosition, Time.deltaTime * springiness);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetPlatformer.GetUpDirection()), Time.deltaTime*springiness*.25f);
		float zoomDiff = InputWatcher.zoomDiff ();
        distance -= (zoomDiff * diffFactor);
        distance = Mathf.Min(MaxZoomLevel, distance);
        distance = Mathf.Max(MinZoomLevel, distance);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(( (movement.acceleration)).normalized, -movement.acceleration.normalized), Time.deltaTime * movement.rotationSmoothing);
    }

    // Based on the camera attributes and the target's special camera attributes, find out where the
    // camera should move to.
    public Vector3 GetGoalPosition()
    {
        // If there is no target, don't move the camera.  So return the camera's current position as the goal position.
        if (!target)
            return transform.position;

        // Our camera script can take attributes from the target.  If there are no attributes attached, we have
        // the following defaults.

        // How much should we zoom the camera based on this target?
        float distanceModifier = 1.0f;
        // By default, we won't account for any target velocity in our calculations;
        float velocityLookAhead = 0.0f;
        Vector2 maxLookAhead = new Vector2(0.0f, 0.0f);

        // Look for CameraTargetAttributes in our target.
        CameraTargetAttributes cameraTargetAttributes = target.GetComponent<CameraTargetAttributes>();

        // If our target has special attributes, use these instead of our above defaults.
        if (cameraTargetAttributes)
        {
            distanceModifier = cameraTargetAttributes.distanceModifier;
            velocityLookAhead = cameraTargetAttributes.velocityLookAhead;
            maxLookAhead = cameraTargetAttributes.maxLookAhead;
        }

        // First do a rough goalPosition that simply follows the target at a certain relative height and distance.
        //Vector3 goalPosition = target.position + targetPlatformer.movement.gravityDirection * distanceModifier;
        Vector3 goalPosition = target.position + new Vector3(0, 0, -distance * distanceModifier) + targetPlatformer.GetUpDirection() * 2;

        // Next, we refine our goalPosition by taking into account our target's current velocity.
        // This will make the camera slightly look ahead to wherever the character is going.

        // First assume there is no velocity.
        // This is so if the camera's target is not a Rigidbody, it won't do any look-ahead calculations because everything will be zero.
        Vector3 targetVelocity = Vector3.zero;

        // If we find a Rigidbody on the target, that means we can access a velocity!
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody)
            targetVelocity = targetRigidbody.velocity;

        // If we find a PlatformerController on the target, we can access a velocity from that!
        PlatformerController targetPlatformerController = target.GetComponent<PlatformerController>();
        if (targetPlatformerController)
            targetVelocity = targetPlatformerController.GetVelocity();

        // If you've had a physics class, you may recall an equation similar to: position = velocity * time;
        // Here we estimate what the target's position will be in velocityLookAhead seconds.
        Vector3 lookAhead = targetVelocity * velocityLookAhead;

        // We clamp the lookAhead vector to some sane values so that the target doesn't go offscreen.
        // This calculation could be more advanced (lengthy), taking into account the target's viewport position,
        // but this works pretty well in practice.
        lookAhead.x = Mathf.Clamp(lookAhead.x, -maxLookAhead.x, maxLookAhead.x);
        lookAhead.y = Mathf.Clamp(lookAhead.y, -maxLookAhead.y, maxLookAhead.y);
        // We never want to take z velocity into account as this is 2D.  Just make sure it's zero.
        lookAhead.z = 0.0f;

        // Now add in our lookAhead calculation.  Our camera following is now a bit better!
        goalPosition += lookAhead;

        

      
        

        // Send back our spiffily calculated goalPosition back to the caller!
        return goalPosition;
    }

}