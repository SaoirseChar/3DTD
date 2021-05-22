// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FluffyUnderware.DevTools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FluffyUnderware.Curvy.Generator.Modules
{
    [ModuleInfo("Input/Spline Path", ModuleName = "Input Spline Path", Description = "Spline Path")]
    [HelpURL(CurvySpline.DOCLINK + "cginputsplinepath")]
#pragma warning disable 618
    public class InputSplinePath : SplineInputModuleBase, IExternalInput, IOnRequestPath
#pragma warning restore 618
    {
        [HideInInspector]
        [OutputSlotInfo(typeof(CGPath))]
        public CGModuleOutputSlot Path = new CGModuleOutputSlot();

        #region ### Serialized Fields ###

        [Tab("General", Sort = 0)]
        [SerializeField]
        [CGResourceManager("Spline")]
        [FieldCondition("m_Spline", null, false, ActionAttribute.ActionEnum.ShowWarning, "Create or assign spline")]
        CurvySpline m_Spline;

        #endregion

        #region ### Public Properties ###

        public CurvySpline Spline
        {
            get { return m_Spline; }
            set
            {
                if (m_Spline != value)
                {
                    m_Spline = value;
                    OnSplineAssigned();
                    ValidateStartAndEndCps();
                }
                Dirty = true;
            }
        }

        public bool SupportsIPE { get { return false; } }

        #endregion

        #region ### IOnRequestModule ###

        public CGData[] OnSlotDataRequest(CGModuleInputSlot requestedBy, CGModuleOutputSlot requestedSlot, params CGDataRequestParameter[] requests)
        {
            CGDataRequestRasterization raster = GetRequestParameter<CGDataRequestRasterization>(ref requests);
            CGDataRequestMetaCGOptions options = GetRequestParameter<CGDataRequestMetaCGOptions>(ref requests);
            //TODO BUG? why is this absent from InputSplineShape?
            if (options)
            {
                if (options.CheckMaterialID)
                {
                    options.CheckMaterialID = false;
                    UIMessages.Add("MaterialID option not supported!");
                }
                if (options.IncludeControlPoints)
                {
                    options.IncludeControlPoints = false;
                    UIMessages.Add("IncludeCP option not supported!");
                }
            }
            /*TODO the logic related to the whole OnSlotDataRequest and the CGModuleInputSlot.GetData<T>(params CGDataRequestParameter[] requests) is flawed, and here are the issues I see:
            Some modules need a CGDataRequestParameter[] as a parameter to properly work. This module is one of them. As you can see bellow, if such requeset array is null or empty, the method returns null. This raises some issues:
            - the code doesn't guide a person that wants to create a new module to help him know if he needs to send a CGDataRequestParameter[] to it's input slots or not.
            - the code silently returns null if the necessary data if the required CGDataRequestParameter[] is not there
            - slots who need CGDataRequestParameter[] and those who don't, even if they output the same CGData, can't connect to the same modules. For example, BuildVolumeSpots needs a rasterized CGPath (one that doesn't need CGDataRequestParameter[]) as an input, while BuildVolumeMesh nees a non rasterized CGPath as an input. So two different behaviors, but the same needed data. The user experience is confusing when he needs to connect such splines. For now there are different colors for slots names in those modules, but this isn't very clear to the user, and the code deciding which color to use use different conditions that the one deciding which modules can connect. So a change in one fo them needs always to be mirrored with a change in the othe one. This is very error prone.
            So the logic for connecting mdules should be hardly tied with the logic of sending the CGDataRequestParameter[] params, and the logic of UI display. This needs some work that I hope will be done sometime soon.
            */
            if (!raster || raster.RasterizedRelativeLength == 0)
                return null;

            CGData data = GetSplineData(Spline, true, raster, options);

            return new CGData[1] { data };
        }

        #endregion

        #region ### Public Methods ###

        //BUG? why is this absent from InputSplineShape?
        public override void OnTemplateCreated()
        {
            base.OnTemplateCreated();
            if (Spline && !IsManagedResource(Spline))
            {
                Spline = null;
            }
        }

        #endregion

        #region ### Protected members ###

        protected override CurvySpline InputSpline
        {
            get { return Spline; }
            set { Spline = value; }

        }

        #endregion
    }
}
