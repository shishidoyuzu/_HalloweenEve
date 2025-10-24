using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �V���O���g��

    [Header("UI�Q��")]
    public Text timeText;       // ���ԕ\���e�L�X�g
    public Text pumpkinText;    // ���ڂ���e�L�X�g
    public GameObject resultUI; // ���U���gUI
    public Text resultText;     // ���U���g�e�L�X�g

    [Header("�Q�[���ݒ�")]
    public float timeLimit = 60f;// ��������
    public int maxPumpkins = 10;// �}�b�v�ɑ��݂��邩�ڂ���

    private float timeLeft;
    private int currentPumpkins = 0;   // �������ꂽ��
    private int collectedPumpkins = 0; // �E������
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // �������Ԃ̏�����
        timeLeft = timeLimit;
        // UI��������
        UpdatePumpkinUI();

        if (resultUI != null)
            resultUI.SetActive(false); // ���U���g��ʂ̔�\��
    }

    void Update()
    {
        // �Q�[���I�[�o�[�ɂȂ�����X���[����
        if (isGameOver) return;

        // �J�E���g�_�E��
        timeLeft -= Time.deltaTime;
        // �������Ԃ̕\��
        timeText.text = "Time: " + Mathf.CeilToInt(timeLeft);

        // �O�b�ɂȂ遁���Ԑ؂�ɂȂ�����
        if (timeLeft <= 0f)
            EndGame();
    }

    // ���ڂ��ᐶ���Ǘ�
    public bool CanSpawnPumpkin() => currentPumpkins < maxPumpkins;
    public void RegisterPumpkin() => currentPumpkins++;

    /*
    CanSpawnPumpkin()�E�E�E���ڂ��ᐶ�����Ă�������GameManager�Ɋm�F
    RegisterPumpkin()�E�E�E����������I���ăJ�E���g�𑝂₷
     */


    // ���ڂ���擾
    public void CollectPumpkin()
    {
        collectedPumpkins++;
        UpdatePumpkinUI();
    }

    void UpdatePumpkinUI()
    {
        pumpkinText.text = $"Pumpkins: {collectedPumpkins} / {maxPumpkins}";
    }

    // �Q�[���I��
    void EndGame()
    {
        isGameOver = true;

        if (resultUI != null)
        {
            resultUI.SetActive(true);
            resultText.text = $"���߂����ڂ���: {collectedPumpkins} / {maxPumpkins}";
        }
    }

    // �V�[���؂�ւ��i�{�^���p�j
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
