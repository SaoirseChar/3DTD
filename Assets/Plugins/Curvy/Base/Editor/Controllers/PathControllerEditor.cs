// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using UnityEditor;
using FluffyUnderware.Curvy.Controllers;

namespace FluffyUnderware.CurvyEditor.Controllers
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PathController), true)]
    public class PathControllerEditor : CurvyControllerEditor<PathController>
    {
    }
}
