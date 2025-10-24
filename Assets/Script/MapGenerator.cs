using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject prefab;  // ��������v���n�u
    public int count;          // �������鐔
}

public class MapGenerator : MonoBehaviour
{
    [Header("�}�b�v�͈́i���S�����_�ɂ���j")]
    public Vector2 mapSize = new Vector2(20.0f, 10.0f);

    [Header("�����I�u�W�F�N�g���X�g")]
    public SpawnObject[] spawnObjects;

    [Header("�d�Ȃ�h�~")]
    public float minDistance = 0.5f; // �߂�����z�u��h��

    [Header("�v���C���[�̎��͂����O")]
    public float playerSafeRadius = 2f;// �v���C���[�̎���ɃI�u�W�F�N�g����������Ȃ�

    private Transform playerPos;// �v���C���[�̊J�n�ʒu

    // �����ꏊ�ɃI�u�W�F�N�g����������Ȃ�
    private readonly System.Collections.Generic.List<Vector2> usedPositions = new();   /* 
        List<Vector2>	�ݒu�ς݂̈ʒu���L�^���郊�X�g
        usedPositions	���ۂɂ��̃f�[�^�������Ă�ϐ�
        readonly	    �r���ŕʂ̃��X�g�ɍ����ւ��ł��Ȃ��悤�ɂ���
        ����	        �d���E���W������邽�߂́u���W�������v                         */

    void Start()
    {
        // �v���C���[�̈ʒu�����擾����
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        // �}�b�v����
        GenerateMap();
    }

    void GenerateMap()
    {
        // foreach �́A�u�z��⃊�X�g�̒��g���ЂƂ����ԂɎ��o���ď�������v���߂̍\���B
        foreach (var obj in spawnObjects)
        {
            // �v���n�u�̐������鐔�Ԃ�A�܂킷
            for (int i = 0; i < obj.count; i++)
            {
                Vector2 pos;
                int tries = 0;// ���񃉃��_�����W����������

                // �d�Ȃ�h�~�t�������_���ʒu�T��
                do
                {
                    pos = new Vector2(
                        Random.Range(-mapSize.x, mapSize.x),
                        Random.Range(-mapSize.y, mapSize.y)
                    );
                    tries++;
                    // ���� 50�����Ă��K�؂ȏꏊ��������Ȃ���������߂�
                    if (tries > 50) break;
                }
                while (!IsPositionValid(pos)); // �߂��ɑ��̃I�u�W�F�N�g���Ȃ������_���ʒu��T������

                // ���܂��������_���ʒu�ɐ���
                Instantiate(obj.prefab, pos, Quaternion.identity);
                // ���܂��������_���ʒu���g�p�ςݍ��W�ɓo�^
                usedPositions.Add(pos);
            }
        }
    }

    // �߂��ɑ��̃I�u�W�F�N�g���Ȃ������`�F�b�N
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
