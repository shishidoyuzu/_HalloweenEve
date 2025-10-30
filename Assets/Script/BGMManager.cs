using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [Header("BGMƒŠƒXƒg")]
    public AudioClip titleBGM;
    public AudioClip gameBGM;

    private AudioSource audioSource;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (scene.name == "GamePlay") { PlayBGM(gameBGM); return; }
        if (scene.name == "Title") { PlayBGM(titleBGM); return; }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return; // “¯‚¶BGM‚È‚ç‰½‚à‚µ‚È‚¢

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
