using System;
using System.Collections.Generic;
using MSFD;
using UnityEngine;

[Serializable]
public struct PickableStoredAreaComponent
{
    [SerializeField] 
    private InterfaceField<IEntityProvidable> entityProviderSource;
    
    [SerializeField] 
    private int maxCapacity;
    
    [SerializeField] 
    private bool canPlaceDiffrentItems;
    
    [SerializeField] 
    private Transform spawnPoint;
    public bool CanPlaceDiffrentItems => canPlaceDiffrentItems;
    public IEntityProvidable EntityProvider => entityProviderSource.i;
    public int MaxCapacity => maxCapacity;
    
    public Stack<GameObject> storedObjects;
    public Transform SpawnPoint => spawnPoint;

    public bool isPickableArea;
}