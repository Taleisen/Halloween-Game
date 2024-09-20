using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatrinaGuidance : MonoBehaviour
{
    LibraryPuzzleControl theRules;
    public float moveSpeed;
    Rigidbody2D myRigidBody;
    Vector3 startingPosition;
    bool reset;
    public GameObject startingPositionMarker;
    public GameObject[] movementTargets;
    float movementCountdown;
    int targetSelected;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        theRules = FindObjectOfType<LibraryPuzzleControl>();
        startingPosition = startingPositionMarker.transform.position;
        reset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!theRules.contestStarted)
        {
            if(!reset)
            {
                transform.position = startingPosition;
                reset = true;
            }

        }

        if (reset)
        {
            reset = false;        }
        else if(movementCountdown<=0)
        {
            myRigidBody.velocity = new Vector2(0f, 0f);// stops current momentum
            movementCountdown = Random.Range(.2f, .5f);
            targetSelected = Random.Range(0, movementTargets.Length);
            transform.position = Vector3.MoveTowards(transform.position, movementTargets[targetSelected].transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            movementCountdown -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, movementTargets[targetSelected].transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
