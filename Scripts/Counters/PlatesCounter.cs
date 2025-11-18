using System;
using UnityEngine;

public class PlatesCounter : CountersBase
{
    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;

    [SerializeField] private KitchenObjects plateKitchenObjectSO;

    private readonly float spawnPlateTimerMax = 4f;
    private readonly int platesSpawnAmountMax = 4;
    private float spawnPlateTimer;
    private int platesSpawnAmount;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;
                OnPlateSpawned?.Invoke();
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnAmount > 0)
            {
                platesSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke();
            }
        }
    }
}
