using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // �ǂ�ȃ^�C�v�̃I�u�W�F�N�g��
    public enum ObjectType
    {
        None,//�^�C�v���Ȃ����
        Pumpkin, // �E���邩�ڂ���
        Grass,   // ���ׂ�Əo�邩�ڂ���
        LeafPile,// �����t
    }

    [Header("�I�u�W�F�N�g�ݒ�")]
    public ObjectType type; // ��ނ�Inspector�Őݒ肷��

    [Header("Grass�p�ݒ�")]
    public GameObject pumpkinPrefab; // ������o�邩�ڂ���̃v���n�u
    [Range(0f, 1f)] public float spawnChance = 0.6f; // �o���m���i60%�j

    private bool hasInteracted = false; // ��x����ɂ��邽�߂̃t���O
    // �i���ނ炩��o�Ă������ڂ�����j�E���邩�ǂ���
    private bool canBeCollected = true;


    private void Start()
    {
        if (type == ObjectType.None)
        {
            Debug.LogWarning($"{gameObject.name} �� ObjectType ���ݒ肳��Ă��܂���I");
        }
        // ��������̂��ڂ�����E���Ȃ��悤��
        if (type == ObjectType.Pumpkin)
        {
            canBeCollected = false;
            Invoke(nameof(EnableCollect), 1f);
        }
    }

    // �v���C���[���G�ꂽ���i���ڂ���p�j
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�ɐG��違�Q�b�g�ł��違
        // �^�C�v�����ڂ��ၕ��x���������Ă��Ȃ��ꍇ
        if (other.CompareTag("Player") && type == ObjectType.Pumpkin && !hasInteracted && canBeCollected)
        {
            Debug.Log("���ڂ�����E�����I");
            hasInteracted = true; // ��������
            GameManager.instance.CollectPumpkin(); // �X�R�A���Z
            Destroy(gameObject); // �����i���ڂ���j������
        }
    }

    void EnableCollect()
    {
        canBeCollected = true;
    }

    // �v���C���[���A�N�V�����i�X�y�[�X�L�[�j�����Ƃ�
    public void OnAction()
    {
        Debug.Log("OnAction�֐��ɗ����I");

        // �E���Ȃ��Ԃ͉����N���Ȃ�
        if (!canBeCollected) return;
        // ���łɔ������Ă����疳��
        if (hasInteracted) return;

        Debug.Log($"(OnAction �Ă΂ꂽ�I type = {type}");

        switch (type)
        {
            // �^�C�v�����ڂ���̎�
            case ObjectType.Pumpkin:
                Debug.Log("���ڂ�����Q�b�g�I");
                hasInteracted = true;
                Destroy(gameObject);
                break;

            // �^�C�v�����ނ�̎�
            case ObjectType.Grass:
                hasInteracted = true;// ��������

                // �܂���������ȓ� & �m������ɒʂ����琶��
                if (GameManager.instance.CanSpawnPumpkin() && Random.value < spawnChance)
                {
                    GameObject pumpkin = Instantiate(pumpkinPrefab, transform.position, Quaternion.identity);
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
