using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolRace : MonoBehaviour
{
    public float playerMoveSpeed;// speed for player's token
    public float opponentMoveSpeed;// speed for opponent token
    public GameObject playerToken;// reference to player token
    public GameObject opponentToken;// reference to opponent token
    bool gameActive;// bool for if race is running
    CameraController theCamera;// reference to the camera
    public PuzzleTrigger pT;// reference to the trigger
    Vector3 playerTokenStartPosition;// stores the player token start position
    Vector3 opponentTokenStartPosition;// stores the opponent token start position
    public Canvas theCanvas;// reference to the UI canvas
    public Image dialogBox;// reference to image that contains the dialog box
    public AudioSource backgroundMusic;
    AudioSource poolMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        poolMusic = GetComponent<AudioSource>();
        theCamera = FindObjectOfType<CameraController>();// registers camera
        playerTokenStartPosition = playerToken.transform.position;// stores player token start position
        opponentTokenStartPosition = opponentToken.transform.position;// stores opponent token start position
        gameActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            opponentToken.transform.position = new Vector3(opponentToken.transform.position.x + 1 * opponentMoveSpeed * Time.deltaTime, opponentToken.transform.position.y, opponentToken.transform.position.z);// opponent moves at steady rate
            if (Input.GetButtonDown("Interact"))
            {
                playerToken.transform.position = new Vector3(playerToken.transform.position.x + 1 * playerMoveSpeed * Time.deltaTime, playerToken.transform.position.y, playerToken.transform.position.z);// tap the jump button to move player token
            }
        }
    }

    public void Intro()
    {
        backgroundMusic.Pause();
        poolMusic.Play();
        playerToken.transform.position = playerTokenStartPosition;// resets player token to start position
        opponentToken.transform.position = opponentTokenStartPosition;// resets opponent token to start position
        theCamera.GetComponent<Camera>().orthographicSize = 6;// changes camera size to fit pool dimensions
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "I'll be red, you'll be blue. Tap the interact button as fast as you can. First one to the other side wins. Are you ready?";// challenge from opponent    
    }

    public void GameStart()// starts the puzzle
    {
        if (!poolMusic.isPlaying)
        {
            poolMusic.Play();
        }
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        playerToken.transform.position = playerTokenStartPosition;// resets player token to start position
        opponentToken.transform.position = opponentTokenStartPosition;// resets opponent token to start position
        gameActive = true;// starts game
    }

    void GameWin()
    {
        poolMusic.Pause();
        winSound.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
    }

    void GameLose()
    {
        poolMusic.Pause();
        loseSound.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never beat me like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// ends the puzzle
    {
        poolMusic.Stop();
        backgroundMusic.Play();
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        theCamera.GetComponent<Camera>().orthographicSize = 5;// reverts camera size to default
        pT.GameReturn();// return to normal game
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameActive = false;// stops game

        if (other.tag == "Player")
        {
            GameWin();
        }
        else
        {
            GameLose();
        }
    }
}
