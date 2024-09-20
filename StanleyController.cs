using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanleyController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    Animator anim;
    LibraryPuzzleControl theRules;
    bool moving;
    Rigidbody2D myRigidBody;
    bool grounded;
    public GameObject topLeftMarker;
    public GameObject bottomRightMarker;
    Vector2 topLeft;
    Vector2 bottomRight;
    public LayerMask ground;
    Vector3 startingPosition;
    bool reset;
    public GameObject startingPositionMarker;

    // Start is called before the first frame update
    void Start()
    {
        topLeft = new Vector2(topLeftMarker.transform.position.x, topLeftMarker.transform.position.y);
        bottomRight = new Vector2(bottomRightMarker.transform.position.x, bottomRightMarker.transform.position.y);
        anim = GetComponent<Animator>();
        theRules = FindObjectOfType<LibraryPuzzleControl>();
        moving = false;
        myRigidBody = GetComponent<Rigidbody2D>();
        startingPosition = startingPositionMarker.transform.position;
        reset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (theRules.contestStarted) 
        {
            if (reset)
            {
                reset = false;
            }
            topLeft = new Vector2(topLeftMarker.transform.position.x, topLeftMarker.transform.position.y);
            bottomRight = new Vector2(bottomRightMarker.transform.position.x, bottomRightMarker.transform.position.y);
            grounded = Physics2D.OverlapArea(topLeft, bottomRight, ground);

            if (Input.GetAxisRaw("Horizontal") > .5f) 
            {
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
                transform.localScale = new Vector3(1, 1, 1);
                if (grounded)
                {
                    moving = true;
                }
            }
            
            if(Input.GetAxisRaw("Horizontal") < -.5f) 
            {
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
                transform.localScale = new Vector3(-1, 1, 1);
                if (grounded)
                {
                    moving = true;
                }
            }

            if (Input.GetAxisRaw("Horizontal") > -.5f && Input.GetAxisRaw("Horizontal") < .5f) 
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);// adds horizontal force to rigidbody
                moving = false;
            }

            if(Input.GetButtonDown("Interact")&& grounded) 
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
            }

            if (!grounded) 
            {
                moving = false;
            }

            anim.SetBool("Falling", !grounded);
            anim.SetBool("Walking", moving);
        }
        else
        {
            if (!reset)
            {
                transform.position = startingPosition;
                reset = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            theRules.GameWin();
        }
    }
}
