using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingGlassController : MonoBehaviour
{

    public HideAndSeek theRules;// reference to game controller
    Rigidbody2D myRigidBody;// reference to rigidbody
    public float moveSpeed;// rate the icon moves

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();// registers the rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        if (theRules.contestStarted) 
        {
            if (Input.GetAxisRaw("Horizontal") > .5f || Input.GetAxisRaw("Horizontal") < -.5f)// player is moving left or right
            {
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, myRigidBody.velocity.y);// adds horizontal force to rigidbody
            }
            if (Input.GetAxisRaw("Horizontal") < .5 && Input.GetAxisRaw("Horizontal") > -.5)// player is not moving horizontally
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);// prevents horizontal sliding
            }
            if (Input.GetAxisRaw("Vertical") > .5f || Input.GetAxisRaw("Vertical") < -.5f)// player is moving left or right
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
            }
            if (Input.GetAxisRaw("Vertical") < .5 && Input.GetAxisRaw("Vertical") > -.5)// player is not moving horizontally
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
            }
        }
    }
}
