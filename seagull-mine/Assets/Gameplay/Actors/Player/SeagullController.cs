using System;
using UnityEngine;
using Utilities;

namespace Gameplay.Actors.Player
{
    public class SeagullController : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed = 5f;
        [SerializeField]
        private float acceleration = 5f;
        [SerializeField]
        private float brakingAcceleration = 5f;
        [SerializeField]
        private float turnSpeed = 5f;
        [SerializeField] 
        private float gravity = 9.8f;

        private float _speed = 0f;
        private bool _isAccelerating = false;

        public void Awake()
        {
            _speed = 0f;
            transform.LookAt(transform.position + Vector3.right, -Vector3.forward);
        }

        public void Turn(float turnAmount)
        {
            transform.RotateAround(transform.position, transform.up, turnAmount * turnSpeed * Time.deltaTime);
        }

        public void Accelerate(float accelerateAmount)
        {
            _isAccelerating = accelerateAmount > 0;
            _speed += acceleration * accelerateAmount * Time.deltaTime;
        }

        public void Brake(float brakeAmount)
        {
            _speed -= brakingAcceleration * brakeAmount * Time.deltaTime;
            _speed = Math.Max(_speed, 0);
        }

        public void Update()
        {
            // Add gravity depending on how steep our angle is, e.g. apply gravity
            // when we're going up or down, but not horizontal.
            {
                // Calculates the angle from right.
                var angle = Vector3.SignedAngle(Vector3.right, transform.forward, Vector3.forward);
                var isUp = Math.Sign(angle);
                // Offset angle by 90 in order get a range of -90 to 90, so we can easily tell
                // how far we are away from horizontal (0 degrees or 180 degrees).
                var percentVertical = 1 - Math.Abs(Math.Abs(angle) - 90) / 90;
                if (isUp > 0)
                {
                    percentVertical = SeagullMath.EaseInQuart(percentVertical);
                }

                _speed -= gravity * percentVertical * isUp * Time.deltaTime;
            }
            
            // Clamp speed.
            _speed = Math.Clamp(_speed, 0, maxSpeed);

            var movement = transform.forward * _speed;
            // Translate along the forward axis by speed.
            transform.Translate(movement * Time.deltaTime, Space.World);
        }
    }
}
