using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationController : MonoBehaviour
{
    public GameObject target;// target for teleport location
    public GameObject player;// reference to the player
    Camera theCamera;// reference to the camera
    public float timer = 2f;// time the character will be immobile after entering a doorway useful when entering the upstairs to avoid going back downstairs
    private float timeCounter;// used to track the time character is immobile
    PlayerController thePC;// the player controller to access movespeed
    bool gateSick;// a bool to track if the character is immobile

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<Camera>();// registers the camera
        player = FindObjectOfType<PlayerController>().gameObject;// identifies the player
        thePC = player.GetComponent<PlayerController>();// identifies the player controller
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, player.transform.position.z);// moves player to the target
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z); // moves camera to the target
    }

    private void GateSick()
    {
        thePC.canMove = false;// stops character movement
        timeCounter = timer;// resets the time for the delay after entering a door
        thePC.gateSick = true;// tells the player controller the player is gate sick
        gateSick = true;// sets the character as gatesick
    }

    private void Update()
    {
        if(timeCounter > 0f)// counts down timer
        {
            timeCounter -= Time.deltaTime;// counts down the timer
        }
        else if (gateSick)// returns mobility to the player
        {
            thePC.canMove = true;// allows player to move again
            thePC.gateSick = false;// turns off gate sickness
            gateSick = false;// sets the character as moveable
        }
        else// player is mobile
        {
            return;
        }
    }
}
