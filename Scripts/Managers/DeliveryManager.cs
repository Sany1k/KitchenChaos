using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event Action OnRecipeSpawned;
    public event Action OnRecipeCompleted;
    public event Action OnRecipeSuccess;
    public event Action OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private readonly float spawnRecipeTimerMax = 6f;
    private readonly int waitingRecipesMax = 4;
    private float spawnRecipeTimer;
    private int successfulRecipesAmount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        waitingRecipeSOList = new();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke();
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKO)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRacipeSO = waitingRecipeSOList[i];

            if (waitingRacipeSO.kitchenObjectSOList.Count == plateKO.GetKitchenObjectSOList().Count)
            {
                bool plateMatchesRecipe = true;
                foreach (KitchenObjects recipeKO in waitingRacipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjects plateKOSO in plateKO.GetKitchenObjectSOList())
                    {
                        if (recipeKO == plateKOSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateMatchesRecipe = false;
                    }
                }
                if (plateMatchesRecipe)
                {
                    successfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke();
                    OnRecipeSuccess?.Invoke();
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke();
    }

    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRecipeSOList;

    public int GetSuccessfulRecipesAmount() => successfulRecipesAmount;
}
