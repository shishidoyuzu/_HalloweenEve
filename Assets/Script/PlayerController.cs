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
    
    // �O�����ɂǂ̂��炢�̒�����Ray���΂����i�I�u�W�F�N�g�ɑ΂���A�N�V��������̋����j
    public float actionDistance = 1.0f;
    // Ray���ǂ̃��C���[�ɓ����邩�𔻒�iInteractable���C���[�j
    public LayerMask interactableLayer;

    private Animator animator;


    void Start()
    {
        // Rigidbody2D�̎擾
        rb = GetComponent<Rigidbody2D>();
        // SpriteRenderer�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Animator�̎擾
        animator = GetComponent<Animator>();
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
            spriteRenderer.flipX = false; // �E
        else if (moveInput.x > 0)
            spriteRenderer.flipX = true;  // ��

        // �X�y�[�X�L�[����������A�A�N�V��������
        if (Input.GetKeyDown(KeyCode.Space))
            TryAction();

        bool isMoving = moveInput.sqrMagnitude > 0;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        // ���ۂ̈ړ������i�����j
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void TryAction()
    {
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = (Vector2)transform.position + direction * 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, actionDistance, interactableLayer);
        Debug.Log("Ray���΂����I");

        // ��΂���Ray��\��
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

