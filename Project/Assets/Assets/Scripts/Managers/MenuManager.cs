using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("Swapping scene");
        SceneManager.LoadScene("Map1_Part1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
