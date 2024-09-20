using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScroller : MonoBehaviour
{
    public DanceContest theContest;// reference to Dance Contest script to check if the contest is active
    float moveSpeed = 1.5f;// how fast the notes scroll

    // Start is called before the first frame update
    void Start()
    {
        theContest = FindObjectOfType<DanceContest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theContest.contestStarted)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime, transform.position.z);
        }
    }
}
