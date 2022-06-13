using System;
using Gameplay.Actors.HomeBase;
using Gameplay.Management;
using UnityEngine;

namespace Gameplay.Actors.Food
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroppableFood : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3 _lastPosition;
        private bool _dropped = false;
        private bool _hasBeenDropped;
        private float _timeSittingStill = 0f;
        private float _timeUntilReset = 5f;
        private bool _isDead;

        public void Update()
        {
            if (_isDead || !_hasBeenDropped)
            {
                return;
            }

            if (IsDead())
            {
                GameManager.Instance.ResetAHuman();
                _isDead = true;
            }
        }

        public bool IsDead()
        {
            if (_rigidbody.velocity.magnitude < 0.01f)
            {
                _timeSittingStill += Time.deltaTime;
            }
            else
            {
                _timeSittingStill = 0;
            }

            return _timeSittingStill > _timeUntilReset;
        }

        public void Drop(Vector3 velocity)
        {
            transform.parent = null;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
            _hasBeenDropped = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!_hasBeenDropped)
                return;
            
            var babySeagulls = other.GetComponent<BabySeagulls>();
            if (babySeagulls != null)
            {
                babySeagulls.Feed();
                Destroy(gameObject);
            }
        }
    }
}
