using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjects kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjects GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parentObject)
    {
        kitchenObjectParent?.ClearKitchenObject();
        kitchenObjectParent = parentObject;
        parentObject.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent; 
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        plateKitchenObject = this as PlateKitchenObject;
        return this is PlateKitchenObject;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjects kitchenObjects, IKitchenObjectParent parent)
    {
        Transform objTransform = Instantiate(kitchenObjects.prefab);
        KitchenObject kitchenObject = objTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(parent);

        return kitchenObject;
    }
}
