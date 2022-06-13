using Gameplay.Management;
using UnityEngine;

namespace UI.Menus.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        public void OnContinue()
        {
            GameManager.Instance.Unpause();
        }

        public void OnQuit()
        {
            GameManager.Instance.Quit();
        }

        public void OnRetry()
        {
            GameManager.Instance.ReloadCurrentLevel();
        }
    }
}