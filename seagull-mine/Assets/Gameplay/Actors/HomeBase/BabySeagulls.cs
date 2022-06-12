using System.Collections;
using Gameplay.Actors.Food;
using Gameplay.Management;
using UnityEngine;

namespace Gameplay.Actors.HomeBase
{
    public class BabySeagulls : MonoBehaviour
    {
        public int babies = 1;
        public AudioClip[] chirps;
        public AudioClip fedAudio;
        public int minChirps;
        public int maxChirps;
        public float randomChirpTime;
        private bool _isChirping;
        private float _nextChirpTime;
        private AudioSource _audioSource;

        public void Awake()
        {
            for (int i = 0; i < babies; i++)
            {
                GameManager.Instance.RegisterHungryBaby();
            }

            _audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            if (_isChirping)
            {
                return;
            }

            if (Time.time > _nextChirpTime)
            {
                StartCoroutine(Chirp());
            }
        }
        
        public IEnumerator PlayEat()
        {
            _isChirping = true;
            _audioSource.clip = fedAudio;
            _audioSource.Play();
            yield return new WaitForSeconds(fedAudio.length + Random.Range(-.1f, .5f));
            _isChirping = false;
        }

        public IEnumerator Chirp()
        {
            _isChirping = true;
            var numChirps = Random.Range(minChirps, maxChirps);

            for (int i = 0; i < numChirps; i++)
            {
                var clip = chirps[Random.Range(0, chirps.Length - 1)];
                _audioSource.clip = clip;
                _audioSource.Play();
                yield return new WaitForSeconds(clip.length + Random.Range(-.1f, .5f));
            }
            
            _nextChirpTime = Time.time + Random.Range(1, randomChirpTime);
            _isChirping = false;
        }

        public void Feed()
        {
            GameManager.Instance.RegisterFedBaby();
            var bobs = transform.GetComponentsInChildren<BobAndSpin>();
            foreach (var bob in bobs)
            {
                if (!bob.isActiveAndEnabled) continue;
                bob.enabled = false;
                break;
            }

            StartCoroutine(PlayEat());
        }
    }
}
