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
    [Range(0f, 1f)] public float spawnChance = 1.0f; // �o���m���i40%�j

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

                    // ���ނ炩��o�Ă������ڂ���ɉ��o������
                    StartCoroutine(SpawnEffect(pumpkin));
                    EnableCollect();
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

    // ���ڂ���o���A�j���[�V����
    IEnumerator SpawnEffect(GameObject pumpkin)
    {
        SpriteRenderer sr = pumpkin.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        // �������ǉ�
        // ���̃}�e���A�����󂳂Ȃ�
        sr.sharedMaterial = Shader.Find("Sprites/Default") != null
            ? sr.sharedMaterial
            : new Material(Shader.Find("Sprites/Default"));

        sr.material.renderQueue = 3000; // �����`��̏��Ԃ𖾎�
        sr.sortingLayerName = "Character"; // ���C���[�𖾎��i���ɉB��Ȃ��悤�Ɂj

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        Vector3 startPos = pumpkin.transform.position - new Vector3(0, 0.5f, 0);
        Vector3 endPos = pumpkin.transform.position;
        pumpkin.transform.position = startPos;

        float duration = 0.3f; // �t�F�[�h�C�����x
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // �㏸�{�t�F�[�h�C��
            pumpkin.transform.position = Vector3.Lerp(startPos, endPos, t);
            c.a = Mathf.Lerp(0f, 1f, t);
            sr.color = c;
            Debug.Log($"Alpha: {sr.color.a}");


            yield return null;
        }

        // �ŏI��Ԃ������I�Ɋ��S�s������
        Color finalColor = sr.color;
        finalColor.a = 1f;
        sr.color = finalColor;
    }
}
