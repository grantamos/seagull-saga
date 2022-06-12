using System;
using System.Collections.Generic;
using Gameplay.Actors.Humans;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI.Gameplay
{
    public class EventPointer : MonoBehaviour
    {
        private static EventPointer _instance;

        public static EventPointer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EventPointer>();
                }

                return _instance;
            }
        }
        
        [SerializeField] private int arrowPadding;
        [SerializeField] private GameObject pointerPrefab;
        [SerializeField] private CanvasScaler canvasScaler;
        private List<Target> _targets = new();

        public void AddTarget(HumanController controller)
        {
            var newPointer = Instantiate(pointerPrefab, transform);
            newPointer.SetActive(false);
            _targets.Add(new Target(
                controller.transform.position,
                newPointer.GetComponent<Arrow>(),
                controller
            ));
        }

        private void Update()
        {
            foreach (var target in _targets)
            {
                switch (target.controller.State)
                {
                    case HumanController.HumanState.REELING_IT_IN:
                        UpdateTarget(target);
                        break;
                    case HumanController.HumanState.CELEBRATING_CATCH:
                        UpdateTarget(target);
                        break;
                    default:
                        DisableTarget(target);
                        break;
                }
            }
        }

        private void DisableTarget(Target target)
        {
            if (target.arrow.gameObject.activeSelf)
            {
                target.arrow.gameObject.SetActive(false);
            }
        }

        private void UpdateTarget(Target target)
        {
            if (target.controller.State != target.lastState)
            {
                target.arrow.gameObject.SetActive(true);

                switch (target.controller.State)
                {
                    case HumanController.HumanState.REELING_IT_IN:
                        target.arrow.SetReeling();
                        break;
                    case HumanController.HumanState.CELEBRATING_CATCH:
                        target.arrow.SetCelebrating();
                        break;
                }
            }
            
            Vector3 toPosition = target.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
            target.arrow.SetAngle(angle);

            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(target.position);

            Vector3 cappedPosition = targetPositionScreenPoint;

            var isOffscreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.y <= 0 ||
                              targetPositionScreenPoint.x >= Screen.width ||
                              targetPositionScreenPoint.y >= Screen.height;

            if (targetPositionScreenPoint.x <= 0) cappedPosition.x = arrowPadding;
            if (targetPositionScreenPoint.x >= Screen.width) cappedPosition.x = Screen.width - arrowPadding;
            if (targetPositionScreenPoint.y <= 0) cappedPosition.y = arrowPadding;
            if (targetPositionScreenPoint.y >= Screen.height) cappedPosition.y = Screen.height - arrowPadding;

            target.arrow.SetPosition(cappedPosition / canvasScaler.transform.localScale.x);
            
            if (isOffscreen)
            {
                target.arrow.Show();
            }
            else
            {
                target.arrow.Hide();
            }
            
            target.lastState = target.controller.State;
        }
        
        private float GetScale(int width, int height, CanvasScaler canvasScaler)
        {
            var scalerReferenceResolution = canvasScaler.referenceResolution;
            var widthScale = width / scalerReferenceResolution.x;

            var heightScale = height / scalerReferenceResolution.y;
            switch (canvasScaler.screenMatchMode)
            {
                case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                    var matchWidthOrHeight = canvasScaler.matchWidthOrHeight;

                    return Mathf.Pow(widthScale, 1f - matchWidthOrHeight) *
                           Mathf.Pow(heightScale, matchWidthOrHeight);

                case CanvasScaler.ScreenMatchMode.Expand:
                    return Mathf.Min(widthScale, heightScale);

                case CanvasScaler.ScreenMatchMode.Shrink:
                    return Mathf.Max(widthScale, heightScale);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public struct Target
    {
        public Vector3 position;
        public Arrow arrow;
        public HumanController controller;
        public HumanController.HumanState lastState;

        public Target(Vector3 position, Arrow arrow, HumanController controller)
        {
            this.position = position;
            this.arrow = arrow;
            this.controller = controller;
            this.lastState = controller.State;
        }
    }
}