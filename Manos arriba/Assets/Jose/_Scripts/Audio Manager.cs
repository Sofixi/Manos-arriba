using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]

    public AudioSource musicSource;

    public AudioSource sfxSource;

    [Header("Music")]

    public AudioClip menuMusic;

    public AudioClip gameplayMusic;

    public AudioClip warningMusic;

    public AudioClip victoryMusic;

    [Header("SFX")]

    public AudioClip buttonClick;

    public AudioClip pickupSFX;

    public AudioClip jumpSFX;

    public AudioClip warningSFX;

    public AudioClip roundEndSFX;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reproducir música
    public void PlayMusic(AudioClip clip)
    {
        // Evita reiniciar misma música
        if (musicSource.clip == clip)
        {
            return;
        }

        musicSource.clip = clip;

        musicSource.Play();
    }

    // Reproducir efecto
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Música menú
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    // Música gameplay
    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    // Música warning
    public void PlayWarningMusic()
    {
        PlayMusic(warningMusic);
    }

    // Música victoria
    public void PlayVictoryMusic()
    {
        PlayMusic(victoryMusic);
    }
}