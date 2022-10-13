using Leopotam.Ecs;
using UnityEngine;

public class SpawnRequestSenderSystem : IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter<SpawnerTimerData> _timerFilter;
    public void Run()
    {
        foreach (var i in _timerFilter)
        {
            ref SpawnerTimerData timeData = ref _timerFilter.Get1(i);
            
            if (timeData.timeIntervalBetweenSpawn <= timeData.lastTime)
            {
                timeData.lastTime = 0.0f;
                
                var entity = _world.NewEntity();
                ref SpawnRequest requester = ref entity.Get<SpawnRequest>();
            }
            else
            {
                float calculatedTime = timeData.lastTime + Time.fixedDeltaTime;
                timeData.lastTime = calculatedTime;
            }
        }
    }
}