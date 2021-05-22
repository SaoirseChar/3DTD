// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;
using FluffyUnderware.Curvy.Controllers;

namespace FluffyUnderware.Curvy.Examples
{
    public class SplineControllerInputRail : MonoBehaviour
    {
        public float acceleration = 0.1f;
        public float limit = 30.0f;
        public SplineController splineController;


        private void Update ()
        {
            float verticalInput = Mathf.Clamp (Input.GetAxis("Vertical"), -1f, 1f);
            splineController.Speed = Mathf.Clamp (splineController.Speed + verticalInput * acceleration * Time.deltaTime, 0.001f, limit);
        }        
    }
}
