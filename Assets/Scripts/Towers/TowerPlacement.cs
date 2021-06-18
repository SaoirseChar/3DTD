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
        private void OnMouseUp()
        {
            if (myHiringManager.selectedTurret != null)
            {
                if (PlayerManager.currentMoney < myHiringManager.selectedTurret.GetComponent<Turret>().turretCost)
                {
                    print("Not Enough money");
                    return;
                }
                PlayerManager.currentMoney -= myHiringManager.selectedTurret.GetComponent<Turret>().turretCost;

                Instantiate(myHiringManager.selectedTurret, transform.parent.position, transform.parent.rotation);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}