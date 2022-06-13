using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UITimer : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public void Update()
        {
            TimeSpan time = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            text.text = time.ToString(@"mm\:ss\:ff");
        }
    }
}
