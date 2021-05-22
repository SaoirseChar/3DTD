// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;

namespace FluffyUnderware.Curvy
{
    /// <summary>
    /// Base class for all Metadata classes.
    /// A Metadata is a custom data attached to a Control Point
    /// </summary>
    [RequireComponent(typeof(CurvySplineSegment))]
    [ExecuteInEditMode]
    public abstract class CurvyMetadataBase : MonoBehaviour
    {
        #region ### Serialized Fields ###
        #endregion

        #region ### Public Properties ###

        public CurvySplineSegment ControlPoint
        {
            get { return mCP; }
        }

        public CurvySpline Spline
        {
            get
            {
                //DESIGN should this throw an exception if mCP is null?
                return (mCP) ? mCP.Spline : null;
            }
        }

        #endregion

        #region ### Private Fields & Properties ###

        CurvySplineSegment mCP;

        #endregion

        #region ### Unity Callbacks ###
        /*! \cond UNITY */

        protected virtual void Awake()
        {
            mCP = GetComponent<CurvySplineSegment>();
            mCP.RegisterMetaData(this);
        }

        private void OnDestroy()
        {
            mCP.UnregisterMetaData(this);
        }

        /*! \endcond */
        #endregion

        #region ### Public Methods ###

        public T GetPreviousData<T>(bool autoCreate = true, bool segmentsOnly = true, bool useFollowUp = false) where T : CurvyMetadataBase
        {
            if (ControlPoint)
            {
                CurvySplineSegment controlPoint = ControlPoint;
                CurvySpline spline = Spline;


                CurvySplineSegment previousControlPoint;
                if (!spline || spline.ControlPointsList.Count == 0)
                    previousControlPoint = null;
                else
                {
                    previousControlPoint = useFollowUp
                        ? spline.GetPreviousControlPointUsingFollowUp(controlPoint)
                        : spline.GetPreviousControlPoint(controlPoint);

                    if (segmentsOnly && previousControlPoint && previousControlPoint.Spline.IsControlPointASegment(previousControlPoint) == false)
                        previousControlPoint = null;
                }

                if (previousControlPoint)
                    return previousControlPoint.GetMetadata<T>(autoCreate);
            }
            return default(T);
        }

        public T GetNextData<T>(bool autoCreate = true, bool segmentsOnly = true, bool useFollowUp = false) where T : CurvyMetadataBase
        {
            if (ControlPoint)
            {
                CurvySplineSegment controlPoint = ControlPoint;
                CurvySpline spline = Spline;

                CurvySplineSegment nextControlPoint;
                if (!spline || spline.ControlPointsList.Count == 0)
                    nextControlPoint = null;
                else
                {
                    nextControlPoint = useFollowUp
                        ? spline.GetNextControlPointUsingFollowUp(controlPoint)
                        : spline.GetNextControlPoint(controlPoint);

                    if (segmentsOnly && nextControlPoint && nextControlPoint.Spline.IsControlPointASegment(nextControlPoint) == false)
                        nextControlPoint = null;
                }

                if (nextControlPoint)
                    return nextControlPoint.GetMetadata<T>(autoCreate);
            }
            return default(T);
        }

        /// <summary>
        /// Call this to make the owner spline send an event to notify its listeners of the change in the spline data.
        /// </summary>
        protected void NotifyModification()
        {
            CurvySpline spline = Spline;
            if (spline && spline.IsInitialized)
                spline.NotifyMetaDataModification();
        }

        #endregion

        #region ### Privates ###
        /*! \cond PRIVATES */


        /*! \endcond */
        #endregion

    }
}
