using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingDirections : MonoBehaviour
{
    public CookingContest theContest;
    public CookingPuzzleControl theRules;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (theContest.preHeat)
        {
            theRules.cooking = false;
            theRules.GameLose();
        }
    }
}
