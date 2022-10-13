using Leopotam.Ecs;

public interface IEntityProvidable
{
    public void SetTargetEntity(EcsEntity entity);
    public EcsEntity GetTargetEntity();
}