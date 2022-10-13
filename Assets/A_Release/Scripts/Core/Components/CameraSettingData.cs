using System;
using MSFD;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;


[Serializable]
public struct CameraSettingData
{
    [SerializeField] 
    private Camera camera;
    [SerializeField] 
    private Transform cameraRootTr;
    [SerializeField]
    private float dampTime; // Approximate time for the camera to refocus.
    [SerializeField]
    private float screenEdgeBuffer; // Space between the top/bottom most target and the screen edge.
    [SerializeField]
    private float minHeight;  // The smallest orthographic size the camera can be.
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float heightMultiplyer;
    [SerializeField]
    private float clampTargetBuffer;
    [SerializeField]
    private bool isNeedToClampTargets;
    //Space restrictions! Could be chnaged dynamicaly
    [HorizontalGroup("XClamp")]
    public float minXCoord;
    [HorizontalGroup("XClamp")]
    public float maxXCoord;
    [HorizontalGroup("ZClamp")]
    public float minZCoord;
    [HorizontalGroup("ZClamp")]
    public float maxZCoord;
    
    public Vector3 moveVelocity;
    public readonly Camera Camera => camera;
    public readonly Transform CameraRootTr => cameraRootTr;
    public readonly float DampTime => dampTime;
    public readonly float ScreenEdgeBuffer => screenEdgeBuffer;
    public readonly float MinHeight => minHeight;
    public readonly float MaxHeight => maxHeight;
    public readonly float HeightMultiplyer => heightMultiplyer;
    public readonly float ClampTargetBuffer => clampTargetBuffer;
    public readonly bool IsNeedToClampTargets => isNeedToClampTargets;
}