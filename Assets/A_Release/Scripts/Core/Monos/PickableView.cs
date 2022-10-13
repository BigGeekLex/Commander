using DG.Tweening;
using UnityEngine;

public class PickableView : MonoBehaviour, IPickable
{
    [SerializeField] 
    private Item type;
    public Item GetItemType()
    {
        return type;
    }
    public void MoveTo(Vector3 position, float duration)
    {
        transform.DOLocalMove(position, duration);
    }
}