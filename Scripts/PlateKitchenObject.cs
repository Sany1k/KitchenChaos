using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event Action<KitchenObjects> OnIngredientAdded;

    [SerializeField] private List<KitchenObjects> validKitchenObjectSOList;

    private List<KitchenObjects> kitchenObjectsList;

    private void Awake()
    {
        kitchenObjectsList = new();
    }

    public bool TryAddIngredient(KitchenObjects kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectsList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectsList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(kitchenObjectSO);
            return true;
        }
    }

    public List<KitchenObjects> GetKitchenObjectSOList()
    {
        return kitchenObjectsList;
    }
}
