using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject prefab;  // 生成するプレハブ
    public int count;          // 生成する数
}

public class MapGenerator : MonoBehaviour
{
    [Header("マップ範囲（中心を原点にする）")]
    public Vector2 mapSize = new Vector2(20.0f, 10.0f);

    [Header("生成オブジェクトリスト")]
    public SpawnObject[] spawnObjects;

    [Header("重なり防止")]
    public float minDistance = 0.5f; // 近すぎる配置を防ぐ

    [Header("プレイヤーの周囲を除外")]
    public float playerSafeRadius = 2f;// プレイヤーの周りにオブジェクトが生成されない

    private Transform playerPos;// プレイヤーの開始位置

    // 同じ場所にオブジェクトが生成されない
    private readonly System.Collections.Generic.List<Vector2> usedPositions = new();   /* 
        List<Vector2>	設置済みの位置を記録するリスト
        usedPositions	実際にそのデータを持ってる変数
        readonly	    途中で別のリストに差し替えできないようにする
        役割	        重複・密集を避けるための「座標メモ帳」                         */

    void Start()
    {
        // プレイヤーの位置情報を取得する
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        // マップ生成
        GenerateMap();
    }

    void GenerateMap()
    {
        // foreach は、「配列やリストの中身をひとつずつ順番に取り出して処理する」ための構文。
        foreach (var obj in spawnObjects)
        {
            // プレハブの生成する数ぶん、まわす
            for (int i = 0; i < obj.count; i++)
            {
                Vector2 pos;
                int tries = 0;// 何回ランダム座標を試したか

                // 重なり防止付きランダム位置探し
                do
                {
                    pos = new Vector2(
                        Random.Range(-mapSize.x, mapSize.x),
                        Random.Range(-mapSize.y, mapSize.y)
                    );
                    tries++;
                    // もし 50回やっても適切な場所が見つからなかったら諦める
                    if (tries > 50) break;
                }
                while (!IsPositionValid(pos)); // 近くに他のオブジェクトがないランダム位置を探す処理

                // 決まったランダム位置に生成
                Instantiate(obj.prefab, pos, Quaternion.identity);
                // 決まったランダム位置を使用済み座標に登録
                usedPositions.Add(pos);
            }
        }
    }

    // 近くに他のオブジェクトがないかをチェック
    bool IsPositionValid(Vector2 pos)
    {
        foreach (var used in usedPositions)
        {
            if (Vector2.Distance(pos, used) < minDistance)
                return false;
        }
        return true;
    }
}
