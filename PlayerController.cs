using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject player;// defines the player object
    public float moveSpeed; // player's movement speed
    Animator anim; // reference to the animator
    bool playerMoving;// tells if player is moving
    Vector2 lastMove; // stores x and y values for most recent move
    Rigidbody2D myRigidBody;// the rigidbody on the player
    TeleportationController doorway;//reference to the teleportation controller
    public bool canMove;// allows the character to become immobilized
    public bool gateSick;// bool for gatesickness

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;// sets the object this is connected to as the player object
        anim = GetComponent<Animator>();// sets the animator on this object as anim
        myRigidBody = GetComponent<Rigidbody2D>();// collects the rigidbody from the player
        canMove = true;// sets player able to move by default
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;// resets PlayerMoving bool for animator
        anim.SetBool("GateSick", gateSick);

        if (!canMove)
        {
            myRigidBody.velocity = new Vector2(0f, 0f);// stops current momentum
            anim.SetBool("PlayerMoving", false);// turns off walking animations
            return;
        }

        if (Input.GetAxisRaw("Horizontal") > .5f || Input.GetAxisRaw("Horizontal") <-.5f)// player is moving left or right
        {
            myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
            playerMoving = true;// sets PlayerMoving bool for animator
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);// sets LastMove float for animator
        }
        if (Input.GetAxisRaw("Vertical") > .5f || Input.GetAxisRaw("Vertical") < -.5f)// player is moving up or down
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);// adds vertical force to the rigidbody
            playerMoving = true;// sets PlayerMoving bool for animator
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

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));// sets MoveX to horizontal axis
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));// sets MoveY to vertical axis
        anim.SetBool("PlayerMoving", playerMoving);// sets PlayerMoving bool in animator
        anim.SetFloat("LastMoveX", lastMove.x);// sets horizontal direction player is facing
        anim.SetFloat("LastMoveY", lastMove.y);// sets vertical direction player is facing
    }
}
