using System;
using Gameplay.Actors.HomeBase;
using UnityEngine;

namespace Gameplay.Actors.Food
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroppableFood : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3 _lastPosition;
        private Vector3 _velocity;
        private bool _dropped = false;

        public void Update()
        {
            if (_dropped)
            {
                var move = _velocity + Vector3.down * 9.8f;
                transform.Translate(move * Time.deltaTime);
            }
            else
            {
                _velocity = (transform.position - _lastPosition) / Time.deltaTime;
                _lastPosition = transform.position;
            }
        }

        public void Drop()
        {
            transform.parent = null;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(_velocity, ForceMode.VelocityChange);
        }

        public void OnTriggerEnter(Collider other)
        {
            var babySeagulls = other.GetComponent<BabySeagulls>();
            if (babySeagulls != null)
            {
                babySeagulls.Feed();
                Destroy(gameObject);
            }
        }
    }
}
