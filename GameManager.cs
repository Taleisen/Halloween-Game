using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject controlsImage;
    public int puzzlesCompleted;
    public Canvas hudCanvas;
    public Canvas endGameGanvas;
    PlayerController thePlayer;
    public AudioSource backgroundMusic;
    public AudioSource wonMusic;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayControls()
    {
        controlsImage.SetActive(true);
    }

    public void CloseControls()
    {
        controlsImage.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        backgroundMusic.Pause();
        wonMusic.Play();
        thePlayer.canMove = false;
        hudCanvas.gameObject.SetActive(false);
        endGameGanvas.gameObject.SetActive(true);
    }

    public void ContinuePlaying()
    {
        wonMusic.Stop();
        backgroundMusic.Play();
        thePlayer.canMove = true;
        endGameGanvas.gameObject.SetActive(false);
        hudCanvas.gameObject.SetActive(true);
    }
}
