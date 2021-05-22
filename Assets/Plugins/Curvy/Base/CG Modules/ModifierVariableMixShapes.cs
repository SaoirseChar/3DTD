// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System;
using UnityEngine;
using FluffyUnderware.DevTools;

namespace FluffyUnderware.Curvy.Generator.Modules
{
    [ModuleInfo("Modifier/Variable Mix Shapes", ModuleName = "Variable Mix Shapes", Description = "Interpolates between two shapes in a way that varies along the shape extrusion")]
    [HelpURL(CurvySpline.DOCLINK + "cgvariablemixshapes")]
#pragma warning disable 618
    public class ModifierVariableMixShapes : CGModule, IOnRequestPath
#pragma warning restore 618
    {
        [HideInInspector]
        [InputSlotInfo(typeof(CGShape), Name = "Shape A")]
        public CGModuleInputSlot InShapeA = new CGModuleInputSlot();

        [HideInInspector]
        [InputSlotInfo(typeof(CGShape), Name = "Shape B")]
        public CGModuleInputSlot InShapeB = new CGModuleInputSlot();

        [HideInInspector]
        [ShapeOutputSlotInfo(OutputsVariableShape = true, Array = true, ArrayType = SlotInfo.SlotArrayType.Hidden)]
        public CGModuleOutputSlot OutShape = new CGModuleOutputSlot();

        #region ### Serialized Fields ###
        [Label("Mix Curve", "Mix between the shapes. Values (Y axis) between -1 for Shape A and 1 for Shape B. Times (X axis) between 0 for extrusion start and 1 for extrusion end")]
        [SerializeField]
        private AnimationCurve m_MixCurve = AnimationCurve.Linear(0, -1, 1, 1);
        #endregion
        #region ### Public Properties ###

        public bool PathIsClosed
        {
            get
            {
                return (IsConfigured) ? InShapeA.SourceSlot().PathProvider.PathIsClosed &&
                                        InShapeB.SourceSlot().PathProvider.PathIsClosed : false;
            }
        }

        /// <summary>
        /// Defines how the result is interpolated. Values (Y axis) between -1 for Shape A and 1 for Shape B. Times (X axis) between 0 for extrusion start and 1 for extrusion end
        /// </summary>
        public AnimationCurve MixCurve
        {
            get { return m_MixCurve; }
            set
            {
                m_MixCurve = value;
                Dirty = true;
            }
        }
        #endregion

        #region ### Unity Callbacks ###
        /*! \cond UNITY */

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Dirty = true;
        }
#endif

        public override void Reset()
        {
            base.Reset();
            m_MixCurve = AnimationCurve.Linear(0, -1, 1, 1);
        }

        /*! \endcond */
        #endregion

        #region ### IOnRequestProcessing ###
        public CGData[] OnSlotDataRequest(CGModuleInputSlot requestedBy, CGModuleOutputSlot requestedSlot, params CGDataRequestParameter[] requests)
        {
            CGDataRequestShapeRasterization raster = GetRequestParameter<CGDataRequestShapeRasterization>(ref requests);
            if (!raster)
                return null;

            int pathFLength = raster.PathF.Length;

            CGData[] result = new CGData[pathFLength];

#if UNITY_EDITOR
            bool warnedAboutInterpolation = false;
#endif
            for (int crossIndex = 0; crossIndex < pathFLength; crossIndex++)
            {
                float mix = MixCurve.Evaluate(raster.PathF[crossIndex]);
#if UNITY_EDITOR
                if ((mix < -1 || mix > 1) && warnedAboutInterpolation == false)
                {
                    warnedAboutInterpolation = true;
                    UIMessages.Add(String.Format("Mix Curve should have values between -1 and 1. Found a value of {0} at time {1}", mix, raster.PathF[crossIndex]));
                }
#endif
                result[crossIndex] = ModifierMixShapes.MixShapes(
                InShapeA.GetData<CGShape>(requests),
                InShapeB.GetData<CGShape>(requests),
                mix,
                UIMessages, crossIndex != 0);
            }

            return result;
        }

        #endregion

    }
}