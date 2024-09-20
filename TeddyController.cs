using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyController : MonoBehaviour
{
    HideAndSeek theRules;// reference to puzzle script
    public GameObject lookingGlass;
    bool spotted;

    // Start is called before the first frame update
    void Start()
    {
        spotted=false;
        theRules = FindObjectOfType<HideAndSeek>();
        lookingGlass = FindObjectOfType<LookingGlassController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(theRules.contestStarted && spotted && Input.GetButtonDown("Interact"))
        {
            theRules.GameWin();
            Destroy(this.gameObject);
        }
        if (theRules.contestStarted && Input.GetButtonDown("Interact") && !spotted)
        {
            theRules.GameLose();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator") 
        {
            spotted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator") 
        {
            spotted = false;
        }
    }
}
