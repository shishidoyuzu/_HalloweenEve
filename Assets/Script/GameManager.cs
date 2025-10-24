using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // シングルトン

    [Header("UI参照")]
    public Text timeText;       // 時間表示テキスト
    public Text pumpkinText;    // かぼちゃテキスト
    public GameObject resultUI; // リザルトUI
    public Text resultText;     // リザルトテキスト

    [Header("ゲーム設定")]
    public float timeLimit = 60f;// 制限時間
    public int maxPumpkins = 10;// マップに存在するかぼちゃ

    private float timeLeft;
    private int currentPumpkins = 0;   // 生成された数
    private int collectedPumpkins = 0; // 拾った数
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
        // 制限時間の初期化
        timeLeft = timeLimit;
        // UIを初期化
        UpdatePumpkinUI();

        if (resultUI != null)
            resultUI.SetActive(false); // リザルト画面の非表示
    }

    void Update()
    {
        // ゲームオーバーになったらスルーする
        if (isGameOver) return;

        // カウントダウン
        timeLeft -= Time.deltaTime;
        // 制限時間の表示
        timeText.text = "Time: " + Mathf.CeilToInt(timeLeft);

        // ０秒になる＝時間切れになったら
        if (timeLeft <= 0f)
            EndGame();
    }

    // かぼちゃ生成管理
    public bool CanSpawnPumpkin() => currentPumpkins < maxPumpkins;
    public void RegisterPumpkin() => currentPumpkins++;

    /*
    CanSpawnPumpkin()・・・かぼちゃ生成してもいいかGameManagerに確認
    RegisterPumpkin()・・・生成したよ！ってカウントを増やす
     */


    // かぼちゃ取得
    public void CollectPumpkin()
    {
        collectedPumpkins++;
        UpdatePumpkinUI();
    }

    void UpdatePumpkinUI()
    {
        pumpkinText.text = $"Pumpkins: {collectedPumpkins} / {maxPumpkins}";
    }

    // ゲーム終了
    void EndGame()
    {
        isGameOver = true;

        if (resultUI != null)
        {
            resultUI.SetActive(true);
            resultText.text = $"あつめたかぼちゃ: {collectedPumpkins} / {maxPumpkins}";
        }
    }

    // シーン切り替え（ボタン用）
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
