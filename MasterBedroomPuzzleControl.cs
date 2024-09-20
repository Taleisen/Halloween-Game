using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterBedroomPuzzleControl : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// starts and stops the puzzle
    BlockPuzzleController puzzle;// reference to the puzzle script
    public Camera theCamera;// reference to the camera to set orthographic size
    public float timer = 300;// sets the start amount for the timer
    float timerCounter;// used to count down the timer
    public Canvas counterCanvas;// canvas for the timer
    public Text counterText;// text to display the timer
    public AudioSource backgroundMusic;
    AudioSource puzzleMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        puzzleMusic = this.GetComponent<AudioSource>();
        puzzle = FindObjectOfType<BlockPuzzleController>();// registers the block puzzle controller
        contestStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (contestStarted && timerCounter > 0)
        {
            timerCounter -= Time.deltaTime;
            counterText.text = "" + Mathf.RoundToInt(timerCounter);// shows how many seconds left in whole numbers
        }
        if (contestStarted && timerCounter <= 0)
        {
            counterCanvas.gameObject.SetActive(false);// deactivates the timer
            GameLose();
        }
    }

    public void Intro()// introduces the puzzle
    {
        backgroundMusic.Pause();
        puzzleMusic.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "I'd like you to fix this picture. Click on the tile to move it to the open space. It will only move if it is next to the empty spot. You have 5 minutes. Are you ready?";// challenge from opponent 
    }

    public void GameStart()
    {
        if (!puzzleMusic.isPlaying)
        {
            puzzleMusic.Play();
        }
        timerCounter = timer;
        theCanvas.gameObject.SetActive(false);// turns off the canvas
        theCamera.orthographicSize = 2;// zooms the camera in on the puzzle
        counterCanvas.gameObject.SetActive(true);
        puzzle.StartShuffle();// initiates the block puzzle controller
    }

    public void GameWin()// player wins
    {
        puzzleMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        counterCanvas.gameObject.SetActive(false);
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
    }

    public void GameLose()// player loses
    {
        puzzleMusic.Pause();
        loseSound.Play();
        contestStarted = false;// stops contest
        counterCanvas.gameObject.SetActive(false);
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        puzzle.ResetPuzzle();
        dialogBox.GetComponentInChildren<Text>().text = "You lost. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        puzzleMusic.Stop();
        backgroundMusic.Play();
        counterCanvas.gameObject.SetActive(false);
        theCamera.orthographicSize = 5;// resets the orthographic size
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }
}
