using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private Color black = Color.black;

    // �V�[���؂�ւ��i�{�^���p�j
public void GoToGamePlay()
{
    // �Â�GameManager��j��
    GameManager existing = FindObjectOfType<GameManager>();
    if (existing != null)
    {
        Destroy(existing.gameObject);
    }

    SceneManager.LoadScene("GamePlay");
}
    public void GoToTitle(string SceneName)
    {
        Initiate.Fade(SceneName, black, 1.0f);
    }
    public void GoToResult(string SceneName)
    {
        Initiate.Fade(SceneName, black, 1.0f);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
