using System;
using Commander.CameraSystem;
using Commander.InputSystem;
using Leopotam.Ecs;
using MSFD;
using UniRx;
using Voody.UniLeo;

public class Initializer : SingletoneBase<Initializer>, IObservable<EcsWorld>
{
    private EcsWorld _world;
    private EcsSystems _systems;

    private ReactiveProperty<EcsWorld> _worldProperty = new ReactiveProperty<EcsWorld>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void AwakeInitialization()
    {
        base.AwakeInitialization();
        
        _world = new EcsWorld ();
        _worldProperty.SetValueAndForceNotify(_world);
        
        _systems = new EcsSystems (_world).ConvertScene().Add (new CommonInputSystem());
        _systems.Add(new MovementSystem());
        _systems.Add(new TargetProviderSystem());
        _systems.Add(new CameraFollowSystem());
        _systems.Add(new PickableSpawnerSystem());
        _systems.Add(new PickupSystem());
        _systems.Add(new SpawnRequestSenderSystem());
        _systems.Add(new DropSystem());
        _systems.OneFrame<PickableSpawnEventComponent>();
        _systems.OneFrame<PickupEventComponent>();

        _systems.Init ();
    }
    private void FixedUpdate () {
        _systems.Run();
    }
    private void OnDestroy () {
        _systems.Destroy ();
        _world.Destroy();
    }

    public IDisposable Subscribe(IObserver<EcsWorld> observer)
    {
        observer.OnNext(_worldProperty.Value);
        return _worldProperty.Subscribe(observer);
    }
}
