using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickableSpawnerSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    
    private EcsFilter<PickableItemsData> _itemsFilter;
    private EcsFilter<SpawnRequest> _requestFilter;
    
    private EcsFilter<PickableStoredAreaComponent> _pointFilter;
    public void Run()
    {
        foreach (var i in _requestFilter)
        {
            var requestedEntity = _requestFilter.GetEntity(i);
            var itemsEntity = _itemsFilter.GetEntity(i);
            
            foreach (var idx in _pointFilter)
            {
                var pointEntity = _pointFilter.GetEntity(idx);

                ref PickableItemsData itemsData = ref itemsEntity.Get<PickableItemsData>();
                ref PickableStoredAreaComponent pointComponent= ref pointEntity.Get<PickableStoredAreaComponent>();

                int randomElementIndex = Random.Range(0, itemsData.ItemsPrefabs.Count);
            
                if (pointComponent.storedObjects == null)
                { 
                    pointComponent.storedObjects = new Stack<GameObject>();
                }

                if (pointComponent.storedObjects.Count >= pointComponent.MaxCapacity)
                {
                    continue;
                }

                if (!pointComponent.isPickableArea)
                {
                    continue;
                }
                
                GameObject selectedPickableGo;
                
                if (pointComponent.CanPlaceDiffrentItems || pointComponent.storedObjects.Count <= 0)
                {
                    selectedPickableGo = itemsData.ItemsPrefabs[randomElementIndex];
                }
            
                else
                {
                    var data = pointComponent;
                    selectedPickableGo = itemsData.ItemsPrefabs.
                        Find((x) => x.GetComponent<IPickable>().GetItemType() ==
                                    data.storedObjects.Peek()
                                        .GetComponent<IPickable>()
                                        .GetItemType());
                }

                GameObject spawned = SpawnObject(selectedPickableGo, pointComponent.SpawnPoint.position, pointComponent.SpawnPoint);
                pointComponent.storedObjects.Push(spawned);
            }
            requestedEntity.Destroy();   
        }
    }

    private GameObject SpawnObject(GameObject go, Vector3 pos, Transform parent)
    {
        return PC.Instantiate(go, pos, Quaternion.identity, parent);
    }

    public void Init()
    {
        foreach (var i in _pointFilter)
        {
            EcsEntity pointEntity = _pointFilter.GetEntity(i);
            
            ref PickableStoredAreaComponent pickableStoredAreaComponent = ref pointEntity.Get<PickableStoredAreaComponent>();
            pickableStoredAreaComponent.EntityProvider.SetTargetEntity(pointEntity);
        }
    }
}