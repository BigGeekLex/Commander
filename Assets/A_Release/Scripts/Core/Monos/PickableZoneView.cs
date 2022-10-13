using Leopotam.Ecs;
using UnityEngine;

public class PickableZoneView : MonoBehaviour, IPickableZone
{
    private EcsEntity _target;
    
    public void SetTargetEntity(EcsEntity entity)
    {
        _target = entity;
    }

    public EcsEntity GetTargetEntity()
    {
        return _target;
    }
}