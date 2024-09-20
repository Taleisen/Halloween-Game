using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiningPuzzleController : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// tells if the contest is started
    public GameObject food;// holds a copy of the beat marker prefabs for instantiation
    int foodCount;// counts the number of food that has been created
    float timeBetweenMeals;// this is the base time between notes and will be randomly selected between a range to adjust play
    float foodCounter;// this is counted down and reset to keep a timer for the time between prompts
    bool delayFood;// this allows me to count down the time between notes without having to create a second float
    public int score;// holds the current score for win/loss
    public bool mealEnd;// bool to indicate all prompts have been destroyed
    public int promptNumber;// keeps track of each prompt
    public Canvas scoreCanvas;// canvas to display score
    public Text scoreCounterText;// location to display the score
    int spawnSelect;// selects food spawn location
    public GameObject[] spawnLocation;// holds the spawn locations
    Vector3 startingPosition;// stores player starting position for reset
    public GameObject harry;// stores the player's avatar for reset
    public AudioSource backgroundMusic;
    AudioSource diningMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        diningMusic = this.GetComponent<AudioSource>();
        startingPosition = new Vector3(harry.transform.position.x, harry.transform.position.y, harry.transform.position.z);// stores starting location
        foodCount = 0;// resets the counter to 0 to start
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
            mealEnd = true;
        }
        if (contestStarted && !delayFood)// timer for the time between button prompts
        {
            if (timeBetweenMeals > 0)
            {
                timeBetweenMeals -= Time.deltaTime;
            }
            else
            {
                delayFood = true;
                FoodSelection();
            }
        }
        if (contestStarted && mealEnd)
        {
            harry.GetComponent<HungryHarryController>().canMove = false;// stops the player from moving after the game ends

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
        diningMusic.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "Catch the food before it hits the ground to score a point, score at least 10 to win. Are you ready?";// challenge from opponent 
    }
    
    public void GameStart()// starts the puzzle
    {
        if (!diningMusic.isPlaying)
        {
            diningMusic.Play();
        }
        harry.transform.position = startingPosition;// resets player to starting position
        promptNumber = 0;// resets the prompt number
        foodCount = 0;// resets food count
        mealEnd = false;// tells the program that it hasn't ended
        theCanvas.gameObject.SetActive(false);// turns off the dialog canvas
        score = 0;// resets score
        contestStarted = true;// starts contest
        delayFood = false;// turns on arrow production
        scoreCanvas.gameObject.SetActive(true);// activates the score canvas

    }

    void GameWin()// player wins
    {
        diningMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
    }

    void GameLose()// player loses
    {
        diningMusic.Pause();
        loseSound.Play();
        contestStarted = false;// stops contest
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never beat me like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        diningMusic.Stop();
        backgroundMusic.Play();
        scoreCanvas.gameObject.SetActive(false);// deactivates the score canvas
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }
    void FoodSelection()
    {
        if (foodCount < 20)// if there have been less than 20 prompts
        {
            spawnSelect = Random.Range(0, 13);// randomly selects where food spawns
            foodCount++;// increases the number of prompts that have passed
            timeBetweenMeals = Random.Range(.5f, 1.2f);// sets the timer for the next prompt
            Instantiate(food, spawnLocation[spawnSelect].transform.position, spawnLocation[spawnSelect].transform.rotation);// generates food item at spawn location
            delayFood = false;// starts timer for next prompt
        }
    }
}
