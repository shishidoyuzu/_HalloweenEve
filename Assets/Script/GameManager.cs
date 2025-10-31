using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �V���O���g��

    [Header("UI�Q��")]
    public TextMeshProUGUI timeText;       // ���ԕ\���e�L�X�g
    public TextMeshProUGUI pumpkinText;    // ���ڂ���e�L�X�g

    [Header("�Q�[���ݒ�")]
    public float timeLimit = 60f;// ��������
    public int maxPumpkins = 10; // �}�b�v�ɑ��݂��邩�ڂ���

    [Header("��������v���n�u")]
    public GameObject pumpkinPrefab; // ���ڂ���v���n�u
    public GameObject grassPrefab;   // ���ނ�v���n�u

    [Header("�T�E���h�ݒ�")]
    public AudioClip pumpkinSE;  // ���ڂ�����������
    public AudioClip grassSE;    // ���𒲂ׂ���
    public AudioSource seSource;

    private float timeLeft;
    private int collectedPumpkins = 0; // �E������
    private bool isGameOver = false;

    void Awake()
    {
        // �V�[���J�n����UI�Ȃǂ��Ď擾
        if (pumpkinText == null)
            pumpkinText = GameObject.Find("PumpkinText")?.GetComponent<TextMeshProUGUI>();
        if (timeText == null)
            timeText = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();

        //seSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // BGM�̍Đ�
        if (BGMManager.instance != null)
        {
            BGMManager.instance.PlayBGM(BGMManager.instance.gameBGM);
        }

        // �������Ԃ̏�����
        timeLeft = timeLimit;

        // UI�Q�Ƃ��؂�Ă�����Ď擾
        if (pumpkinText == null)
        {
            var pumpkinObj = GameObject.Find("PumpkinText");
            if (pumpkinObj != null)
                pumpkinText = pumpkinObj.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("PumpkinText ��������܂���I");
        }

        if (timeText == null)
        {
            var timeObj = GameObject.Find("TimeText");
            if (timeObj != null)
                timeText = timeObj.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("TimeText ��������܂���I");
        }

        // UI������
        UpdatePumpkinUI();
    }

    void Update()
    {
        // �Q�[���I�[�o�[�ɂȂ�����X���[����
        if (isGameOver) return;

        // �J�E���g�_�E��
        timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
        // �������Ԃ̕\��
        timeText.text = Mathf.CeilToInt(timeLeft).ToString();

        // �O�b�ɂȂ遁���Ԑ؂�ɂȂ�����
        if (timeLeft <= 0f)
            EndGame();
    }

    // ���ڂ���擾
    public void CollectPumpkin()
    {
        // ���ڂ���̐����{�P����
        collectedPumpkins++;
        UpdatePumpkinUI();

        // �}�b�v���ɂ��ڂ��Ⴉ���ނ�𐶐�����
        RandomSpawnObj();
    }

    void UpdatePumpkinUI()
    {
        // �{�P�������ڂ���̐����X�V����
        pumpkinText.text = $"���ڂ��� : {collectedPumpkins}";
    }

    public void RandomSpawnObj()
    {
        // MapGenerator����}�b�v�͈͂��擾
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        if(mapGenerator == null) return; // MapGenerator���Ȃ��ꍇ�A������

        float map_width  = mapGenerator.mapSize.x;
        float map_height = mapGenerator.mapSize.y;

        // �}�b�v�͈͂̒��Ő����ʒu�������_���Ŏ擾
        Vector2 Random_SpawnPos = new Vector2(
            Random.Range(-map_width  / 2f, map_height / 2f),
            Random.Range(-map_height / 2f, map_width / 2f));

        // 7:3�̊m���Łu���ڂ���v���u���ނ�v�����������
        if (Random.value < 0.7f)
            Instantiate(pumpkinPrefab, Random_SpawnPos, Quaternion.identity);
        else
            Instantiate(grassPrefab, Random_SpawnPos, Quaternion.identity);
    }

    // �Q�[���I��
    void EndGame()
    {
        isGameOver = true;

        // �v���C���[�̑�����~
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.enabled = false;

        // �X�R�A��ۑ����Ă����iResult�ŕ\���j
        PlayerPrefs.SetInt("PumpkinScore", collectedPumpkins);

        ChangeScene changer = FindObjectOfType<ChangeScene>();
        if (changer != null)
        {
            changer.GoToResult("Result");
        }
        else
        {
            string SceneName = "Result";
            // �t�F�[�h�������ꍇ�͒��ڑJ��
            Initiate.Fade(SceneName, Color.black, 1.0f);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        Debug.Log("PlaySE����");
        if (clip == null) return;
        seSource.PlayOneShot(clip);
    }
}
