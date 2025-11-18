using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += StateChangedAction;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void StateChangedAction()
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }
}
