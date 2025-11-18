using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    private CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        cuttingCounter = GetComponent<CuttingCounter>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CutEvent;
    }

    private void CutEvent()
    {
        animator.SetTrigger(CUT);
    }
}
