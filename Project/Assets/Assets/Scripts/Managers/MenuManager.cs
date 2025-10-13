using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("Swapping scene");
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
    
    public void Skip()
    {
        SceneManager.LoadScene("Map1_Part1");
    }
}
