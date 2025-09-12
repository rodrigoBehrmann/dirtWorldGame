using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip _buttonClickSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, AudioSource source = null)
    {
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
        else
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void PlayButtonClickSound()
    {
        PlaySound(_buttonClickSound);
    }
}
