using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Turret
{
    public class OnEndGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(endGameDone());
        }

        IEnumerator endGameDone()
        {
            Time.timeScale = 0.05f;
            yield return new WaitForSeconds(1);
            Time.timeScale = 0;
        }
    }
}