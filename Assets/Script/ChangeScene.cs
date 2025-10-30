using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private Color black = Color.black;

    // �V�[���؂�ւ��i�{�^���p�j
    public void GoToGame(string SceneName)
    {
        Initiate.Fade(SceneName,black,1.0f);
        //BGMManager.instance.PlayBGM(BGMManager.instance.gameBGM);
    }
    public void GoToTitle(string SceneName)
    {
        Initiate.Fade(SceneName, black, 1.0f);
        //BGMManager.instance.PlayBGM(BGMManager.instance.titleBGM);
    }

    public void GoToResult(string SceneName)
    {
        Initiate.Fade(SceneName, black, 1.0f);
        //BGMManager.instance.PlayBGM(BGMManager.instance.resultBGM);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
