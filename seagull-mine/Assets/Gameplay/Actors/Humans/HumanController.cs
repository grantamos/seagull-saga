using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Actors.Humans
{
    public class HumanController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject beachChair;
        [SerializeField] private HumanState state = HumanState.HANGING_OUT;
        [SerializeField] private HumanSettings settings;
        [SerializeField] private GameObject fishPrefab;

        private GameObject _fishingLine;
        private float _timeInState = 0f;
        private GameObject _fishInstance;

        private void Awake()
        {
            _fishingLine = transform.Find("fishing-pole/FishingLine").gameObject;
            _fishingLine.SetActive(false);

            switch (state)
            {
                case HumanState.HANGING_OUT:
                    transform.position = beachChair.transform.position;
                    break;
                case HumanState.GOING_FISHING:
                    break;
                case HumanState.FISHING:
                    transform.position = FishingPosition();
                    break;
                case HumanState.REELING_IT_IN:
                    break;
                case HumanState.CELEBRATING_CATCH:
                    break;
                case HumanState.GIVEN_UP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            _timeInState += Time.deltaTime;
            switch (state)
            {
                case HumanState.HANGING_OUT:
                    UpdateHangOutState();
                    break;
                case HumanState.GOING_FISHING:
                    UpdateGoingFishing();
                    break;
                case HumanState.FISHING:
                    UpdateFishingState();
                    break;
                case HumanState.REELING_IT_IN:
                    UpdateReelItInState();
                    break;
                case HumanState.CELEBRATING_CATCH:
                    UpdateCelebratingCatch();
                    break;
                case HumanState.GIVEN_UP:
                    UpdateGivenUp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateGivenUp()
        {
            var moveTo = beachChair.transform.position;
            var direction = moveTo - transform.position;
            var movement = direction.normalized * moveSpeed * Time.deltaTime;

            if (movement.magnitude < direction.magnitude)
            {
                transform.Translate(movement);
            }
            else
            {
                // TODO - Lay down.
            }
        }

        private void ChangeState(HumanState newState)
        {
            state = newState;
            _timeInState = 0f;
        }

        private void UpdateGoingFishing()
        {
            // TODO don't hardcode the 4 here, e.g. the plane of playing.
            var moveTo = FishingPosition();
            var direction = moveTo - transform.position;
            var movement = direction.normalized * moveSpeed * Time.deltaTime;

            if (movement.magnitude < direction.magnitude)
            {
                transform.Translate(movement);
            }
            else
            {
                ChangeState(HumanState.FISHING);
                _fishingLine.SetActive(true);
            }
        }

        private void UpdateCelebratingCatch()
        {
            if (_timeInState > settings.celebrationTime)
            {
                ChangeState(HumanState.FISHING);
                // Show the fishing line!
                _fishingLine.SetActive(true);
                // Look towards the ocean.
                transform.LookAt(transform.position + Vector3.forward);
                Destroy(_fishInstance);
            }

            if (_fishInstance.IsDestroyed())
            {
                ChangeState(HumanState.GIVEN_UP);
                Destroy(transform.Find("fishing-pole").gameObject);
            }
        }

        private void UpdateReelItInState()
        {
            if (_timeInState > settings.reelTime)
            {
                ChangeState(HumanState.CELEBRATING_CATCH);
                _fishInstance = Instantiate(fishPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
                _fishingLine.SetActive(false);
            }
        }

        private void UpdateFishingState()
        {
            if (_timeInState > settings.fishingTime)
            {
                ChangeState(HumanState.REELING_IT_IN);
            }
        }

        private void UpdateHangOutState()
        {
            if (_timeInState > settings.hangTime)
            {
                ChangeState(HumanState.GOING_FISHING);
            }
        }

        private Vector3 FishingPosition()
        {
            return Vector3.Scale(beachChair.transform.position, new Vector3(1, 1, 0)) - Vector3.forward * 4;
        }

        [Serializable]
        public struct HumanSettings
        {
            public float hangTime;
            public float fishingTime;
            public float celebrationTime;
            public float reelTime;

            public HumanSettings(float hangTime, float fishingTime, float celebrationTime, float reelTime)
            {
                this.hangTime = hangTime;
                this.fishingTime = fishingTime;
                this.celebrationTime = celebrationTime;
                this.reelTime = reelTime;
            }
        }
        
        [Serializable]
        public enum HumanState
        {
            HANGING_OUT,
            GOING_FISHING,
            FISHING,
            REELING_IT_IN,
            CELEBRATING_CATCH,
            GIVEN_UP,
        }
    }
}