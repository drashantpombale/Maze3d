using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button retry;
    public Button exit;
    // Start is called before the first frame update
    void Start()
    {
        retry.onClick.AddListener(Retry);
        exit.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void Retry()
    {
        SceneController.Instance.LoadGameScene(1);
    }
}
