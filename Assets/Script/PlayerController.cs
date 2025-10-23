using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    // �ړ����x
    public float moveSpeed = 4.0f;
    // �ړ�����
    private Vector2 moveInput;

    // �ړ����͂ɉ������v���C���[�̔��]
    private SpriteRenderer spriteRenderer;
    // Rigidbody2D
    private Rigidbody2D rb;
    
    // �O�����ɂǂ̂��炢�̒�����Ray���΂���
    // �i�I�u�W�F�N�g�ɑ΂���A�N�V��������̋����j
    public float actionDistance = 1f;
    // Ray���ǂ̃��C���[�ɓ����邩�𔻒�iInteractable���C���[�j
    public LayerMask interactableLayer;

    // Start is called before the first frame update

    void Start()
    {
        // Rigidbody2D�̎擾
        rb = GetComponent<Rigidbody2D>();
        // SpriteRenderer�̎擾
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // �ړ����͂̎擾
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // �΂߈ړ��̍ہA�ړ����x���㉺���E�̈ړ����x�ɂȂ�
        moveInput.Normalize();

        // �������]����
        if (moveInput.x < 0)
            spriteRenderer.flipX = false; // �E����
        else if (moveInput.x > 0)
            spriteRenderer.flipX = true;  // ������

        if (Input.GetKeyDown(KeyCode.Space))
            TryAction();
    }

    void FixedUpdate()
    {
        // ���ۂ̈ړ������i�����j
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void TryAction()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, actionDistance, interactableLayer);

        if (hit.collider != null)
        {
            /*
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                obj.OnAction();
            }
            */
        }

        Debug.DrawRay(transform.position, direction * actionDistance, Color.yellow, 0.2f);
    }
}

