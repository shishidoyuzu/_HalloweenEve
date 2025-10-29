using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // �ǂ�ȃ^�C�v�̃I�u�W�F�N�g��
    public enum ObjectType
    {
        None,//�^�C�v���Ȃ����
        Pumpkin, // �E���邩�ڂ���
        Grass,   // ���ׂ�Əo�邩�ڂ���
        Tree,    // ��
        LeafPile,// �����t
    }

    [Header("�I�u�W�F�N�g�ݒ�")]
    public ObjectType type; // ��ނ�Inspector�Őݒ肷��

    [Header("Grass�p�ݒ�")]
    public GameObject pumpkinPrefab; // ������o�邩�ڂ���̃v���n�u
    [Range(0f, 1f)] public float spawnChance = 0.4f; // �o���m���i40%�j

    private bool hasInteracted = false; // ��x����ɂ��邽�߂̃t���O

    private void Start()
    {
        if (type == ObjectType.None)
        {
            Debug.LogWarning($"{gameObject.name} �� ObjectType ���ݒ肳��Ă��܂���I");
        }
    }

    // �v���C���[���G�ꂽ���i���ڂ���p�j
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�ɐG��違�^�C�v�����ڂ��ၕ��x���������Ă��Ȃ��ꍇ
        if (other.CompareTag("Player") && type == ObjectType.Pumpkin && !hasInteracted)
        {
            Debug.Log("���ڂ�����E�����I");
            hasInteracted = true; // ��������
            GameManager.instance.CollectPumpkin(); // �X�R�A���Z
            Destroy(gameObject); // �����i���ڂ���j������
        }
    }

    // �v���C���[���A�N�V�����i�X�y�[�X�L�[�j�����Ƃ�
    public void OnAction()
    {
        Debug.Log("OnAction�֐��ɗ����I");
        // ���łɔ������Ă����疳��
        if (hasInteracted) return;

        Debug.Log($"(OnAction �Ă΂ꂽ�I type = {type}");

        switch (type)
        {
            // �^�C�v�����ނ�̎�
            case ObjectType.Grass:
                hasInteracted = true;// ��������

                // �܂���������ȓ� & �m������ɒʂ����琶��
                if (GameManager.instance.CanSpawnPumpkin() && Random.value < spawnChance)
                {
                    Instantiate(pumpkinPrefab, transform.position, Quaternion.identity);
                    GameManager.instance.RegisterPumpkin(); // �����J�E���g�{1
                }

                Destroy(gameObject); // ���������i���׏I���j
                break;

            case ObjectType.LeafPile:
                hasInteracted = true;
                // �����t�̃A�N�V�����i��Œǉ��\��j
                Destroy(gameObject);
                break;
        }
    }
}
