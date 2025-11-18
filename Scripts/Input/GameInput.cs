using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    public event Action OnInteractAction;
    public event Action OnAlternInteractAction;
    public event Action OnPauseAction;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        inputActions = new();
        inputActions.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.AlternInteract.performed += AlternInteract_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.AlternInteract.performed -= AlternInteract_performed;
        inputActions.Player.Pause.performed -= Pause_performed;
        inputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext callback)
    {
        OnPauseAction?.Invoke();
    }

    private void Interact_performed(InputAction.CallbackContext callback)
    {
        OnInteractAction?.Invoke();
    }

    private void AlternInteract_performed(InputAction.CallbackContext callback)
    {
        OnAlternInteractAction?.Invoke();
    }

    public Vector2 GetPlayerVector()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }
}
