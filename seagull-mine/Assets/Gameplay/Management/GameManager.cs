using UnityEditor;
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

        public SceneAsset mainMenu;
        public Level[] levels;
        public GameObject pauseMenuPrefab;
        private GameObject _pauseMenuInstance;

        private bool _isPaused = false;

        private void Awake()
        {
            _pauseMenuInstance = Instantiate(pauseMenuPrefab);
            _pauseMenuInstance.SetActive(false);
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

        public void LoadMenu()
        {
            SceneManager.LoadScene(mainMenu.name);
        }

        public void LoadLevel(int i)
        {
            if (i > levels.Length)
            {
                Debug.LogError("Load level out of bounds. " + i);
                return;
            }

            SceneManager.LoadScene(levels[i].scene.name);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}