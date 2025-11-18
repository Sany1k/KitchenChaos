using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject selectVisual;
    
    private CountersBase baseCounter;

    private void Start()
    {
        baseCounter = GetComponent<CountersBase>();
        Player.Instance.OnSelectedCounterChanged += SelectedCounterChanged;
    }

    private void ShowVisual()
    {
        selectVisual.SetActive(true);
    }

    private void HideVisual()
    {
        selectVisual.SetActive(false);
    }

    private void SelectedCounterChanged(CountersBase sender)
    {
        if (sender == baseCounter)
        {
            ShowVisual();
        }
        else
        {
            HideVisual();
        }
    }
}
