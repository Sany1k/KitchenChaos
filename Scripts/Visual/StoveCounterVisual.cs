using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesOnGameObject;

    private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter = GetComponent<StoveCounter>();
        stoveCounter.OnStoveEventChanged += StoveEventChangedEvent;
    }

    private void StoveEventChangedEvent(CookingState state)
    {
        bool showVisual = state == CookingState.Frying || state == CookingState.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesOnGameObject.SetActive(showVisual);
    }
}
