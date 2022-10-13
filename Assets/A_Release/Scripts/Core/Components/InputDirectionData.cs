using System;
using UnityEngine;


[Serializable]
public struct InputDirectionData
{
    [SerializeField] private string horizontal;
    [SerializeField] private string vertical;
    public readonly string HorizontalAxisName => horizontal;
    public readonly string VerticalAxisName => vertical;
    
    public Vector2 direction;
}