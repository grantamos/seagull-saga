using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class Arrow : MonoBehaviour
    {
        public RectTransform arrow;
        public Image activity;
        public Sprite celebrating;
        public Sprite reeling;

        private RectTransform _rectTransform;
        private bool _isVisible = true;

        public void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetReeling()
        {
            activity.sprite = reeling;
        }

        public void SetCelebrating()
        {
            activity.sprite = celebrating;
        }

        public void SetAngle(float angle)
        {
            arrow.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void SetPosition(Vector3 position)
        {
            _rectTransform.anchoredPosition = position;
        }

        public void Hide()
        {
            if (!_isVisible)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            _isVisible = false;
        }

        public void Show()
        {
            if (_isVisible)
                return;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            _isVisible = true;
        }
    }
}