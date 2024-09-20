using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryHarryController : MonoBehaviour
{
    bool playerMoving;// tells if player is moving
    Vector2 lastMove; // stores x and y values for most recent move
    Rigidbody2D myRigidBody;// the rigidbody on the player
    float moveSpeed = 300f;// movespeed for Harry
    Animator anim;// reference to animator
    public DiningPuzzleController puzzleControl;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();// registers the animator
        myRigidBody = GetComponent<Rigidbody2D>();// registers Harry's Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) 
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            anim.SetBool("PlayerMoving", false);
        }
        if (puzzleControl.contestStarted && !puzzleControl.mealEnd) 
        {
            if (Input.GetAxisRaw("Horizontal") > .5f || Input.GetAxisRaw("Horizontal") < -.5f)// player is moving left or right
            {
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
                playerMoving = true;// sets PlayerMoving bool for animator
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);// sets LastMove float for animator
            }
            if (Input.GetAxisRaw("Horizontal") < .5 && Input.GetAxisRaw("Horizontal") > -.5)// player is not moving horizontally
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);// prevents horizontal sliding
                playerMoving = false;// stops player movement animation
            }
            anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));// sets MoveX to horizontal axis
            anim.SetBool("PlayerMoving", playerMoving);// sets PlayerMoving bool in animator
            anim.SetFloat("LastMoveX", lastMove.x);// sets horizontal direction player is facing
            anim.SetFloat("LastMoveY", 0);// sets vertical direction player is facing
        }
    }
}
