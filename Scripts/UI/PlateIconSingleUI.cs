using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjects kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
