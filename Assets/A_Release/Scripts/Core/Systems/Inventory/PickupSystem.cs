using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class PickupSystem : IEcsRunSystem
{
    private EcsWorld _world;
    
    private EcsFilter<InventoryData, PickableHolderComponent> _holderFilter;
    private EcsFilter<PickupEventComponent> _pickupEventFilter;
    public void Run()
    {
        foreach (var idx in _pickupEventFilter)
        {
            ref EcsEntity pickupRequestEntity = ref _pickupEventFilter.GetEntity(idx);
            ref EcsEntity holderEntity = ref _holderFilter.GetEntity(idx);
            
            ref InventoryData inventoryData = ref holderEntity.Get<InventoryData>();
            ref PickableHolderComponent holderComponent = ref holderEntity.Get<PickableHolderComponent>();
            ref PickupEventComponent servicedData = ref pickupRequestEntity.Get<PickupEventComponent>();
            
            if (inventoryData.storedContainer == null) inventoryData.storedContainer = new Stack<GameObject>();

            if (inventoryData.storedContainer.Count >= inventoryData.maxCapacity)
            {
                pickupRequestEntity.Destroy();
                return;
            }

            ref PickableStoredAreaComponent pickableStoredAreaComponent = ref servicedData.servicedEntity.Get<PickableStoredAreaComponent>();

            if (!pickableStoredAreaComponent.isPickableArea)
            {
                pickupRequestEntity.Destroy();
                continue;
            }
            
            if (pickableStoredAreaComponent.storedObjects == null || pickableStoredAreaComponent.storedObjects.Count <= 0)
            {
                pickupRequestEntity.Destroy();
                return;
            }
            
            GameObject nextGO;

            if (pickableStoredAreaComponent.storedObjects.TryPeek(out nextGO))
            {
                pickableStoredAreaComponent.storedObjects.Pop();
                
                if (inventoryData.canStoreDiffrentItems || inventoryData.storedContainer.Count <= 0)
                {
                    SelectNextItemPosition(nextGO, holderComponent.defaultSpawnPosition, holderComponent.defaultOffset, inventoryData.storedContainer.Count);
                }
                else
                {
                    if (inventoryData.storedContainer.Peek().GetComponent<IPickable>().GetItemType() ==
                        nextGO.GetComponent<IPickable>().GetItemType())
                    {
                        SelectNextItemPosition(nextGO, holderComponent.defaultSpawnPosition, holderComponent.defaultOffset, inventoryData.storedContainer.Count);
                    }
                    else
                    {
                        pickupRequestEntity.Destroy();
                        return;
                    }
                }
                
                pickupRequestEntity.Destroy();
                inventoryData.storedContainer.Push(nextGO);
            }
        }
    }
    private void SelectNextItemPosition(GameObject item, Transform initialHolderTransform, float defaultOffset, int containerCount)
    {
        item.transform.SetParent(initialHolderTransform);
        
        Vector3 nextSpawnPosition = initialHolderTransform.localPosition + Vector3.up * defaultOffset * containerCount; //Default offset later use mesh bounds!
                                
        float defaultDuration = 0.5f;
        
        IPickable pickable = item.GetComponent<IPickable>();
        pickable.MoveTo(nextSpawnPosition, defaultDuration);
    }
}
