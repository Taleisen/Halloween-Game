using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTrigger : MonoBehaviour
{
    PlayerController thePC;// reference to the player controller
    CameraController theCamera;// reference to the camera
    public GameObject puzzle;// reference a game object to show the puzzle
    public GameObject puzzleScriptObject;// reference to object holding the script for the puzzle
    public bool hasWon;// used to determine if the player has won this challenge already
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public GameManager theGM;
    bool reportedWin;
    ParticleSystem darkCloud;

    // Start is called before the first frame update
    void Start()
    {
        darkCloud = GetComponent<ParticleSystem>();
        thePC = FindObjectOfType<PlayerController>();// defines the player
        theCamera = FindObjectOfType<CameraController>();// defines the camera
        theGM = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)// triggered when player bumps into collider
    {
        thePC.canMove = false;// stops player from moving during puzzle

        if (!hasWon)// player has not completed the puzzle
        {
            theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
            dialogBox.GetComponentInChildren<Text>().text = "Would you like to challenge me?";// challenge from opponent
        }
        if (hasWon)// player has completed the puzzle
        {
            theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
            dialogBox.GetComponentInChildren<Text>().text = "Thank you for freeing me. Would you like to play again?";// option to challenge again
        }
    }

    public void PuzzleStart()
    {
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        theCamera.transform.position = new Vector3(puzzle.transform.position.x, puzzle.transform.position.y, theCamera.gameObject.transform.position.z);// moves the camera to the puzzle location
        theCamera.followTarget = puzzle;// sets the focus for the camera during the puzzle
        puzzleScriptObject.GetComponent<MonoBehaviour>().Invoke("Intro", 0f);// activates the intro for the puzzle
    }

    public void GameReturn()
    {
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        theCamera.FollowPlayer();// returns camera focus to the player
        theCamera.transform.position = new Vector3(thePC.gameObject.transform.position.x, thePC.gameObject.transform.position.y, theCamera.transform.position.z);// returns the camera to the player location
        thePC.canMove = true;// allows the player to begin moving
        CheckForWin();
    }

    void CheckForWin()
    {
        if (hasWon && !reportedWin)
        {
            theGM.puzzlesCompleted++;
            reportedWin = true;
            darkCloud.Stop();
            if (theGM.puzzlesCompleted == 8)
            {
                theGM.EndGame();
            }
        }
    }
}
