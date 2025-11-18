using System;
using UnityEngine;

public class CuttingCounter : CountersBase, IHasProgress
{
    public static event Action<CuttingCounter> OnAnyCut;
    public event Action<float> OnProgressChanged;
    public event Action OnCut;

    [SerializeField] private CuttingRecipe[] cuttingRecipeArray;

    private int cuttingProgress;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(Player player)
    {
        if (relatedKitchenObject == null)
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    float progressNormalized = (float)cuttingProgress / GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax;
                    OnProgressChanged?.Invoke(progressNormalized);
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (relatedKitchenObject != null && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;
            OnCut?.Invoke();
            OnAnyCut?.Invoke(this);
            CuttingRecipe cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
            float progressNormalized = (float)cuttingProgress / cuttingRecipe.cuttingProgressMax;
            OnProgressChanged?.Invoke(progressNormalized);

            if (cuttingProgress >= cuttingRecipe.cuttingProgressMax)
            {
                KitchenObjects ko = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(ko, this);
            }
        }
    }

    private KitchenObjects GetOutputForInput(KitchenObjects inputKitchenObject)
    {
        CuttingRecipe cuttingRecipe = GetCuttingRecipeWithInput(inputKitchenObject);

        if (cuttingRecipe != null)
        {
            return cuttingRecipe.output;
        }
        else
        {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjects inputKitchenObject)
    {
        CuttingRecipe cuttingRecipe = GetCuttingRecipeWithInput(inputKitchenObject);

        return cuttingRecipe != null;
    }

    private CuttingRecipe GetCuttingRecipeWithInput(KitchenObjects inputKitchenObject)
    {
        foreach (CuttingRecipe recipe in cuttingRecipeArray)
        {
            if (recipe.input == inputKitchenObject)
            {
                return recipe;
            }
        }

        return null;
    }
}
