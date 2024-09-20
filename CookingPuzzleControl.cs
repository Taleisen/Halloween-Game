using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingPuzzleControl : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// starts and stops the puzzle
    public float riseSpeed;// rate that the level rises when you press interact
    public float lowerSpeed;// rate that the level lowers when you are not pressing interact
    float cooktime;// used to count down the cooking timer
    public float recipeTime;// holds the amount of time the player has to keep the level between the values
    public bool cooking;// used to count down the timer during the contest
    public GameObject thermometer;// this is the icon for the thermometer
    Vector2 thermometerStartPosition;// this is the x and y initial position for the thermometer
    CookingContest easyBake;// used to reset the pre-heat bool
    public Canvas timerCanvas;// canvas for cook timer
    public Text timerText;// text to display timer
    public AudioSource backgroundMusic;
    AudioSource puzzleMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        puzzleMusic = this.GetComponent<AudioSource>();
        easyBake = FindObjectOfType<CookingContest>();
        cooking = false;// initializes value
        contestStarted = false;// initializes bool value
        cooktime = recipeTime;// initializes the value of the cooking time
        thermometerStartPosition = thermometer.transform.position;// saves the start position
    }

    // Update is called once per frame
    void Update()
    {
        if (thermometer.transform.position.y < thermometerStartPosition.y) // keeps the thermometer from going below start point
        {
            thermometer.transform.position = thermometerStartPosition;
        }

        if (contestStarted) 
        {
            thermometer.transform.position = new Vector2(thermometer.transform.position.x, thermometer.transform.position.y - lowerSpeed * Time.deltaTime);// this lowers the thermometer when the player is not hitting the interact button
            if (Input.GetButtonDown("Interact")) 
            {
                thermometer.transform.position = new Vector2(thermometer.transform.position.x, thermometer.transform.position.y + riseSpeed * Time.deltaTime);// raises the thermometer when you press the interact button
            }
        }

        if (cooking) 
        {
            if (cooktime < 0)// prevents a negative number from displaying for the timer
            {
                cooktime = 0;
            }
            if (cooktime > 0) 
            {
                cooktime -= Time.deltaTime;
                timerText.text = "" + Mathf.RoundToInt(cooktime);
            }
            else 
            {
                GameWin();
            }
        }
    }

    public void Intro()// introduces the puzzle
    {
        backgroundMusic.Pause();
        puzzleMusic.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "Time to show me your cooking skills. Tap the interact key to heat the oven. The pre-heating is done when it gets to the green. Keep it at in the green area for 15 seconds after you have pre-heated it to win. Are you ready?";// challenge from opponent 
    }

    public void GameStart() 
    {
        if (!puzzleMusic.isPlaying)
        {
            puzzleMusic.Play();
        }
        cooktime = recipeTime;// resets the cooking time value
        theCanvas.gameObject.SetActive(false);// turns off the canvas
        easyBake.preHeat = false;// resets the pre-heating bool
        thermometer.transform.position = thermometerStartPosition;// resets the thermometer position
        contestStarted = true;// starts the puzzle
    }

    public void GameWin()// player wins
    {
        puzzleMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        cooking = false;// turns cooking bool off
        timerCanvas.gameObject.SetActive(false);// turns off the timer canvas
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        thermometer.transform.position = thermometerStartPosition;// resets the thermometer position
        easyBake.preHeat = false;// resets the pre-heat bool
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
    }

    public void GameLose()// player loses
    {
        puzzleMusic.Pause();
        loseSound.Play();
        contestStarted = false;// stops contest
        cooking = false;// turns off cooking bool
        timerCanvas.gameObject.SetActive(false);// turns off the timer canvas
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never win like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        puzzleMusic.Stop();
        backgroundMusic.Play();
        timerCanvas.gameObject.SetActive(false);// turns off the timer canvas
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }

    public void TimerStart()
    {
        timerCanvas.gameObject.SetActive(true);// activates the timer canvas
    }
}
