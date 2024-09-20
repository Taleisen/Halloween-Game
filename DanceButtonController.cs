using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceButtonController : MonoBehaviour
{
    public string danceDirections;
    SpriteRenderer theSR;// reference to object's sprite renderer
    public Sprite defaultImage;// reference to standard button image
    public Sprite pressedImage;// reference to pressed button image
    public DanceContest theContest;// reference to dance contest script
    public KeyCode primaryKeyToPress;// defines primary keycode for the button
    public KeyCode alternateKeyToPress;// defines an alternate button to press
    public GameObject directionKey;// stores an object for the direction arrow in front of the button
    public bool directionPressable;// bool to tell if it should be changed
    
    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();// registers the sprite renderer
        directionKey.GetComponent<SpriteRenderer>().color = Color.white;// restores the arrow to white
    }

    // Update is called once per frame
    void Update()
    {
        if (theContest.contestStarted)// checks to see if the challenge has started
        {
            if (directionPressable)
            {
                directionKey.GetComponent<SpriteRenderer>().color = Color.green;// turns the arrow green
            }
            else
            {
                directionKey.GetComponent<SpriteRenderer>().color = Color.white;// restores the arrow to white
            }
            if (Input.GetKeyDown(primaryKeyToPress) || Input.GetKeyDown(alternateKeyToPress))// activates when key is pressed
            {
                theSR.sprite = pressedImage;
            }

            if (Input.GetKeyUp(primaryKeyToPress) || Input.GetKeyUp(alternateKeyToPress))// activates when key is released
            {
                theSR.sprite = defaultImage;
            }
        }
    }
}
