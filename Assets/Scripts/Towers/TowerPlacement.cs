using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Player;

namespace TowerDefense.Turret
{
    public class TowerPlacement : MonoBehaviour
    {
        public HiringManager myHiringManager;

        private void Awake()
        {
            myHiringManager = FindObjectOfType<HiringManager>();
        }

        // OnMouseUp is called when the user has released the mouse button
        void OnMouseUp()
        {
            print("Placement clicked.");
            myHiringManager = FindObjectOfType<HiringManager>();
            int turretCost = myHiringManager.selectedTurret.GetComponentInChildren<Turret>().turretCost;
            if (myHiringManager.selectedTurret != null)
            {
                if (PlayerManager.currentMoney < turretCost)
                {
                    print("Not Enough money");
                    return;
                }
                PlayerManager.currentMoney -= turretCost;

                Instantiate(myHiringManager.selectedTurret, transform.parent.position, transform.parent.rotation);
                Destroy(gameObject);
            }
        }
    }
}