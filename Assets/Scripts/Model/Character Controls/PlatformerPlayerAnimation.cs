using UnityEngine;
using System.Collections;

public class PlatformerPlayerAnimation : MonoBehaviour
{
    // Adjusts the speed at which the run animation is played back
    public float runAnimationSpeedModifier = 1.5f;
    // Adjusts the speed at which the jump animation is played back
    public float jumpAnimationSpeedModifier = 2.0f;

    public Transform animationTarget;
    public Transform rollSoundTarget;
    private PlatformerController controller;
    private string jumpAnimationName = "Jump";
    private string runAnimationName = "Run";
    private string idleAnimationName = "Idle";
    private string fallAnimationName = "Flailing";
    private string landAnimationName = "Landing";
    private string rollAnimationName = "Roll";
    private string slideAnimationName = "Sliding";
    private string buttonAnimationName = "ButtonPress";
    private string boxFallAnimationName = "FallingBox";
    private string boxIdleAnimationName = "IdleBox";
    private string boxRunAnimationName = "BoxRun";
    private string runIntoWallAnimationName = "RunIntoWall";
    private string boxLandingAnimationName = "LandingBox";
    private string boxSlideAwayAnimationName = "SlidingBoxAway";
    private string boxSlideTowardsAnimationName = "SlidingBoxTowards";
    private float lastJumpTime = 0f;
    private float lastRunIntoWall = 0f;
    private float blockFallFromSlideTime = 0f;
    private bool holdingBox = false;

    public void DropBox()
    {
        animationTarget.GetComponent<Animation>().Stop();
        holdingBox = false;
    }
    public void PickUpBox()
    {
        animationTarget.GetComponent<Animation>().Stop();
        holdingBox = true;
    }


    [System.NonSerialized]
    private float timeLastRun = 0f;

    void Start()
    {

        controller = (PlatformerController)GetComponent("PlatformerController");
        animationTarget.GetComponent<Animation>().Stop();
        GetComponent<AudioSource>().volume = Constants.effectsVolume;
        // By default loop all animations
        animationTarget.GetComponent<Animation>().wrapMode = WrapMode.Loop;

        // Jump animation are in a higher layer:
        // Thus when a jump animation is playing it will automatically override all other animations until it is faded out.
        // This simplifies the animation script because we can just keep playing the walk / run / idle cycle without having to spcial case jumping animations.



        int jumpingLayer = 10;
        int normalLayer = 9;
        int rollLayer = 11;
        int boxLayer = 12;
        AnimationState run = animationTarget.GetComponent<Animation>()[runAnimationName];
        run.layer = normalLayer;
        run.speed *= runAnimationSpeedModifier;

        AnimationState jump = animationTarget.GetComponent<Animation>()[jumpAnimationName];
        jump.layer = jumpingLayer;
        jump.speed *= jumpAnimationSpeedModifier;
        jump.wrapMode = WrapMode.Once;

        AnimationState falling = animationTarget.GetComponent<Animation>()[fallAnimationName];
        falling.layer = normalLayer;
        falling.wrapMode = WrapMode.Loop;


        AnimationState idle = animationTarget.GetComponent<Animation>()[idleAnimationName];
        idle.layer = normalLayer;
        idle.wrapMode = WrapMode.Loop;


        AnimationState land = animationTarget.GetComponent<Animation>()[landAnimationName];
        land.layer = normalLayer;
        land.speed *= 1f;
        land.wrapMode = WrapMode.Once;

        AnimationState roll = animationTarget.GetComponent<Animation>()[rollAnimationName];
        roll.speed *= 0.75f;
        roll.layer = rollLayer;
        roll.wrapMode = WrapMode.Once;



        AnimationState slide = animationTarget.GetComponent<Animation>()[slideAnimationName];
        slide.layer = normalLayer;


        AnimationState button = animationTarget.GetComponent<Animation>()[buttonAnimationName];
        button.speed *= 4f;
        button.wrapMode = WrapMode.Once;
        button.layer = normalLayer;

        AnimationState holding = animationTarget.GetComponent<Animation>()[boxIdleAnimationName];
        holding.layer = boxLayer;
        holding.wrapMode = WrapMode.Loop;

        holding = animationTarget.GetComponent<Animation>()[boxRunAnimationName];
        holding.layer = boxLayer;
        holding.speed *= runAnimationSpeedModifier;
        holding.wrapMode = WrapMode.Loop;

        holding = animationTarget.GetComponent<Animation>()[runIntoWallAnimationName];
        holding.layer = normalLayer;
        holding.speed *= 1f;
        holding.wrapMode = WrapMode.Once;

        holding = animationTarget.GetComponent<Animation>()[boxFallAnimationName];
        holding.layer = boxLayer;
        holding.wrapMode = WrapMode.Loop;

        holding = animationTarget.GetComponent<Animation>()[boxLandingAnimationName];
        holding.layer = boxLayer;
        holding.wrapMode = WrapMode.Once;

        holding = animationTarget.GetComponent<Animation>()[boxSlideAwayAnimationName];
        holding.layer = boxLayer;
        holding.wrapMode = WrapMode.Loop;

        holding = animationTarget.GetComponent<Animation>()[boxSlideTowardsAnimationName];
        holding.layer = boxLayer;
        holding.wrapMode = WrapMode.Loop;



    }

    public void PlayRun()
    {
        if (holdingBox)
        {
            if (!animationTarget.GetComponent<Animation>().IsPlaying(boxRunAnimationName))
            {
                animationTarget.GetComponent<Animation>().CrossFade(boxRunAnimationName, 0f);
                timeLastRun = Time.time;
            }

        }
        else if (!animationTarget.GetComponent<Animation>().IsPlaying(runAnimationName) && !animationTarget.GetComponent<Animation>().IsPlaying(landAnimationName) && !animationTarget.GetComponent<Animation>().IsPlaying(rollAnimationName))
        {
            animationTarget.GetComponent<Animation>().CrossFade(runAnimationName, 0f);
            timeLastRun = Time.time;
        }



        if (!controller.GetComponent<AudioSource>().isPlaying)
        {
            controller.GetComponent<AudioSource>().Play();
        }

    }
    public void PlayRunIntoWall()
    {
        if (holdingBox)
        {
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(runIntoWallAnimationName) && Time.time - lastRunIntoWall > 2f)
        {
            Debug.Log(Time.time + " " + "Run into wall");

            animationTarget.GetComponent<Animation>().Stop();
            lastRunIntoWall = Time.time;
            animationTarget.GetComponent<Animation>().Play(runIntoWallAnimationName);
            timeLastRun = Time.time;
        }
    }
    public void PlayIdle()
    {

        if (holdingBox)
        {
            if (!animationTarget.GetComponent<Animation>().IsPlaying(boxIdleAnimationName) && !animationTarget.GetComponent<Animation>().IsPlaying(boxLandingAnimationName) && Time.time - timeLastRun > 0.5f)
            {

                animationTarget.GetComponent<Animation>().CrossFade(boxIdleAnimationName, 0.1f);
            }
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(idleAnimationName))
        {
            if (!animationTarget.GetComponent<Animation>().IsPlaying(landAnimationName))
            {
                if (!animationTarget.GetComponent<Animation>().IsPlaying(rollAnimationName) && !animationTarget.GetComponent<Animation>().IsPlaying(runIntoWallAnimationName) && Time.time - timeLastRun > 0.5f && Time.time - lastRunIntoWall > 0.5f)
                {
                    animationTarget.GetComponent<Animation>().CrossFade(idleAnimationName, 0.1f);
                }
            }
        }

    }
    public void PlayJump()
    {
        if (holdingBox)
        {
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(jumpAnimationName))
        {
            animationTarget.GetComponent<Animation>().Stop(idleAnimationName);
            animationTarget.GetComponent<Animation>().CrossFade(jumpAnimationName, 0.2f);

        }
        lastJumpTime = Time.time;
    }
    public void PlayLand()
    {
        if (holdingBox)
        {
            //if (!animationTarget.animation.IsPlaying(boxLandingAnimationName))
            //{
            Debug.Log(Time.time + " " + "BoxLand");
            animationTarget.GetComponent<Animation>().Play(boxLandingAnimationName);
            return;
            //}
        }

        if (!animationTarget.GetComponent<Animation>().IsPlaying(rollAnimationName))
        {
            Debug.Log(Time.time + " " + "Land");
            animationTarget.GetComponent<Animation>().Play(landAnimationName);
        }
    }
    public void PlayFalling()
    {
        if (Time.time - blockFallFromSlideTime < 0.5f)
        {
            return;
        }
        if (holdingBox)
        {
            if (!animationTarget.GetComponent<Animation>().IsPlaying(boxFallAnimationName) && Time.time - timeLastRun > 0.5f)
            {
                animationTarget.GetComponent<Animation>().CrossFade(boxFallAnimationName, 0.5f);
            }
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(fallAnimationName) && !animationTarget.GetComponent<Animation>().IsPlaying(rollAnimationName) && Time.time - timeLastRun > 0.5f)
        {
            Debug.Log(Time.time + " " + "fall");
            animationTarget.GetComponent<Animation>().CrossFade(fallAnimationName, 0.5f);
        }
    }
    public void PlayRoll(float timeUngrounded)
    {
        if (holdingBox)
        {
            return;
        }
        if (Time.time - lastJumpTime < 2f) //Stops you from jumping and rolling when you hit scenery mid-jump
        {
            return;
        }
        if (timeUngrounded < 1f)
        {
            return;
        }
        //velocity downward should be a certain speed
        if (!animationTarget.GetComponent<Animation>().IsPlaying(rollAnimationName))
        {
            Debug.Log(Time.time + " " + "Roll");
            animationTarget.GetComponent<Animation>().Stop();
            animationTarget.GetComponent<Animation>().CrossFade(rollAnimationName, 0.2f);
            rollSoundTarget.GetComponent<AudioSource>().Play();
        }
    }
    public void PlaySlide()
    {
        blockFallFromSlideTime = Time.time;
        if (holdingBox)
        {
            if (!animationTarget.GetComponent<Animation>().IsPlaying(boxSlideAwayAnimationName))
            {
                animationTarget.GetComponent<Animation>().CrossFade(boxSlideAwayAnimationName, 0.2f);

            }
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(slideAnimationName))
        {
            Debug.Log(Time.time + " " + "Slide");
            animationTarget.GetComponent<Animation>().CrossFade(slideAnimationName, 0.2f);
        }
    }
    public void PlayButtonPress()
    {
        if (holdingBox)
        {
            return;
        }
        if (!animationTarget.GetComponent<Animation>().IsPlaying(buttonAnimationName))
        {
            Debug.Log(Time.time + " " + "Button");
            animationTarget.GetComponent<Animation>().CrossFade(buttonAnimationName, 0.25f);
        }
    }

}
