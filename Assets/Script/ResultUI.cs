using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;


    void Start()
    {
        // GameManager からスコアを受け取る（保存済みの PlayerPrefs から取得）
        int score = PlayerPrefs.GetInt("PumpkinScore", 0);
        scoreText.text = $"集めたかぼちゃ：{score}個";

        // スコアに応じたメッセージを切り替える
        if (score >= 60)
            messageText.text = "とても大量のかぼちゃを集めれた！かぼちゃを集めるのに夢中になっちゃった…";
        else if (score >= 50)
            messageText.text = "たくさんのかぼちゃを集めれた！もう持ちきれない…！";
        else if (score >= 40)
            messageText.text = "いっぱいかぼちゃを集めれた！集めすぎたかな…？";
        else if (score >= 30)
            messageText.text = "十分な量のかぼちゃを集めれた！";
        else if (score >= 20)
            messageText.text = "それなりかぼちゃを集めれた！";
        else if (score >= 10)
            messageText.text = "少しのかぼちゃを集めれた！";
        else if (score >= 1)
            messageText.text = "ちょっとだけかぼちゃを集めれた！";
        else if (score == 0)
            messageText.text = "１個も拾えなかった…。もう１度行こう…";
        else
            messageText.text = "予測してない数値";
    }
}
