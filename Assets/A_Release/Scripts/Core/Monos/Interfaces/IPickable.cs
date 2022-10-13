using UnityEngine;

public interface IPickable
{
    public Item GetItemType();

    public void MoveTo(Vector3 position, float duration);
}