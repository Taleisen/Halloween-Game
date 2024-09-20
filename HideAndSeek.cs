using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAndSeek : MonoBehaviour
{

    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// starts and stops the puzzle
    public GameObject lookingGlass;// reference to the player's avatar
    public GameObject[] spawnPoints;// reference of spawn points
    public GameObject teddy;// reference to the teddy bear to spawn
    int selectedPoint;// the spawn point selected
    Vector3 lookingGlassStartPosition;// stores the starting point for the looking glass
    TeddyController theTeddy;// reference to the bear to reset scene
    public float timer;// sets the start amount for the timer
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
        lookingGlassStartPosition = lookingGlass.transform.position;
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
        dialogBox.GetComponentInChildren<Text>().text = "I've lost my favorite bear. He can only be seen with my special looking glass. You have 15 second to find it. Move the spyglass to reveal him and press the interact button when you have found it. Will you help me find it?";// challenge from opponent 
    }

    public void GameStart()
    {
        if (!puzzleMusic.isPlaying)
        {
            puzzleMusic.Play();
        }
        timerCounter = timer;// sets the timer
        counterCanvas.gameObject.SetActive(true);// displays the timer
        lookingGlass.transform.position = lookingGlassStartPosition;// resets the looking glass position
        selectedPoint = Random.Range(0, 25);// randomly selects spawn point
        theCanvas.gameObject.SetActive(false);// turns off the canvas
        contestStarted = true;// starts the puzzle
        Instantiate(teddy, spawnPoints[selectedPoint].transform.position, spawnPoints[selectedPoint].transform.rotation);// puts the teddy bear at the selected spawn location
        theTeddy = FindObjectOfType<TeddyController>();// assigns the teddy bear
    }

    public void GameWin()// player wins
    {
        puzzleMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        counterCanvas.gameObject.SetActive(false);
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
        dialogBox.GetComponentInChildren<Text>().text = "You found it. Want to play again?";// challenge from opponent
    }

    public void GameLose()// player loses
    {
        puzzleMusic.Pause();
        loseSound.Play();
        counterCanvas.gameObject.SetActive(false);// deactivates the timer
        contestStarted = false;// stops contest
        Destroy(theTeddy.gameObject);// destroys the old bear
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "It's still lost. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        puzzleMusic.Stop();
        backgroundMusic.Play();
        counterCanvas.gameObject.SetActive(false);
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }
}
