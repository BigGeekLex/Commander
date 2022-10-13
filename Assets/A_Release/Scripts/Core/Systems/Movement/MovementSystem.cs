using Leopotam.Ecs;
using UnityEngine;


public class MovementSystem : IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter<MovementData, InputDirectionData, Movable> _moveFilter;
    public void Run()
    {
        foreach (var i in _moveFilter)
        {
            ref MovementData movementData = ref _moveFilter.Get1(i);
            ref InputDirectionData directionData = ref _moveFilter.Get2(i);
            ref Movable movable = ref _moveFilter.Get3(i);
            
            Vector3 inputDirection = VectorConverter.ConvertVector2ToVector3(directionData.direction, true);

            if (inputDirection.magnitude > 0)
            {
                Vector3 previousPosition = movable.rb.position;
                Vector3 nextPosition = previousPosition + movementData.movementSpeed * inputDirection * Time.fixedDeltaTime;
                
                movable.rb.MovePosition(nextPosition);
                //Refactor Rotation Logic out of this
                movable.rb.MoveRotation(Quaternion.LookRotation( nextPosition-previousPosition, Vector3.up));   
            }
        }
    }
}