using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryPuzzleControl : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// tells if the contest is started
    public Canvas timerCanvas;
    public Text timerText;
    public float timerInit;
    float timer;
    public AudioSource backgroundMusic;
    AudioSource puzzleMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        puzzleMusic = GetComponent<AudioSource>();
        contestStarted = false;
        timer = timerInit;
    }

    // Update is called once per frame
    void Update()
    {
        if (contestStarted)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.text = "" + Mathf.RoundToInt(timer);
            }
            else
            {
                GameLose();
            }
        }
    }

    public void Intro()// introduces the puzzle
    {
        backgroundMusic.Pause();
        puzzleMusic.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "Catch me if you can, you've got 1 minute. Are you ready?";// challenge from opponent 
    }

    public void GameStart()// starts the puzzle
    {
        if (!puzzleMusic.isPlaying)
        {
            puzzleMusic.Play();
        }
        timer = timerInit;
        timerCanvas.gameObject.SetActive(true);
        theCanvas.gameObject.SetActive(false);// turns off the dialog canvas
        contestStarted = true;// starts contest

    }

    public void GameWin()// player wins
    {
        puzzleMusic.Pause();
        winSound.Play();
        timerCanvas.gameObject.SetActive(false);
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
    }

    void GameLose()// player loses
    {
        puzzleMusic.Pause();
        loseSound.Play();
        timerCanvas.gameObject.SetActive(false);
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never beat me like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        puzzleMusic.Stop();
        backgroundMusic.Play();
        timerCanvas.gameObject.SetActive(false);
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }
}
