using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model.Character_Controls
{
    class PlatformerControllerMovement
    {
        // The speed when walking 
        [System.NonSerialized]
        public const float runSpeed = 5.0f;
        [System.NonSerialized]
        public const float walkSpeed = 2.0f;
        // when space is pressed, we jump
        [System.NonSerialized]
        public const float jumpSpeed = 10f;
        [System.NonSerialized]
        public const float jumpOffset = 1f; //Lag time after jumping before movement is registers (so we have a chance to unground)
        [System.NonSerialized]
        public const float GravityFactor = 1.4f;
        // This controls how fast the graphics of the character "turn around" when the player turns around using the controls.
        public const float rotationSmoothing = 10.0f;


        [System.NonSerialized]
        public float verticalSpeed = 0.0f;
        [System.NonSerialized]
        public float horizontalSpeed = 0.0f;
        [System.NonSerialized]
        public bool isMoving = false;
        [System.NonSerialized]
        public bool isWalking = false;
        [System.NonSerialized]
        public float directionMod = 1f;
        [System.NonSerialized]
        public Vector3 velocity;
        [System.NonSerialized]
        public Vector3 oldPosition;

        [System.NonSerialized]
        public Vector3 acceleration;
        [System.NonSerialized]
        public float jumpTime = 0f;   //Last Jump time
        [System.NonSerialized]
        public float lastTimeOnGround = 0f;   //Last Jump time
        [System.NonSerialized]
        public bool jumped = false;
        private const float horizontalControlPower = 2f;

        [System.NonSerialized]
        public bool jumpedOnUpdate;
        public float InAirControlPower
        {
            get { return horizontalControlPower; }
        }
    }
}
