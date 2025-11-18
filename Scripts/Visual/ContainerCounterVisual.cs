using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        containerCounter = GetComponent<ContainerCounter>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += PlayerGrabbedObjectEvent;
    }

    private void PlayerGrabbedObjectEvent()
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
