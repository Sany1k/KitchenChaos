using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game object does't have interface");
        }

        hasProgress.OnProgressChanged += ProgressChangedEvent;
        transform.forward = Camera.main.transform.forward;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void ProgressChangedEvent(float progress)
    {
        barImage.fillAmount = progress;

        if (progress == 0f || progress == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
