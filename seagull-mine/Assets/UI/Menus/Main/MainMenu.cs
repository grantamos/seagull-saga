using Gameplay.Management;
using UnityEngine;

namespace UI.Menus.Main
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.LoadLevel(0);
        }
    }
}