using Gameplay.Management;
using UnityEngine;

namespace UI.Menus.Win
{
    public class WinScreen : MonoBehaviour
    {
        public GameObject nextLevelButton;

        public void Awake()
        {
            if (!GameManager.Instance.HasNextLevel())
                nextLevelButton.SetActive(false);
        }

        public void NextLevel()
        {
            GameManager.Instance.NextLevel();
        }

        public void Retry()
        {
            GameManager.Instance.ReloadCurrentLevel();
        }

        public void Quit()
        {
            GameManager.Instance.Quit();
        }
    }
}
