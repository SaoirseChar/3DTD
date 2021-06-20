using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Player;
using UnityEngine.EventSystems;

namespace TowerDefense.Turret
{
    public class TowerPlacement : MonoBehaviour
    {
        public HiringManager myHiringManager;
        public Color hoverColor;
        public Color notEnoughMoneyColor;
        private Renderer myRend;
        private Color startColor;

        private void Awake()
        {
            myRend = GetComponent<Renderer>();
            startColor = myRend.material.color;
            myHiringManager = FindObjectOfType<HiringManager>();
        }

        // OnMouseUp is called when the user has released the mouse button
        void OnMouseUp()
        {
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

                Instantiate(myHiringManager.selectedTurret, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (myHiringManager.selectedTurret == null)
                return;

            int turretCost = myHiringManager.selectedTurret.GetComponentInChildren<Turret>().turretCost;

            if (PlayerManager.currentMoney > turretCost)
            {
                myRend.material.color = hoverColor;
            }
            else
            {
                myRend.material.color = notEnoughMoneyColor;
            }

        }

        void OnMouseExit()
        {
            myRend.material.color = startColor;
        }
    }
}