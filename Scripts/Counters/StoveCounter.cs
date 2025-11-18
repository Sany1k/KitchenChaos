using System;
using UnityEngine;

public enum CookingState
{
    None,
    Frying,
    Fried,
    Burned
}

public class StoveCounter : CountersBase, IHasProgress
{
    public event Action<float> OnProgressChanged;
    public event Action<CookingState> OnStoveEventChanged;

    [SerializeField] private FryingRecipe fryingRecipe;
    [SerializeField] private FryingRecipe burnedRecipe;

    private CookingState cookingState;
    private float fryingTimer;
    private float burningTimer;

    private void Update()
    {
        if (relatedKitchenObject != null) 
        {
            switch (cookingState)
            {
                case CookingState.None:
                    break;
                case CookingState.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(fryingTimer / fryingRecipe.fryingTimerMax);

                    if (fryingTimer > fryingRecipe.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);
                        cookingState = CookingState.Fried;
                        burningTimer = 0f;
                        OnStoveEventChanged?.Invoke(cookingState);
                    }
                    break;
                case CookingState.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(burningTimer / burnedRecipe.fryingTimerMax);

                    if (burningTimer > burnedRecipe.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burnedRecipe.output, this);
                        cookingState = CookingState.Burned;
                        OnStoveEventChanged?.Invoke(cookingState);
                        OnProgressChanged?.Invoke(0f);
                    }
                    break;
                case CookingState.Burned:
                    break;
            }
        }
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
                    cookingState = CookingState.Frying;
                    fryingTimer = 0f;
                    OnStoveEventChanged?.Invoke(cookingState);
                    OnProgressChanged?.Invoke(fryingTimer / fryingRecipe.fryingTimerMax);
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
                        cookingState = CookingState.None;
                        OnStoveEventChanged?.Invoke(cookingState);
                        OnProgressChanged?.Invoke(0f);
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingState = CookingState.None;
                OnStoveEventChanged?.Invoke(cookingState);
                OnProgressChanged?.Invoke(0f);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjects inputKitchenObject) => fryingRecipe.input == inputKitchenObject;
}
