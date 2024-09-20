using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingContest : MonoBehaviour
{
    public bool preHeat;// active when the pre-heating is done to start the game
    CookingPuzzleControl theRules;
    AudioSource preheated;

    // Start is called before the first frame update
    void Start()
    {
        preheated = this.GetComponent<AudioSource>();
        preHeat = false;// resets preheat condition
        theRules = FindObjectOfType<CookingPuzzleControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!preHeat)
        {
            preheated.Play();
            preHeat = true;// pre-heating is finished
            theRules.cooking = true;// activates cooking bool
            theRules.TimerStart();
        }
    }
}
