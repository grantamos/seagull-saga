using System;
using Cinemachine;
using UnityEngine;

namespace Gameplay.Camera
{
    public class AdjustDistanceBasedOnHeight : MonoBehaviour
    {
        [SerializeField] private float minDistance = 15;
        [SerializeField] private float maxDistance = 30;
        [SerializeField] private float minHeight = 30;
        [SerializeField] private float maxHeight = 30;

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineFramingTransposer _transposer;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            var lerpAmount = (Math.Clamp(transform.position.y, minHeight, maxHeight) - minHeight) / (maxHeight - minHeight);
            var distance = Mathf.Lerp(minDistance, maxDistance, lerpAmount);
            _transposer.m_CameraDistance = distance;
        }
    }
}
