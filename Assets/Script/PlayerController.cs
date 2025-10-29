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
    
    // 前方向にどのくらいの長さのRayを飛ばすか（オブジェクトに対するアクション判定の距離）
    public float actionDistance = 1.0f;
    // Rayがどのレイヤーに当たるかを判定（Interactableレイヤー）
    public LayerMask interactableLayer;

    private Animator animator;


    void Start()
    {
        // Rigidbody2Dの取得
        rb = GetComponent<Rigidbody2D>();
        // SpriteRendererの取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Animatorの取得
        animator = GetComponent<Animator>();
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
            spriteRenderer.flipX = false; // 右
        else if (moveInput.x > 0)
            spriteRenderer.flipX = true;  // 左

        // スペースキーを押したら、アクションする
        if (Input.GetKeyDown(KeyCode.Space))
            TryAction();

        bool isMoving = moveInput.sqrMagnitude > 0;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // 実際の移動処理（物理）
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void TryAction()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
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

