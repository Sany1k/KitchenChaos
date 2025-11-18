using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const string MUSIC_VOLUME_SAVE = "MusicVolume";

    private AudioSource audioSource;
    private float volume = 0.3f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_SAVE, 0.3f);
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME_SAVE, volume);
    }

    public float GetVolume() => volume;
}
