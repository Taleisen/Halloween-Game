using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceContest : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// tells if the contest is started
    public GameObject leftArrowSpawn;// left arrow spawn location
    public GameObject rightArrowSpawn;// right arrow spawn location
    public GameObject upArrowSpawn;// up arrow spawn location
    public GameObject downArrowSpawn;// down arrow spawn location
    public GameObject arrows;// holds a copy of the beat marker prefabs for instantiation
    int arrowCount;// counts the number of arrows that have been created
    int arrowSelect;// used to house the integer to select which arrow is chosen
    float timeBetweenNotes;// this is the base time between notes and will be randomly selected between a range to adjust play
    float notesCounter;// this is counted down and reset to keep a timer for the time between prompts
    bool delayArrows;// this allows me to count down the time between notes without having to create a second float
    public int score;// holds the current score for win/loss
    public bool songEnd;// bool to indicate all prompts have been destroyed
    public int promptNumber;// keeps track of each prompt
    public Canvas scoreCanvas;// canvas to display score
    public Text scoreCounterText;// location to display the score
    public AudioSource backgroundMusic;
    public AudioSource danceMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        arrowCount = 0;// resets the counter to 0 to start
        score = 0;// sets score to 0
        promptNumber = 0;// resets the prompt number
        contestStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounterText.text = "Score: " + score;// sets the text for the score

        if (promptNumber == 20)// all prompts have been destroyed
        {
            songEnd = true;
        }
        if (contestStarted && !delayArrows)// timer for the time between button prompts
        {
            if (timeBetweenNotes > 0)
            {
                timeBetweenNotes -= Time.deltaTime;
            }
            else
            {
                delayArrows = true;
                ArrowSelection();
            }
        }
        if (contestStarted && songEnd)
        {
            if (score >= 10)// sets win condition
            {
                GameWin();
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
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "Press the corresponding arrow key when the arrow turns green to score a point, score at least 10 to win. Are you ready to show me your moves?";// challenge from opponent 
        danceMusic.Play();
    }

    public void GameStart()// starts the puzzle
    {
        if (!danceMusic.isPlaying)
        {
            danceMusic.Play();
        }
        promptNumber = 0;// resets the prompt number
        arrowCount = 0;// resets arrow count
        songEnd = false;// tells the program that it hasn't ended
        theCanvas.gameObject.SetActive(false);// turns off the dialog canvas
        score = 0;// resets score
        contestStarted = true;// starts contest
        delayArrows = false;// turns on arrow production
        scoreCanvas.gameObject.SetActive(true);// activates the score canvas
    }

    void GameWin()// player wins
    {
        danceMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
    }

    void GameLose()// player loses
    {
        danceMusic.Pause();
        loseSound.Play();
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never beat me like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        danceMusic.Stop();
        backgroundMusic.Play();
        scoreCanvas.gameObject.SetActive(false);// deactivates the score canvas
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }

    void ArrowSelection()
        {
        if (arrowCount < 20)// if there have been less than 20 prompts
        {
            arrowSelect = Random.Range(0, 4);// randomly selects which arrow is selected
            arrowCount++;// increases the number of prompts that have passed
            timeBetweenNotes = Random.Range(.5f, 1.2f);// sets the timer for the next prompt

           if (arrowSelect == 0)// if it selected the left arrow
           {
                Instantiate(arrows, leftArrowSpawn.transform.position, leftArrowSpawn.transform.rotation);
                delayArrows = false;// starts timer for next prompt
            }
           else if (arrowSelect == 1)// if it selected the right arrow
           {
                Instantiate(arrows, upArrowSpawn.transform.position, upArrowSpawn.transform.rotation);
                delayArrows = false;// starts timer for next prompt
            }
            else if (arrowSelect == 2)// if it selected the down arrow
            {
                Instantiate(arrows, downArrowSpawn.transform.position, downArrowSpawn.transform.rotation);
                delayArrows = false;// starts timer for next prompt
            }
            else// if it selected the right arrow
            {
                Instantiate(arrows, rightArrowSpawn.transform.position, rightArrowSpawn.transform.rotation);
                delayArrows = false;// starts timer for next prompt
            }
        }
    }
}
