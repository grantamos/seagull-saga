using UnityEngine;

namespace Gameplay.Actors.Food
{
    public class BobAndSpin : MonoBehaviour
    {
        [SerializeField] private float spinSpeed = 2f;
        [SerializeField] private float bobSpeed = 2f;
        [SerializeField] private float bobHeight = 1f;

        private Vector3 _startPosition;
        private float _randomOffset = 0;

        private void Awake()
        {
            _startPosition = transform.position;
            _randomOffset = Random.value * bobSpeed;
        }

        private void Update()
        {
            var offset = Mathf.Sin(Time.time * bobSpeed + _randomOffset);
            transform.position = _startPosition + Vector3.up * offset * bobHeight;
            transform.Rotate(transform.up, spinSpeed * Time.deltaTime);
        }
    }
}