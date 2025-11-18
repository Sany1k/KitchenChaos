using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualList;
    private PlatesCounter platesCounter;

    private void Awake()
    {
        platesCounter = GetComponent<PlatesCounter>();
        plateVisualList = new();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlateSpawnedAction;
        platesCounter.OnPlateRemoved += PlateRemovedAction;
    }

    private void PlateSpawnedAction()
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float offsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, offsetY * plateVisualList.Count, 0);
        plateVisualList.Add(plateVisualTransform.gameObject);
    }

    private void PlateRemovedAction()
    {
        GameObject lastPlateVisual = plateVisualList[^1];
        plateVisualList.Remove(lastPlateVisual);
        Destroy(lastPlateVisual);
    }
}
