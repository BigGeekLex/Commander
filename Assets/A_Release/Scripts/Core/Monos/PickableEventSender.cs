using System;
using Leopotam.Ecs;
using MSFD;
using UniRx;
using UnityEngine;

public class PickableEventSender : MonoBehaviour
{
    [SerializeField] 
    private InterfaceField<IColliderProvidable> providerSource;
    
    [SerializeField] 
    private DetectInfo detectInfo;
    [SerializeField] 
    private PickableEventType type;

    private IColliderProvidable Providable => providerSource.i;
    private IObservable<EcsWorld> _worldObservableSource;
    private EcsWorld _ecsWorld;
    private float _defaultRepeatRate = 1.0f;

    private EcsEntity _currentEntity;
    private void Start()
    {
        if (Providable != null)
        {
            Providable.TriggerEntered += OnTriggerEnterHandler;
            Providable.TriggerExited += OnTriggerExitHandler;
        }
        else
        {
            Debug.LogError("Please attach IColliderProvidable link to GO");
            return;
        }
        _worldObservableSource = Initializer.Instance;
        
        if (_worldObservableSource != null)
        {
            _worldObservableSource.Subscribe((x) => _ecsWorld = x).AddTo(this);
        }
    }

    private void OnDestroy()
    {
        if (Providable != null)
        {
            Providable.TriggerEntered -= OnTriggerEnterHandler;
            Providable.TriggerExited -= OnTriggerExitHandler; 
        }
    }

    private void OnTriggerEnterHandler(Collider other)
    {
        if (detectInfo.IsTargetCorrect(other))
        {
            IEntityProvidable entityProvidable;

            if (other.gameObject.TryGetComponent(out entityProvidable))
            {
                _currentEntity = entityProvidable.GetTargetEntity();
                InvokeRepeating(nameof(SendRequest), _defaultRepeatRate, _defaultRepeatRate);   
            }
        }
    }
    private void OnTriggerExitHandler(Collider other)
    {
        if (detectInfo.IsTargetCorrect(other))
        {
            CancelInvoke();   
        }
    }
    private void SendRequest()
    {
        if (_ecsWorld != null)
        {
            var entity = _ecsWorld.NewEntity();
            
            if (type == PickableEventType.Pickup)
            {
                ref PickupEventComponent pickupEventRequest = ref entity.Get<PickupEventComponent>();
                pickupEventRequest.servicedEntity = _currentEntity;
            }
            
            if(type == PickableEventType.Drop)
            {
                ref DropEventComponent dropEventRequest = ref entity.Get<DropEventComponent>();
                dropEventRequest.servicedEntity = _currentEntity;
            }
        } 
    }
}
