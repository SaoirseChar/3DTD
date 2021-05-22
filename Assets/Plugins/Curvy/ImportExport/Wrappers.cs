// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace FluffyUnderware.Curvy.ImportExport
{
    /// <summary>
    /// A wrapper to the CurvySpline class
    /// </summary>
    [Serializable]
    public class SerializedCurvySpline
    {
        public string Name;
        public Vector3 Position;
        public Vector3 Rotation;
        public CurvyInterpolation Interpolation;
        public bool RestrictTo2D;
        public bool Closed;
        public bool AutoEndTangents;
        public CurvyOrientation Orientation;
        public float AutoHandleDistance;
        public int CacheDensity;
        public float MaxPointsPerUnit;
        public bool UsePooling;
        public bool UseThreading;
        public bool CheckTransform;
        public CurvyUpdateMethod UpdateIn;
        public SerializedCurvySplineSegment[] ControlPoints;

        public SerializedCurvySpline()
        {
            Interpolation = CurvyGlobalManager.DefaultInterpolation;
            AutoEndTangents = CurvySplineDefaultValues.AutoEndTangents;
            Orientation = CurvySplineDefaultValues.Orientation;
            AutoHandleDistance = CurvySplineDefaultValues.AutoHandleDistance;
            CacheDensity = CurvySplineDefaultValues.CacheDensity;
            MaxPointsPerUnit = CurvySplineDefaultValues.MaxPointsPerUnit;
            UsePooling = CurvySplineDefaultValues.UsePooling;
            CheckTransform = CurvySplineDefaultValues.CheckTransform;
            UpdateIn = CurvySplineDefaultValues.UpdateIn;
            ControlPoints = new SerializedCurvySplineSegment[0];
           
        }

        public SerializedCurvySpline([NotNull]CurvySpline spline, CurvySerializationSpace space)
        {
            Name = spline.name;
            Position = (space == CurvySerializationSpace.Local)
                ? spline.transform.localPosition
                : spline.transform.position;
            Rotation = (space == CurvySerializationSpace.Local)
                ? spline.transform.localRotation.eulerAngles
                : spline.transform.rotation.eulerAngles;
            Interpolation = spline.Interpolation;
            RestrictTo2D = spline.RestrictTo2D;
            Closed = spline.Closed;
            AutoEndTangents = spline.AutoEndTangents;
            Orientation = spline.Orientation;
            AutoHandleDistance = spline.AutoHandleDistance;
            CacheDensity = spline.CacheDensity;
            MaxPointsPerUnit = spline.MaxPointsPerUnit;
            UsePooling = spline.UsePooling;
            UseThreading = spline.UseThreading;
            CheckTransform = spline.CheckTransform;
            UpdateIn = spline.UpdateIn;
            ControlPoints = new SerializedCurvySplineSegment[spline.ControlPointCount];
            for (int i = 0; i < spline.ControlPointCount; i++)
                ControlPoints[i] = new SerializedCurvySplineSegment(spline.ControlPointsList[i], space);
        }

        /// <summary>
        /// Fills an existing spline with data from this instance
        /// </summary>
        /// <remarks>This method will dirty the spline</remarks>
        public void WriteIntoSpline([NotNull]CurvySpline deserializedSpline, CurvySerializationSpace space)
        {
            deserializedSpline.name = Name;
            if (space == CurvySerializationSpace.Local)
            {
                deserializedSpline.transform.localPosition = Position;
                deserializedSpline.transform.localRotation = Quaternion.Euler(Rotation);
            }
            else
            {
                deserializedSpline.transform.position = Position;
                deserializedSpline.transform.rotation = Quaternion.Euler(Rotation);
            }
            deserializedSpline.Interpolation = Interpolation;
            deserializedSpline.RestrictTo2D = RestrictTo2D;
            deserializedSpline.Closed = Closed;
            deserializedSpline.AutoEndTangents = AutoEndTangents;
            deserializedSpline.Orientation = Orientation;
            deserializedSpline.AutoHandleDistance = AutoHandleDistance;
            deserializedSpline.CacheDensity = CacheDensity;
            deserializedSpline.MaxPointsPerUnit = MaxPointsPerUnit;
            deserializedSpline.UsePooling = UsePooling;
            deserializedSpline.UseThreading = UseThreading;
            deserializedSpline.CheckTransform = CheckTransform;
            deserializedSpline.UpdateIn = UpdateIn;

            foreach (SerializedCurvySplineSegment serializedControlPoint in ControlPoints)
                serializedControlPoint.WriteIntoControlPoint(deserializedSpline.InsertAfter(null, true), space);

            deserializedSpline.SetDirtyAll();
        }
    }

    /// <summary>
    /// Serialized Control Point
    /// </summary>
    [Serializable]
    public class SerializedCurvySplineSegment
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public bool AutoBakeOrientation;
        public bool OrientationAnchor;
        public CurvyOrientationSwirl Swirl;
        public float SwirlTurns;
        public bool AutoHandles;
        public float AutoHandleDistance;
        public Vector3 HandleOut;
        public Vector3 HandleIn;

        public SerializedCurvySplineSegment()
        {
           
            Swirl = CurvySplineSegmentDefaultValues.Swirl;
            AutoHandles = CurvySplineSegmentDefaultValues.AutoHandles;
            AutoHandleDistance = CurvySplineSegmentDefaultValues.AutoHandleDistance;
            HandleOut = CurvySplineSegmentDefaultValues.HandleOut;
            HandleIn = CurvySplineSegmentDefaultValues.HandleIn;
        }

        public SerializedCurvySplineSegment([NotNull]CurvySplineSegment segment, CurvySerializationSpace space)
        {
            Position = (space == CurvySerializationSpace.Global)
                ? segment.transform.position
                : segment.transform.localPosition;
            Rotation = (space == CurvySerializationSpace.Global)
                ? segment.transform.rotation.eulerAngles
                : segment.transform.localRotation.eulerAngles;
            AutoBakeOrientation = segment.AutoBakeOrientation;
            OrientationAnchor = segment.SerializedOrientationAnchor;
            Swirl = segment.Swirl;
            SwirlTurns = segment.SwirlTurns;
            AutoHandles = segment.AutoHandles;
            AutoHandleDistance = segment.AutoHandleDistance;
            HandleOut = segment.HandleOut;
            HandleIn = segment.HandleIn;
        }

        /// <summary>
        /// Fills an existing control point with data from this instance.
        /// </summary>
        public void WriteIntoControlPoint([NotNull]CurvySplineSegment controlPoint, CurvySerializationSpace space)
        {
            if (space == CurvySerializationSpace.Global)
            {
                controlPoint.transform.position = Position;
                controlPoint.transform.rotation = Quaternion.Euler(Rotation);
            }
            else
            {
                controlPoint.transform.localPosition = Position;
                controlPoint.transform.localRotation = Quaternion.Euler(Rotation);
            }
            controlPoint.AutoBakeOrientation = AutoBakeOrientation;
            controlPoint.SerializedOrientationAnchor = OrientationAnchor;
            controlPoint.Swirl = Swirl;
            controlPoint.SwirlTurns = SwirlTurns;
            controlPoint.AutoHandles = AutoHandles;
            controlPoint.AutoHandleDistance = AutoHandleDistance;
            controlPoint.SetBezierHandleIn(HandleIn);
            controlPoint.SetBezierHandleOut(HandleOut);
        }
    }
}
