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

        public Turret selectedTower;

        public Turret standardTurretPrefab;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}