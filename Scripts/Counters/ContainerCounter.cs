using System;
using UnityEngine;

public class ContainerCounter : CountersBase
{
    public Action OnPlayerGrabbedObject; 

    [SerializeField] private KitchenObjects kitchenObjects;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjects, player);
            OnPlayerGrabbedObject?.Invoke();
        }
    }
}
