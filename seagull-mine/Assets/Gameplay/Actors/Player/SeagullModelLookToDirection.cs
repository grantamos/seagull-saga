using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Gameplay.Actors.Player
{
    public class SeagullModelLookToDirection : MonoBehaviour
    {
        private Vector3 _lastPosition = Vector3.zero;

        public void Awake()
        {
            _lastPosition = transform.position;
        }

        public void Update()
        {
            var newPosition = transform.position;
            var velocity = newPosition - _lastPosition;
            if (velocity.magnitude < 0.001)
            {
                velocity = transform.parent.forward;
            }
            
            // Calculates the angle from up.
            var angle = Vector3.Angle(Vector3.up, velocity);
            // Offset angle by 90 in order get a range of -90 to 90, so we can easily tell
            // how far we are away from straight up or straight down (0 degrees or 180 degrees).
            angle -= 90;
            
            // Use the sign of the angle to determine which way is forward (towards or away from the camera).
            // Can be removed to "always have the back to the camera", e.g. always face away from camera
            // when going up or down.
            var up = Vector3.Lerp(Vector3.up, Math.Sign(angle) * Vector3.forward, Math.Abs(angle) / 90f);
            
            transform.LookAt(transform.position + velocity, up);

            _lastPosition = transform.position;
        }
    }
}
