using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    bool canBePressed;// this is available when the player is in the correct area
    public KeyCode keyToPress;// this stores the key that is to be pressed
    public KeyCode alternateKeyToPress;
    public bool hasBeenPressed;
    Animator anim;// reference to the animator to change his sprite
    DanceButtonController DBControl;// references the dance directions script assigned to the arrows
    public string dance;// the name of the animation for the direction
    DanceContest theRules;// reference to the dance contest script for score
    AudioSource correctSound;

    // Start is called before the first frame update
    void Start()
    {
        correctSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();// collect the animator from the object
        theRules = FindObjectOfType<DanceContest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canBePressed && !hasBeenPressed)// checks if it is interactable and has not yet been changed
        {
            if (Input.GetKeyDown(keyToPress) || Input.GetKeyDown(alternateKeyToPress))// player presses the correct button
            {
                theRules.score++;// adds one to the score
                Dance();// changes the direction the player is facing
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            DBControl = collision.GetComponent<DanceButtonController>();// assigns the dance button script
            dance = DBControl.danceDirections;// assigns the string for the animation
            keyToPress = DBControl.primaryKeyToPress;// assigns the primary key
            alternateKeyToPress = DBControl.alternateKeyToPress;// assigns the secondary key
            canBePressed = true;// activates note
            DBControl.directionPressable = true;// activates the color change for the arrow
        }
        if (collision.tag == "Destroyer")
        {
            theRules.promptNumber++;
            Destroy(this.gameObject);// cleans up the prompts
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            DBControl.directionPressable = false;// turns off the color change for the arrow
            canBePressed = false;// deactivates note
        }
    }

    public void Dance()
    {
        correctSound.Play();
        anim.Play(dance);// face the direction indicated by the prompt
        hasBeenPressed = true;// stops the script from checking after the animation is active
    }
}
