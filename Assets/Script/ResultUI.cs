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
            messageText.text = "�ƂĂ���ʂ̂��ڂ�����W�߂ꂽ�I���ڂ�����W�߂�̂ɖ����ɂȂ���������c";
        else if (score >= 50)
            messageText.text = "��������̂��ڂ�����W�߂ꂽ�I������������Ȃ��c�I";
        else if (score >= 40)
            messageText.text = "�����ς����ڂ�����W�߂ꂽ�I�W�߂��������ȁc�H";
        else if (score >= 30)
            messageText.text = "�\���ȗʂ̂��ڂ�����W�߂ꂽ�I";
        else if (score >= 20)
            messageText.text = "����Ȃ肩�ڂ�����W�߂ꂽ�I";
        else if (score >= 10)
            messageText.text = "�����̂��ڂ�����W�߂ꂽ�I";
        else if (score >= 1)
            messageText.text = "������Ƃ������ڂ�����W�߂ꂽ�I";
        else if (score == 0)
            messageText.text = "�P���E���Ȃ������c�B�����P�x�s�����c";
        else
            messageText.text = "�\�����ĂȂ����l";
    }
}
