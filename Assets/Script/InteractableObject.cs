using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // どんなタイプのオブジェクトか
    public enum ObjectType
    {
        None,//タイプがない状態
        Pumpkin, // 拾えるかぼちゃ
        Grass,   // 調べると出るかぼちゃ
        Tree,    // 木
        LeafPile,// 落ち葉
    }

    [Header("オブジェクト設定")]
    public ObjectType type; // 種類をInspectorで設定する

    [Header("Grass用設定")]
    public GameObject pumpkinPrefab; // 草から出るかぼちゃのプレハブ
    [Range(0f, 1f)] public float spawnChance = 0.4f; // 出現確率（40%）

    private bool hasInteracted = false; // 一度きりにするためのフラグ

    private void Start()
    {
        if (type == ObjectType.None)
        {
            Debug.LogWarning($"{gameObject.name} の ObjectType が設定されていません！");
        }
    }

    // プレイヤーが触れた時（かぼちゃ用）
    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに触れる＆タイプがかぼちゃ＆一度も反応していない場合
        if (other.CompareTag("Player") && type == ObjectType.Pumpkin && !hasInteracted)
        {
            Debug.Log("かぼちゃを拾った！");
            hasInteracted = true; // 反応した
            GameManager.instance.CollectPumpkin(); // スコア加算
            Destroy(gameObject); // 自分（かぼちゃ）を消す
        }
    }

    // プレイヤーがアクション（スペースキー）したとき
    public void OnAction()
    {
        Debug.Log("OnAction関数に来た！");
        // すでに反応していたら無視
        if (hasInteracted) return;

        Debug.Log($"(OnAction 呼ばれた！ type = {type}");

        switch (type)
        {
            // タイプが草むらの時
            case ObjectType.Grass:
                hasInteracted = true;// 反応した

                // まだ生成上限以内 & 確率判定に通ったら生成
                if (GameManager.instance.CanSpawnPumpkin() && Random.value < spawnChance)
                {
                    Instantiate(pumpkinPrefab, transform.position, Quaternion.identity);
                    GameManager.instance.RegisterPumpkin(); // 生成カウント＋1
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
