// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.DevTools.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FluffyUnderware.Curvy.Examples
{
    public class SceneSwitcher : MonoBehaviour
    {
        public Text Text;
        public Dropdown DropDown;

        private void Start()
        {
            List<string> items = getScenes();
            if (items.Count == 0 || CurrentLevel < 0)
                Text.text = "Add scenes to the build settings to enable the scene switcher!";
            else
            {
                Text.text = "Scene selector";
                DropDown.ClearOptions();
                DropDown.AddOptions(items);
            }
            DropDown.value = CurrentLevel;
            DropDown.onValueChanged.AddListener(OnValueChanged);
        }

        private int CurrentLevel
        {
            get
            {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            return Application.loadedLevel;
#else
                return SceneManager.GetActiveScene().buildIndex;
#endif
            }
            set
            {
                if (CurrentLevel != value)
                {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                Application.LoadLevel(value);
#else
                    SceneManager.LoadScene(value, LoadSceneMode.Single);
#endif
                }
            }
        }

#if !UNITY_EDITOR
    /// <summary>
    /// I use this to show names more understandable to people unfamiliar with Curvy. I use this only outside of the Unity's editor, to be used by the demo build.
    /// </summary>
        Dictionary<string, string> scenesAlternativeNames = new Dictionary<string, string>{
            { "00_SplineController", "Move object: Follow a static spline" },
            { "04_PaintSpline", "Move object: Follow a dynamic spline" },
            { "20_CGPaths", "Move object: Follow a blended spline" },
            { "21_CGExtrusion", "Move object: Follow a Curvy generated volume" },
            
            { "06_Orientation", "Basics: Store orientation data in a spline" },
            { "01_MetaData", "Basics: Store custom data in a spline" },
            { "05_NearestPoint", "Basics: Find nearest point on spline" },
            { "03_Connections", "Basics: Connections and events" },
            { "24_CGConformPath", "Basics: Project a Spline onto a mesh" },

            { "10_RBSplineController", "Physics: Interaction with a Curvy spline" },
            { "11_Rigidbody", "Physics: Interaction with a Curvy generated mesh" },

            { "02_GUI", "Splines and UI" },
            
            { "22_CGClonePrefabs", "Advanced: Clone objects along a spline" },
            { "26_CGExtrusionExtendedUV", "Advanced: Extended UV functionality" },
            { "27_CGVariableExtrusion", "Advanced: Variable shape extrusion" },
            
            { "12_Train", "Train: Railway junction" },
            { "13_TrainMultiTrackDrifting", "Train: Multi tracks drifting!" },
            { "25_CGExtrusionAdvanced", "Train: Advanced scene" },

            { "28_AsteroidBelt", "Asteroid Belt" },
            { "23_CGTube", "Space tube" },
            { "50_EndlessRunner", "Space Runner" },
            { "51_InfiniteTrack", "Dynamically generated infinite track" },
        };
#endif

        private List<string> getScenes()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            List<string> result = new List<string>(sceneCount);
            for (int i = 0; i < sceneCount; i++)
            {
                string[] path = SceneUtility.GetScenePathByBuildIndex(i).Split('/');
                string sceneName = path[path.Length - 1].TrimEnd(".unity");
                string itemName;
#if !UNITY_EDITOR
                if (scenesAlternativeNames.ContainsKey(sceneName))
                    itemName = scenesAlternativeNames[sceneName];
                else
                    itemName = sceneName;
#else
                itemName = sceneName;
#endif
                result.Add(itemName);
            }
            return result;
        }


        private void OnValueChanged(int value)
        {
            CurrentLevel = DropDown.value;
        }
    }
}