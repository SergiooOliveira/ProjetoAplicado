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

    public void MapSelect()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void Map2_cloud()
    {
        Debug.Log("hello world");
        SceneManager.LoadScene("Map2_cloud");
    }

    public void test()
    {
        Debug.Log("test");
    }
}
