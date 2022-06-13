using System.Collections;
using Gameplay.Actors.Humans;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Management
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                }

                return _instance;
            }
        }

        public string mainMenu;
        public string[] levels;
        public GameObject pauseMenuPrefab;
        private GameObject _pauseMenuInstance;

        public GameObject winScreenPrefab;
        private GameObject _winScreenInstance;
        private HumanController[] _humans;

        private bool _isPaused;
        private int _hungryBabies;
        private int _currentLevel;

        public int HungryBabies => _hungryBabies;

        private void Awake()
        {
            _pauseMenuInstance = Instantiate(pauseMenuPrefab);
            _pauseMenuInstance.SetActive(false);
            _winScreenInstance = Instantiate(winScreenPrefab);
            _winScreenInstance.SetActive(false);

            _humans = FindObjectsOfType<HumanController>();

            var currentScene = SceneManager.GetActiveScene().name;
            for (var i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                if (level != currentScene) continue;
                _currentLevel = i;
            }

            Time.timeScale = 1f;
        }

        public void Pause()
        {
            if (_isPaused)
            {
                return;
            }

            _isPaused = true;
            Time.timeScale = 0f;
            _pauseMenuInstance.SetActive(true);
        }

        public void Unpause()
        {
            if (!_isPaused)
            {
                return;
            }

            _isPaused = false;
            Time.timeScale = 1f;
            _pauseMenuInstance.SetActive(false);
        }

        public void ResetAHuman()
        {
            foreach (var human in _humans)
            {
                if (human.State == HumanController.HumanState.GIVEN_UP)
                {
                    human.Reset();
                    return;
                }
            }
            
            Debug.Log("Oh no, no human to reset.");
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(mainMenu);
        }

        public bool HasNextLevel()
        {
            return _currentLevel < levels.Length - 1;
        }

        public void NextLevel()
        {
            LoadLevel(_currentLevel + 1);
        }

        public void ReloadCurrentLevel(float delay)
        {
            StartCoroutine(ReloadCurrent(delay));
        }

        private IEnumerator ReloadCurrent(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReloadCurrentLevel();
        }

        public void ReloadCurrentLevel()
        {
            LoadLevel(_currentLevel);
        }

        public void LoadLevel(int i)
        {
            if (i > levels.Length)
            {
                Debug.LogError("Load level out of bounds. " + i);
                return;
            }

            SceneManager.LoadScene(levels[i]);
            Time.timeScale = 1f;
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            LoadMenu();
#else
            Application.Quit();
#endif
        }

        public void RegisterHungryBaby()
        {
            _hungryBabies++;
        }

        public void RegisterFedBaby()
        {
            _hungryBabies--;
            if (_hungryBabies == 0)
            {
                ShowWinScreen();
            }
        }

        public void ShowWinScreen()
        {
            Time.timeScale = 0f;
            _winScreenInstance.SetActive(true);
        }
    }
}