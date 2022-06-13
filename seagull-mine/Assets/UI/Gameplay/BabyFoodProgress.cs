using System.Collections.Generic;
using Gameplay.Actors.HomeBase;
using Gameplay.Management;
using UnityEngine;

namespace UI.Gameplay
{
    public class BabyFoodProgress : MonoBehaviour
    {
        public BabySeagulls babySeagulls;
        public GameObject babySeagullIconPrefab;
        private int _hungryBabiesCount;
        private List<BabyHungryIcon> _babyHungry = new();

        public void Awake()
        {
            _hungryBabiesCount = GameManager.Instance.HungryBabies;
            for (int i = 0; i < babySeagulls.babies; i++)
            {
                var icon = Instantiate(babySeagullIconPrefab, transform);
                _babyHungry.Add(icon.GetComponent<BabyHungryIcon>());
            }
        }

        public void Update()
        {
            if (GameManager.Instance.HungryBabies != _hungryBabiesCount)
            {
                _hungryBabiesCount = GameManager.Instance.HungryBabies;
                
                for (int i = 0; i < _babyHungry.Count; i++)
                {
                    if (i < _hungryBabiesCount)
                        _babyHungry[i].SetHungry();
                    else
                        _babyHungry[i].SetFull();
                }
            }
        }
    }
}
