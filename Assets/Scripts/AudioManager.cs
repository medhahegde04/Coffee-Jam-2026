using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip clickSound;

    void Awake()
    {
        Instance = this;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayCorrect() => sfxSource.PlayOneShot(correctSound);
    public void PlayWrong() => sfxSource.PlayOneShot(wrongSound);
    public void PlayClick() => sfxSource.PlayOneShot(clickSound);
}