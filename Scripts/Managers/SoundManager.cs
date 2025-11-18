using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private const string SOUND_VOLUME_SAVE = "SoundEffectsVolume";

    [SerializeField] private AudioClipRefs audioClipRefs;

    private Vector3 deliveryCounterPos;
    private float volume;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        volume = PlayerPrefs.GetFloat(SOUND_VOLUME_SAVE, 1f);
    }

    private void Start()
    {
        deliveryCounterPos = DeliveryCounter.Instance.transform.position;
        DeliveryManager.Instance.OnRecipeSuccess += RecipeSuccessAction;
        DeliveryManager.Instance.OnRecipeFailed += RecipeFailedAction;
        CuttingCounter.OnAnyCut += AnyCutAction;
        Player.Instance.OnPickedSomething += PickedSomethingAction;
        CountersBase.OnAnyObjectPlacedHere += AnyObjectPlacedHereAction;
        TrashCounter.OnAnyObjectTrashed += AnyObjectTrashedAction;
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefs.footstep, position, volume);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(SOUND_VOLUME_SAVE, volume);
    }

    public float GetVolume() => volume;

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void AnyObjectTrashedAction(TrashCounter sender)
    {
        PlaySound(audioClipRefs.trash, sender.transform.position);
    }

    private void AnyObjectPlacedHereAction(CountersBase sender)
    {
        PlaySound(audioClipRefs.objectDrop, sender.transform.position);
    }

    private void PickedSomethingAction()
    {
        PlaySound(audioClipRefs.objectPickup, Player.Instance.transform.position);
    }

    private void AnyCutAction(CuttingCounter sender)
    {
        PlaySound(audioClipRefs.chop, sender.transform.position);
    }

    private void RecipeSuccessAction()
    {
        PlaySound(audioClipRefs.deliverySuccess, deliveryCounterPos);
    }

    private void RecipeFailedAction()
    {
        PlaySound(audioClipRefs.deliveryFailed, deliveryCounterPos);

    }
}
