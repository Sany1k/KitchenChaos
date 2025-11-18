using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private readonly float footstepTimerMax = 0.1f;
    private readonly float volume = 1f;
    private float footstepTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            if (player.IsPlayerWalking())
            {
                footstepTimer = footstepTimerMax;
                SoundManager.Instance.PlayFootstepsSound(transform.position, volume);
            }
        }
    }
}
