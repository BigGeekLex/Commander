using System;
using Leopotam.Ecs;
using UnityEngine;

[Serializable]
public struct PickableSpawnEventComponent
{
    public EcsEntity Target; //EcsSpawnData
    //public Transform selectedPoint;
    public GameObject spawnedObject;
}