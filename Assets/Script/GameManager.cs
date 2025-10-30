using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �V���O���g��

    [Header("UI�Q��")]
    public TextMeshProUGUI timeText;       // ���ԕ\���e�L�X�g
    public TextMeshProUGUI pumpkinText;    // ���ڂ���e�L�X�g
    public GameObject resultUI;            // ���U���gUI
    public TextMeshProUGUI resultText;     // ���U���g�e�L�X�g

    [Header("�Q�[���ݒ�")]
    public float timeLimit = 60f;// ��������
    public int maxPumpkins = 10; // �}�b�v�ɑ��݂��邩�ڂ���

    [Header("��������v���n�u")]
    public GameObject pumpkinPrefab; // ���ڂ���v���n�u
    public GameObject grassPrefab;   // ���ނ�v���n�u


    private float timeLeft;
    private int collectedPumpkins = 0; // �E������
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // �V�[�����܂����ł������Ȃ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    void Start()
    {
        // �������Ԃ̏�����
        timeLeft = timeLimit;
        // UI��������
        UpdatePumpkinUI();

        // ���U���g�p��UI�������
        if (resultUI != null)
            resultUI.SetActive(false); // ���U���g��ʂ̔�\��
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

        // ���U���g�p��UI�������
        if (resultUI != null)
        {
            // �\�����āA�W�߂����ڂ���̐����o��
            resultUI.SetActive(true);
            resultText.text = $"���߂����ڂ���: {collectedPumpkins}";
        }
    }

    // �V�[���؂�ւ��i�{�^���p�j
    public void Retry()
    {
        // ���A�N�e�B�u�ɂȂ��Ă���V�[����������x�ǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
