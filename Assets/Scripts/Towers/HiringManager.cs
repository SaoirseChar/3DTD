using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Player;

namespace TowerDefense.Turret
{
    public class HiringManager : MonoBehaviour
    {
        public static HiringManager thisInstance;

        void Awake()
        {
            if (thisInstance != null)
            {
                Debug.LogError("More than one BuildManager in scene!");
                return;
            }
            thisInstance = this;
        }

        public GameObject selectedTurret;

        public GameObject standardTurretPrefab;

        public void SelectStandarTurret()
        {
            print("Standard Turret Selected");
            selectedTurret = standardTurretPrefab;
        }
    }
}