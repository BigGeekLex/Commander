using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TargetSourceData
{
    [SerializeField] 
    private string targetRegisterEventName;
    [SerializeField] 
    private string targetRemoveEventName;
    
    public List<Transform> targets;
    public readonly string TargetRegisterEventName => targetRegisterEventName;
    public readonly string TargetRemoveEventName => targetRemoveEventName;
}