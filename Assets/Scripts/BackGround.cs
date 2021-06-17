using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Background
{
    public class BackGround : MonoBehaviour
    {
        public float speed = 100; 

        // Update is called once per frame
        // This will constantly move it and then move it to the back
        void Update()
        {
            transform.position += new Vector3((speed * Time.deltaTime),0,0);
            if (transform.position.x>=2000)
            {
                transform.position += new Vector3(-3000, 0, 0);
            }
        }
    }
}