using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {

    float delayTime = 5f;
    public string levelToLoad;

	void Update()
	{
		delayTime -= Time.deltaTime;
		if (delayTime <= 0) 
		{
			SceneManager.LoadScene (levelToLoad);
		}
	}
}
