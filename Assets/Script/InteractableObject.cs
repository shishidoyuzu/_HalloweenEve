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
    [Range(0f, 1f)] public float spawnChance = 1.0f; // 出現確率（40%）

    private bool hasInteracted = false; // 一度きりにするためのフラグ
    // （草むらから出てきたかぼちゃを）拾えるかどうか
    private bool canBeCollected = true;


    private void Start()
    {
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
            GameManager.instance.CollectPumpkin(); // スコア加算
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
        Debug.Log("OnAction関数に来た！");

        // 拾えない間は何も起きない
        if (!canBeCollected) return;
        // すでに反応していたら無視
        if (hasInteracted) return;

        Debug.Log($"(OnAction 呼ばれた！ type = {type}");

        switch (type)
        {
            // タイプがかぼちゃの時
            case ObjectType.Pumpkin:
                Debug.Log("かぼちゃをゲット！");
                hasInteracted = true;
                Destroy(gameObject);
                break;

            // タイプが草むらの時
            case ObjectType.Grass:
                hasInteracted = true;// 反応した

                // まだ生成上限以内 & 確率判定に通ったら生成
                if (GameManager.instance.CanSpawnPumpkin() && Random.value < spawnChance)
                {
                    GameObject pumpkin = Instantiate(pumpkinPrefab, transform.position, Quaternion.identity);
                    GameManager.instance.RegisterPumpkin(); // 生成カウント＋1

                    // 草むらから出てきたかぼちゃに演出をつける
                    StartCoroutine(SpawnEffect(pumpkin));
                    EnableCollect();
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

    // かぼちゃ出現アニメーション
    IEnumerator SpawnEffect(GameObject pumpkin)
    {
        SpriteRenderer sr = pumpkin.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        // ←ここ追加
        // 元のマテリアルを壊さない
        sr.sharedMaterial = Shader.Find("Sprites/Default") != null
            ? sr.sharedMaterial
            : new Material(Shader.Find("Sprites/Default"));

        sr.material.renderQueue = 3000; // 透明描画の順番を明示
        sr.sortingLayerName = "Character"; // レイヤーを明示（草に隠れないように）

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        Vector3 startPos = pumpkin.transform.position - new Vector3(0, 0.5f, 0);
        Vector3 endPos = pumpkin.transform.position;
        pumpkin.transform.position = startPos;

        float duration = 0.3f; // フェードイン速度
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // 上昇＋フェードイン
            pumpkin.transform.position = Vector3.Lerp(startPos, endPos, t);
            c.a = Mathf.Lerp(0f, 1f, t);
            sr.color = c;
            Debug.Log($"Alpha: {sr.color.a}");


            yield return null;
        }

        // 最終状態を強制的に完全不透明に
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;
    }
}
