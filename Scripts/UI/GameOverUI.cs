using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    public event Action OnGameRestartEvent;

    [SerializeField] private TextMeshProUGUI recipesDiliveredText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        restartButton.onClick.AddListener(() => 
        {
            OnGameRestartEvent?.Invoke();
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += StateChangedAction;
        Hide();
    }

    private void StateChangedAction()
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            recipesDiliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            Show();
        }
        else
        {
            Hide();
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
