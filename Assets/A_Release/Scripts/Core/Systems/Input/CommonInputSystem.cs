using Leopotam.Ecs;
using MSFD.CnControls;
using UnityEngine;

namespace Commander.InputSystem
{
    public class CommonInputSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<InputDirectionData> _inputDirectionDataFilter = null;
        public void Run()
        {
            foreach (var i in _inputDirectionDataFilter)
            {
                ref InputDirectionData inputDirectionData = ref _inputDirectionDataFilter.Get1(i);
                
                float x = CnInputManager.GetAxis(inputDirectionData.HorizontalAxisName);
                float y = CnInputManager.GetAxis(inputDirectionData.VerticalAxisName);
                
                inputDirectionData.direction = new Vector2(x, y);
            }
        }
    }   
}
