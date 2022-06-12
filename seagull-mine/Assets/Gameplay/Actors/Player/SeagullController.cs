using System;
using Gameplay.Actors.Food;
using UnityEngine;
using Utilities;

namespace Gameplay.Actors.Player
{
    public class SeagullController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float brakingAcceleration = 5f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float maxHeight = 20f;
        [SerializeField] private GameObject fishPrefab;
        [SerializeField] private Transform clawsTransform;
        
        private GameObject _fishInstance;
        private float _speed = 0f;
        private bool _isAccelerating = false;
        private Plane _ceilingPlane;

        public void Awake()
        {
            _speed = 0f;
            transform.LookAt(transform.position + Vector3.right, -Vector3.forward);
            _ceilingPlane = new Plane(Vector3.up, Vector3.up * maxHeight);
            CreateFish();
        }

        private void CreateFish()
        {
            _fishInstance = Instantiate(fishPrefab, clawsTransform);
            _fishInstance.transform.localPosition = Vector3.zero;
            _fishInstance.SetActive(false);
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

            // Calculate movement vector;
            var movement = transform.forward * _speed;

            // If the movement vector would take us above our height limit
            // start to slow our speed.
            var positionOneSecondLater = transform.position + movement;
            if (positionOneSecondLater.y > maxHeight)
            {
                var ray = new Ray(transform.position, movement);
                var distance = 0f;

                if (_ceilingPlane.Raycast(ray, out distance))
                {
                    _speed = distance;
                }
            }

            // Translate along the forward axis by speed.
            transform.Translate(movement * Time.deltaTime, Space.World);
        }

        public void OnTriggerEnter(Collider other)
        {
            var edible = other.GetComponent<Edible>();
            if (edible != null)
            {
              Eat(edible);  
            }
        }

        public void Eat(Edible edible)
        {
            if (_fishInstance.activeSelf)
            {
                return;
            }
            
            Destroy(edible.gameObject);
            _fishInstance.SetActive(true);
        }

        public void DropFood()
        {
            if (_fishInstance.activeSelf)
            {
                DroppableFood food = _fishInstance.GetComponent<DroppableFood>();
                food.Drop();
                CreateFish();
            }
        }
    }
}