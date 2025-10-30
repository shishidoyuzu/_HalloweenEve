using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public AudioSource bgmSource;
    public AudioClip titleBGM;
    public AudioClip gameBGM;
    public AudioClip resultBGM;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
