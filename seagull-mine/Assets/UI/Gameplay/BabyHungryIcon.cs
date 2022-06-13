using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class BabyHungryIcon : MonoBehaviour
    {
        public Sprite hungryIcon;
        public Sprite fullIcon;
        public Image image;

        public void Awake()
        {
            image.sprite = hungryIcon;
        }
        
        public void SetHungry()
        {
            image.sprite = hungryIcon;
        }

        public void SetFull()
        {
            image.sprite = fullIcon;
        }
    }
}
