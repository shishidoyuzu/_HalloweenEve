using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // どんなタイプのオブジェクトか
    public enum ObjectType
    {
        None,//タイプがない状態
        Pumpkin, // 拾えるかぼちゃ
        Grass,   // 調べると出るかぼちゃ
        LeafPile,// 落ち葉
    }

    [Header("オブジェクト設定")]
    public ObjectType type; // 種類をInspectorで設定する

    [Header("Grass用設定")]
    public GameObject pumpkinPrefab; // 草から出るかぼちゃのプレハブ
    [Range(0f, 1f)] public float spawnChance = 0.6f; // 出現確率（60%）

    private bool hasInteracted = false; // 一度きりにするためのフラグ
    // （草むらから出てきたかぼちゃを）拾えるかどうか
    private bool canBeCollected = true;


    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        if (type == ObjectType.None)
        {
            Debug.LogWarning($"{gameObject.name} の ObjectType が設定されていません！");
        }
        // 生成直後のかぼちゃを拾えないように
        if (type == ObjectType.Pumpkin)
        {
            canBeCollected = false;
            Invoke(nameof(EnableCollect), 1f);
        }
    }

    // プレイヤーが触れた時（かぼちゃ用）
    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに触れる＆ゲットできる＆
        // タイプがかぼちゃ＆一度も反応していない場合
        if (other.CompareTag("Player") && type == ObjectType.Pumpkin && !hasInteracted && canBeCollected)
        {
            Debug.Log("かぼちゃを拾った！");
            hasInteracted = true; // 反応した
            
            // 安全にGameManagerを探してスコア加算
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.CollectPumpkin();
                gm?.PlaySE(gm.pumpkinSE);
            }
            Destroy(gameObject); // 自分（かぼちゃ）を消す
        }
    }

    void EnableCollect()
    {
        canBeCollected = true;
    }

    // プレイヤーがアクション（スペースキー）したとき
    public void OnAction()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        // 拾えない間は何も起きない
        if (!canBeCollected) return;
        // すでに反応していたら無視
        if (hasInteracted) return;

        Debug.Log($"(OnAction 呼ばれた！ type = {type}");

        switch (type)
        {
            // タイプが草むらの時
            case ObjectType.Grass:
                hasInteracted = true;// 反応した
                gm?.PlaySE(gm.grassSE);
                // まだ生成上限以内 & 確率判定に通ったら生成
                if (Random.value < spawnChance)
                {
                    GameObject pumpkin = Instantiate(
                        pumpkinPrefab, 
                        transform.position + Vector3.down * 0.2f, 
                        Quaternion.identity);
                }

                Destroy(gameObject); // 草を消す（調べ終わり）
                break;

            case ObjectType.LeafPile:
                hasInteracted = true;
                // 落ち葉のアクション（後で追加予定）
                Destroy(gameObject);
                break;
        }
    }
}
