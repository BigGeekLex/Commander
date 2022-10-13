using Leopotam.Ecs;
using MSFD;
using UnityEngine;

namespace Commander.CameraSystem
{
    public class TargetProviderSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsFilter<TargetSourceData> _targetFilter = null;
        public void Init()
        {
            foreach (var i in _targetFilter)
            {
                ref TargetSourceData data = ref _targetFilter.Get1(i);
                
                Messenger<Transform>.AddListener(data.TargetRegisterEventName, RegisterTarget);
                Messenger<Transform>.AddListener(data.TargetRemoveEventName, DeleteTarget);
            }
        }
        public void Destroy()
        {
            foreach (var i in _targetFilter)
            {
                ref TargetSourceData data = ref _targetFilter.Get1(i);
                
                Messenger<Transform>.RemoveListener(data.TargetRegisterEventName, RegisterTarget);
                Messenger<Transform>.RemoveListener(data.TargetRemoveEventName, DeleteTarget);
            }
        }
        private void RegisterTarget(Transform targetSource)
        {
            foreach (var i in _targetFilter)
            {
                ref TargetSourceData data = ref _targetFilter.Get1(i);

                if (!data.targets.Contains(targetSource))
                {
                    data.targets.Add(targetSource); 
                }
            }    
        }
        private void DeleteTarget(Transform target)
        {
            foreach (var i in _targetFilter)
            {
                ref TargetSourceData data = ref _targetFilter.Get1(i);

                if (data.targets.Contains(target))
                {
                    data.targets.Remove(target); 
                }
            }  
        }
    }
}