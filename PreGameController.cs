using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameController : MonoBehaviour
{
    public string levelToLoad;

    public void Continue()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PatreonLink()
    {
        Application.OpenURL("http://www.patreon.com/dracheblume");
    }
}
