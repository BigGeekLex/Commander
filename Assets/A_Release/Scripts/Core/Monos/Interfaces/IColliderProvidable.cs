using System;
using UnityEngine;

public interface IColliderProvidable
{
    public event Action<Collider> TriggerEntered;
    public event Action<Collider> TriggerExited;
}