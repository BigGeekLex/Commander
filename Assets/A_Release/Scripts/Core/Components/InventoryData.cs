using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InventoryData
{
   public bool canStoreDiffrentItems;
   public int maxCapacity;
   public Stack<GameObject> storedContainer;
}
