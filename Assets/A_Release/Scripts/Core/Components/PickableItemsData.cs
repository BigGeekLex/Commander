using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct PickableItemsData
{
    [SerializeField]
    private List<GameObject> itemsPrefabsSource;
    public readonly List<GameObject> ItemsPrefabs => itemsPrefabsSource;
}