using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Actors.HomeBase
{
    public class KillBabiesAndMakeBigGulls : MonoBehaviour
    {
        public GameObject flyingGulls;
        public GameObject poof;
        
        public void Awake()
        {
            StartCoroutine(Go());
        }

        public IEnumerator Go()
        {
            yield return new WaitForSeconds(2f);
            List<GameObject> delete = new();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("seagull"))
                {
                    delete.Add(transform.GetChild(i).gameObject);
                }
            }

            foreach (var d in delete)
            {
                Destroy(d);
            }

            Instantiate(flyingGulls, transform.position, Quaternion.identity);
            Instantiate(poof, transform.position, Quaternion.identity);
        }
    }
}
