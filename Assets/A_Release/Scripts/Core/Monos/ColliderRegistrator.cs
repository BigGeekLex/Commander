using System;
using UnityEngine;

public class ColliderRegistrator : MonoBehaviour, IColliderProvidable
{
    public event Action<Collider> TriggerEntered;
    public event Action<Collider> TriggerExited;
    
    private void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExited?.Invoke(other);
    }
}