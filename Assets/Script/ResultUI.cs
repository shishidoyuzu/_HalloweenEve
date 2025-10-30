using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("UI�Q��")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;


    void Start()
    {
        // GameManager ����X�R�A���󂯎��i�ۑ��ς݂� PlayerPrefs ����擾�j
        int score = PlayerPrefs.GetInt("PumpkinScore", 0);
        scoreText.text = $"�W�߂����ڂ���F{score}��";

        // �X�R�A�ɉ��������b�Z�[�W��؂�ւ���
        if (score >= 60)
            messageText.text = "�ƂĂ���ʂ̂��ڂ�����W�߂ꂽ�I\n���ڂ�����W�߂�̂ɖ����ɂȂ���������c";
        else if (score >= 50)
            messageText.text = "��������̂��ڂ�����W�߂ꂽ�I\n������������Ȃ��c�I";
        else if (score >= 40)
            messageText.text = "�����ς����ڂ�����W�߂ꂽ�I\n�W�߂��������ȁc�H";
        else if (score >= 30)
            messageText.text = "�\���ȗʂ̂��ڂ�����W�߂ꂽ�I";
        else if (score >= 20)
            messageText.text = "����Ȃ肩�ڂ�����W�߂ꂽ�I";
        else if (score >= 10)
            messageText.text = "�����̂��ڂ�����W�߂ꂽ�I";
        else
            messageText.text = "�P���E���Ȃ������c\n�����P�x�s�����c";
    }
}
