using System;
using UI.Gameplay;
using Unity.VisualScripting;
using UnityEditor.Search;
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
        
        [SerializeField] private GameObject hangingOut;
        [SerializeField] private GameObject goingFishing;
        [SerializeField] private GameObject fishing;
        [SerializeField] private GameObject reeling;
        [SerializeField] private GameObject celebrating;

        private float _timeInState = 0f;
        private GameObject _fishInstance;

        public HumanState State => state;

        private void Awake()
        {
            switch (state)
            {
                case HumanState.HANGING_OUT:
                    transform.position = beachChair.transform.position;
                    ShowModel(hangingOut);
                    break;
                case HumanState.GOING_FISHING:
                    ShowModel(goingFishing);
                    break;
                case HumanState.FISHING:
                    ShowModel(fishing);
                    transform.position = FishingPosition();
                    break;
                case HumanState.REELING_IT_IN:
                    ShowModel(reeling);
                    break;
                case HumanState.CELEBRATING_CATCH:
                    ShowModel(celebrating);
                    break;
                case HumanState.GIVEN_UP:
                    ShowModel(goingFishing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EventPointer.Instance.AddTarget(this);
        }

        private void ShowModel(GameObject toShow)
        {
            hangingOut.SetActive(false);
            goingFishing.SetActive(false);
            celebrating.SetActive(false);
            fishing.SetActive(false);
            reeling.SetActive(false);
            
            toShow.SetActive(true);
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
                ShowModel(hangingOut);
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
                ShowModel(fishing);
            }
        }

        private void UpdateCelebratingCatch()
        {
            if (_timeInState > settings.celebrationTime)
            {
                ChangeState(HumanState.FISHING);
                // Look towards the ocean.
                transform.LookAt(transform.position + Vector3.forward);
                Destroy(_fishInstance);
            }

            if (_fishInstance.IsDestroyed())
            {
                ChangeState(HumanState.GIVEN_UP);
                ShowModel(goingFishing);
            }
        }

        private void UpdateReelItInState()
        {
            if (_timeInState > settings.reelTime)
            {
                ChangeState(HumanState.CELEBRATING_CATCH);
                _fishInstance = Instantiate(fishPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
                ShowModel(celebrating);
            }
        }

        private void UpdateFishingState()
        {
            if (_timeInState > settings.fishingTime)
            {
                ChangeState(HumanState.REELING_IT_IN);
                ShowModel(reeling);
            }
        }

        private void UpdateHangOutState()
        {
            if (_timeInState > settings.hangTime)
            {
                ChangeState(HumanState.GOING_FISHING);
                ShowModel(goingFishing);
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

        public void Reset()
        {
            transform.position = beachChair.transform.position;
            ShowModel(hangingOut);
            ChangeState(HumanState.HANGING_OUT);
        }
    }
}