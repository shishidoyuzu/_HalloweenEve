using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 4.0f;
    // 移動入力
    private Vector2 moveInput;

    // 移動入力に応じたプレイヤーの反転
    private SpriteRenderer spriteRenderer;
    // Rigidbody2D
    private Rigidbody2D rb;
    
    // 前方向にどのくらいの長さのRayを飛ばすか
    // （オブジェクトに対するアクション判定の距離）
    public float actionDistance = 1.0f;
    // Rayがどのレイヤーに当たるかを判定（Interactableレイヤー）
    public LayerMask interactableLayer;

    // Start is called before the first frame update

    void Start()
    {
        // Rigidbody2Dの取得
        rb = GetComponent<Rigidbody2D>();
        // SpriteRendererの取得
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 移動入力の取得
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 斜め移動の際、移動速度を上下左右の移動速度になる
        moveInput.Normalize();

        // 向き反転処理
        if (moveInput.x < 0)
            spriteRenderer.flipX = false; // 右向き
        else if (moveInput.x > 0)
            spriteRenderer.flipX = true;  // 左向き

        // スペースキーを押したら、アクションする
        if (Input.GetKeyDown(KeyCode.Space))
            TryAction();
    }

    void FixedUpdate()
    {
        // 実際の移動処理（物理）
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void TryAction()
    {
        Vector2 direction;
        if (spriteRenderer.flipX)
            direction = Vector2.right; // 右向きなら右
        else
            direction = Vector2.left;  // 左向きなら左

        Vector2 origin = (Vector2)transform.position + direction * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, actionDistance, interactableLayer);
        Debug.Log("Rayを飛ばした！");

        // 飛ばしたRayを表示
        Debug.DrawRay(transform.position, direction * actionDistance, Color.magenta, 0.2f);

        if (hit.collider != null)
        {
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                obj.OnAction();
            }
        }
    }
}

