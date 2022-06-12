using UnityEngine;

namespace Gameplay.Actors.Player
{
    public class DieIfHeightBelowValue : MonoBehaviour
    {
        public float minHeight = 0f;
        void Update()
        {
            if (transform.position.y < minHeight)
            {
                Destroy(gameObject);
            }
        }
    }
}
