using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // シングルトン

    [Header("UI参照")]
    public TextMeshProUGUI timeText;       // 時間表示テキスト
    public TextMeshProUGUI pumpkinText;    // かぼちゃテキスト

    [Header("ゲーム設定")]
    public float timeLimit = 60f;// 制限時間
    public int maxPumpkins = 10; // マップに存在するかぼちゃ

    [Header("生成するプレハブ")]
    public GameObject pumpkinPrefab; // かぼちゃプレハブ
    public GameObject grassPrefab;   // 草むらプレハブ

    [Header("サウンド設定")]
    public AudioClip pumpkinSE;  // かぼちゃを取った音
    public AudioClip grassSE;    // 草を調べた音
    public AudioSource seSource;

    private float timeLeft;
    private int collectedPumpkins = 0; // 拾った数
    private bool isGameOver = false;

    void Awake()
    {
        // シーン開始時にUIなどを再取得
        if (pumpkinText == null)
            pumpkinText = GameObject.Find("PumpkinText")?.GetComponent<TextMeshProUGUI>();
        if (timeText == null)
            timeText = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();

        //seSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // BGMの再生
        if (BGMManager.instance != null)
        {
            BGMManager.instance.PlayBGM(BGMManager.instance.gameBGM);
        }

        // 制限時間の初期化
        timeLeft = timeLimit;

        // UI参照が切れていたら再取得
        if (pumpkinText == null)
        {
            var pumpkinObj = GameObject.Find("PumpkinText");
            if (pumpkinObj != null)
                pumpkinText = pumpkinObj.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("PumpkinText が見つかりません！");
        }

        if (timeText == null)
        {
            var timeObj = GameObject.Find("TimeText");
            if (timeObj != null)
                timeText = timeObj.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("TimeText が見つかりません！");
        }

        // UI初期化
        UpdatePumpkinUI();
    }

    void Update()
    {
        // ゲームオーバーになったらスルーする
        if (isGameOver) return;

        // カウントダウン
        timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
        // 制限時間の表示
        timeText.text = Mathf.CeilToInt(timeLeft).ToString();

        // ０秒になる＝時間切れになったら
        if (timeLeft <= 0f)
            EndGame();
    }

    // かぼちゃ取得
    public void CollectPumpkin()
    {
        // かぼちゃの数を＋１する
        collectedPumpkins++;
        UpdatePumpkinUI();

        // マップ内にかぼちゃか草むらを生成する
        RandomSpawnObj();
    }

    void UpdatePumpkinUI()
    {
        // ＋１したかぼちゃの数を更新する
        pumpkinText.text = $"かぼちゃ : {collectedPumpkins}";
    }

    public void RandomSpawnObj()
    {
        // MapGeneratorからマップ範囲を取得
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        if(mapGenerator == null) return; // MapGeneratorがない場合、抜ける

        float map_width  = mapGenerator.mapSize.x;
        float map_height = mapGenerator.mapSize.y;

        // マップ範囲の中で生成位置をランダムで取得
        Vector2 Random_SpawnPos = new Vector2(
            Random.Range(-map_width  / 2f, map_height / 2f),
            Random.Range(-map_height / 2f, map_width / 2f));

        // 7:3の確率で「かぼちゃ」か「草むら」が生成される
        if (Random.value < 0.7f)
            Instantiate(pumpkinPrefab, Random_SpawnPos, Quaternion.identity);
        else
            Instantiate(grassPrefab, Random_SpawnPos, Quaternion.identity);
    }

    // ゲーム終了
    void EndGame()
    {
        isGameOver = true;

        // プレイヤーの操作を停止
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.enabled = false;

        // スコアを保存しておく（Resultで表示）
        PlayerPrefs.SetInt("PumpkinScore", collectedPumpkins);

        ChangeScene changer = FindObjectOfType<ChangeScene>();
        if (changer != null)
        {
            changer.GoToResult("Result");
        }
        else
        {
            string SceneName = "Result";
            // フェードが無い場合は直接遷移
            Initiate.Fade(SceneName, Color.black, 1.0f);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        Debug.Log("PlaySE来た");
        if (clip == null) return;
        seSource.PlayOneShot(clip);
    }
}
