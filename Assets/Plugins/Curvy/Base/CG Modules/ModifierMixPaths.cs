// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FluffyUnderware.DevTools;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace FluffyUnderware.Curvy.Generator.Modules
{
    [ModuleInfo("Modifier/Mix Paths", ModuleName = "Mix Paths", Description = "Interpolates between two paths")]
    [HelpURL(CurvySpline.DOCLINK + "cgmixpaths")]
#pragma warning disable 618
    public class ModifierMixPaths : CGModule, IOnRequestPath
#pragma warning restore 618
    {
        [HideInInspector]
        [InputSlotInfo(typeof(CGPath), Name = "Path A")]
        public CGModuleInputSlot InPathA = new CGModuleInputSlot();

        [HideInInspector]
        [InputSlotInfo(typeof(CGPath), Name = "Path B")]
        public CGModuleInputSlot InPathB = new CGModuleInputSlot();

        [HideInInspector]
        [OutputSlotInfo(typeof(CGPath))]
        public CGModuleOutputSlot OutPath = new CGModuleOutputSlot();

        #region ### Serialized Fields ###

        [SerializeField, RangeEx(-1, 1, Tooltip = "Mix between the paths. Values between -1 for Path A and 1 for Path B")]
        float m_Mix;

        #endregion

        #region ### Public Properties ###

        /// <summary>
        /// Defines how the result is interpolated. Values between -1 for Path A and 1 for Path B
        /// </summary>
        public float Mix
        {
            get { return m_Mix; }
            set
            {
                if (m_Mix != value)
                    m_Mix = value;
                Dirty = true;
            }
        }

        public bool PathIsClosed
        {
            get
            {
                return (IsConfigured) ? InPathA.SourceSlot().PathProvider.PathIsClosed &&
                                        InPathB.SourceSlot().PathProvider.PathIsClosed : false;
            }
        }

        #endregion

        #region ### Unity Callbacks ###
        /*! \cond UNITY */

        protected override void OnEnable()
        {
            base.OnEnable();
            Properties.MinWidth = 250;
            Properties.LabelWidth = 50;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Mix = m_Mix;
        }
#endif
        public override void Reset()
        {
            base.Reset();
            Mix = 0;
        }

        /*! \endcond */
        #endregion

        #region ### IOnRequestProcessing ###
        public CGData[] OnSlotDataRequest(CGModuleInputSlot requestedBy, CGModuleOutputSlot requestedSlot, params CGDataRequestParameter[] requests)
        {

            CGDataRequestRasterization raster = GetRequestParameter<CGDataRequestRasterization>(ref requests);
            if (!raster)
                return null;

            CGPath DataA = InPathA.GetData<CGPath>(requests);
            CGPath DataB = InPathB.GetData<CGPath>(requests);

            return new CGData[1] { MixPath(DataA, DataB, Mix, UIMessages) };
        }
        #endregion

        #region ### Public Static Methods ###

        /// <summary>
        /// Returns the mixed path
        /// </summary>
        /// <param name="pathA"></param>
        /// <param name="pathB"></param>
        /// <param name="mix"> A value between -1 and 1. -1 will select the path with the most points. 1 will select the other </param>
        /// <param name="warningsContainer">Is filled with warnings raised by the mixing logic</param>
        /// <returns>The mixed path</returns>
        public static CGPath MixPath(CGPath pathA, CGPath pathB, float mix, [NotNull] List<string> warningsContainer)
        {
            if (pathA == null)
                return pathB;

            if (pathB == null)
                return pathA;

            int pathVertexCount = Mathf.Max(pathA.Count, pathB.Count);

            CGPath data = new CGPath();
            data.Direction = new Vector3[pathVertexCount];//Direction is updated in the overriden call of Recalculate, which is called in InterpolateShape
            ModifierMixShapes.InterpolateShape(data, pathA, pathB, mix, warningsContainer);

            float interpolationTime = (mix + 1) * 0.5f;
            Assert.IsTrue(interpolationTime >= 0);
            Assert.IsTrue(interpolationTime <= 1);

            //BUG: Directions should be recomputed based on positions, and not interpolated. This is already done in the Recalculate() method called inside InterpolateShape() (line above), but Recalculate has a bug that makes it not compute Direction[0], so I kept the code bellow to recompute directions.
            //OPTIM avoid double computation of directions
            Vector3[] directions = new Vector3[pathVertexCount];
            if (pathA.Count == pathVertexCount)
                for (int i = 0; i < pathVertexCount; i++)
                {
                    Vector3 bDirection;
                    {
                        float frag;
                        int idx = pathB.GetFIndex(pathA.F[i], out frag);
                        bDirection = Vector3.SlerpUnclamped(pathB.Direction[idx], pathB.Direction[idx + 1], frag);
                    }

                    directions[i] = Vector3.SlerpUnclamped(pathA.Direction[i], bDirection, interpolationTime);

                }
            else
                for (int i = 0; i < pathVertexCount; i++)
                {
                    Vector3 aDirection;
                    {
                        float frag;
                        int idx = pathA.GetFIndex(pathB.F[i], out frag);
                        aDirection = Vector3.SlerpUnclamped(pathA.Direction[idx], pathA.Direction[idx + 1], frag);
                    }

                    directions[i] = Vector3.SlerpUnclamped(aDirection, pathB.Direction[i], interpolationTime);
                }

            data.Direction = directions;
            return data;
        }

        #endregion
    }
}
