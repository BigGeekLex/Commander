using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class DropSystem : IEcsRunSystem
{
    private EcsWorld _world;
    
    private EcsFilter<InventoryData>.Exclude<PickableHolderComponent> _holderFilter;
    private EcsFilter<DropEventComponent> _dropEventFilter;
    private EcsFilter<PickableStoredAreaComponent> _dropFilter;
    public void Run()
    {
        foreach (var idx in _dropEventFilter)
        {
            ref EcsEntity dropRequestEntity = ref _dropEventFilter.GetEntity(idx);
            ref EcsEntity holderEntity = ref _holderFilter.GetEntity(idx);
            
            ref InventoryData inventoryData = ref holderEntity.Get<InventoryData>();
            ref DropEventComponent servicedData = ref dropRequestEntity.Get<DropEventComponent>();
            
            if (inventoryData.storedContainer == null || inventoryData.storedContainer.Count <= 0)
            {
                dropRequestEntity.Destroy();
                return;
            }

            ref PickableStoredAreaComponent dropableStoredAreaComponent = ref servicedData.servicedEntity.Get<PickableStoredAreaComponent>();

            if (dropableStoredAreaComponent.isPickableArea)
            {
                dropRequestEntity.Destroy();
                continue;
            }
            
            if (dropableStoredAreaComponent.storedObjects == null)
            {
                dropableStoredAreaComponent.storedObjects = new Stack<GameObject>();
            }

            if (dropableStoredAreaComponent.storedObjects.Count >= dropableStoredAreaComponent.MaxCapacity)
            {
                dropRequestEntity.Destroy();
                return;
            }
            
            GameObject nextGO;
            
            if (inventoryData.storedContainer.TryPeek(out nextGO))
            {
                if (dropableStoredAreaComponent.CanPlaceDiffrentItems || dropableStoredAreaComponent.storedObjects.Count <= 0)
                {
                    SelectNextItemPosition(nextGO, dropableStoredAreaComponent.SpawnPoint, 0.0f, 0);
                }
                else
                {
                    if (dropableStoredAreaComponent.storedObjects.Peek().GetComponent<IPickable>().GetItemType() 
                        != nextGO.GetComponent<IPickable>().GetItemType())
                    {
                        dropRequestEntity.Destroy();
                        return;
                    }
                    else
                    {
                        SelectNextItemPosition(nextGO, dropableStoredAreaComponent.SpawnPoint, 0.0f, 0);
                    }
                }
                
                inventoryData.storedContainer.Pop();
                dropableStoredAreaComponent.storedObjects.Push(nextGO);
            }
            dropRequestEntity.Destroy();
        }
    }
    private void SelectNextItemPosition(GameObject item, Transform initialHolderTransform, float defaultOffset, int containerCount)
    {
        item.transform.SetParent(initialHolderTransform);
        
        Vector3 nextSpawnPosition = initialHolderTransform.localPosition + Vector3.up * defaultOffset * containerCount;
                                
        float defaultDuration = 0.25f;
        
        IPickable pickable = item.GetComponent<IPickable>();
        pickable.MoveTo(nextSpawnPosition, defaultDuration);
    }
}