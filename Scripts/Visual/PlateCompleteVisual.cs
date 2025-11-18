using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct KitchenObjectSO_GameObject
{
    public KitchenObjects kitchenObjectSO;
    public GameObject gameObject;
}
public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> objectSOList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += IngredientAddedEvent;
        foreach (var ko in objectSOList)
        {
            ko.gameObject.SetActive(false);
        }
    }

    private void IngredientAddedEvent(KitchenObjects kitchenObjects)
    {
        objectSOList.Find(ko => ko.kitchenObjectSO == kitchenObjects).gameObject.SetActive(true);
    }
}
