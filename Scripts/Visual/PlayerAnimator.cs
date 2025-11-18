using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string WALKING = "IsWalking";

    [SerializeField] private Player player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(WALKING, player.IsPlayerWalking());
    }
}
