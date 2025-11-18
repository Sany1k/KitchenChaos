using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event Action<CountersBase> OnSelectedCounterChanged;
    public event Action OnPickedSomething;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private CountersBase selectedCounter;
    private KitchenObject relatedKitchenObject;
    private Vector3 lastInteractDir;
    private readonly float playerRadius = 0.7f;
    private readonly float interactDistance = 2f;
    private bool isWalking;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += InteractAction;
        gameInput.OnAlternInteractAction += AlternInteractAction;
    }

    private void Update()
    {
        PlayerMovements();
        HandleInteractions();
    }

    public bool IsPlayerWalking() => isWalking;

    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        relatedKitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke();
        }
    }

    public KitchenObject GetKitchenObject() => relatedKitchenObject;

    public void ClearKitchenObject() => relatedKitchenObject = null;

    public bool HasKitchenObject() => relatedKitchenObject != null;

    private void PlayerMovements()
    {
        Vector3 moveDirection = new(gameInput.GetPlayerVector().x, 0, gameInput.GetPlayerVector().y);

        float moveDistance = movementSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * interactDistance, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * interactDistance, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * interactDistance, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirZ;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        isWalking = moveDirection != Vector3.zero;
    }

    private void HandleInteractions()
    {
        Vector3 moveDirection = new(gameInput.GetPlayerVector().x, 0, gameInput.GetPlayerVector().y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDir = moveDirection;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hitInfo, interactDistance, countersLayerMask))
        {
            if (hitInfo.transform.TryGetComponent(out CountersBase countersBase))
            {
                if (countersBase != selectedCounter)
                {
                    SetSelectCounter(countersBase);
                }
            }
            else
            {
                SetSelectCounter(null);
            }
        }
        else
        {
            SetSelectCounter(null);
        }
    }

    private void SetSelectCounter(CountersBase selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(selectedCounter);
    }

    private void InteractAction()
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void AlternInteractAction()
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }
}
