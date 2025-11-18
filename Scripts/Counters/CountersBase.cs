using System;
using UnityEngine;

public class CountersBase : MonoBehaviour, IKitchenObjectParent
{
    public static event Action<CountersBase> OnAnyObjectPlacedHere;

    [SerializeField] private Transform counterTopPoint;

    protected KitchenObject relatedKitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    public virtual void Interact(Player player) { }
    public virtual void InteractAlternate(Player player) { }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        relatedKitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return relatedKitchenObject;
    }

    public void ClearKitchenObject()
    {
        relatedKitchenObject = null;
    }
}
