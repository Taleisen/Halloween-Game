using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayroomPuzzleControl : MonoBehaviour
{
    public PuzzleTrigger pT;// reference to the trigger
    public Image dialogBox;// reference to image that contains the dialog box
    public Canvas theCanvas;// reference to UI canvas
    public bool contestStarted;// tells if the contest is started
    Animator anim;
    Rigidbody2D myRigidBody;
    public float moveSpeed;
    Vector2 lastMove;
    bool walking;
    Vector3 startPosition;
    public Canvas quitPuzzleCanvas;
    public float timer = 300;// sets the start amount for the timer
    float timerCounter;// used to count down the timer
    public Canvas counterCanvas;// canvas for the timer
    public Text counterText;// text to display the timer
    public AudioSource backgroundMusic;
    AudioSource mazeMusic;
    public AudioSource winSound;
    public AudioSource loseSound;

    // Start is called before the first frame update
    void Start()
    {
        mazeMusic = this.GetComponent<AudioSource>();
        timerCounter = timer;
        quitPuzzleCanvas.gameObject.SetActive(false);
        walking = false;
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        contestStarted = false;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Walking", walking);
        if (!contestStarted) 
        {
            myRigidBody.velocity = new Vector2(0f, 0f);// stops current momentum
            anim.SetBool("Walking", false);// turns off walking animations
            return;
        }
        if (Input.GetAxisRaw("Horizontal") > .5f || Input.GetAxisRaw("Horizontal") < -.5f)// player is moving left or right
        {
            myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
            walking = true;// sets PlayerMoving bool for animator
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);// sets LastMove float for animator
        }
        if (Input.GetAxisRaw("Vertical") > .5f || Input.GetAxisRaw("Vertical") < -.5f)// player is moving up or down
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);// adds vertical force to the rigidbody
            walking = true;// sets PlayerMoving bool for animator
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));// sets LastMove float for animator
        }
        if (Input.GetAxisRaw("Horizontal") < .5 && Input.GetAxisRaw("Horizontal") > -.5)// player is not moving horizontally
        {
            myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);// prevents horizontal sliding
        }
        if (Input.GetAxisRaw("Vertical") < .5 && Input.GetAxisRaw("Vertical") > -.5)// player is not moving vertically
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);// prevents vertical sliding
        }
        if(Input.GetAxisRaw("Horizontal") < .5 && Input.GetAxisRaw("Horizontal") > -.5 && Input.GetAxisRaw("Vertical") < .5 && Input.GetAxisRaw("Vertical") > -.5) 
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            walking = false;
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));// sets MoveX to horizontal axis
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));// sets MoveY to vertical axis
        anim.SetBool("Walking", walking);// sets PlayerMoving bool in animator
        anim.SetFloat("LastMoveX", lastMove.x);// sets horizontal direction player is facing
        anim.SetFloat("LastMoveY", lastMove.y);// sets vertical direction player is facing

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
        mazeMusic.Play();
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "Help Mr. Biggles get to the toy box. You have 5 minutes. Are you ready?";// challenge from opponent 
    }

    public void GameStart()// starts the puzzle
    {
        if (!mazeMusic.isPlaying)
        {
            mazeMusic.Play();
        }
        quitPuzzleCanvas.gameObject.SetActive(true);
        transform.position = startPosition;
        anim.SetFloat("LastMoveX", 0f);
        anim.SetFloat("LastMoveY", -1f);
        theCanvas.gameObject.SetActive(false);// turns off the dialog canvas
        contestStarted = true;// starts contest
        timerCounter = timer;
    }

    void GameWin()// player wins
    {
        mazeMusic.Pause();
        winSound.Play();
        contestStarted = false;// stops contest
        counterCanvas.gameObject.SetActive(false);// deactivates the timer
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You won. Want to play again?";// challenge from opponent
        pT.hasWon = true;// tells the puzzle trigger that this puzzle has been beaten
    }

    public void GameLose()// player loses
    {
        mazeMusic.Pause();
        loseSound.Play();
        contestStarted = false;// stops contest
        counterCanvas.gameObject.SetActive(false);// deactivates the timer
        theCanvas.gameObject.SetActive(true);// activates canvas to reveal dialog
        dialogBox.GetComponentInChildren<Text>().text = "You'll never beat me like that. Want to try again?";// challenge from opponent
    }

    public void GameEnd()// exits the puzzle
    {
        mazeMusic.Stop();
        backgroundMusic.Play();
        counterCanvas.gameObject.SetActive(false);// deactivates the timer
        theCanvas.gameObject.SetActive(false);// deactivates canvas
        pT.GameReturn();// return to normal game
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            GameWin();
        }
    }
}
