using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScroller : MonoBehaviour
{
    public DiningPuzzleController theContest;// reference to Dance Contest script to check if the contest is active
    float moveSpeed = 1.5f;// how fast the notes scroll
    AudioSource eat;
    HungryHarryController harry;

    // Start is called before the first frame update
    void Start()
    {
        harry = FindObjectOfType<HungryHarryController>();
        eat = harry.GetComponent<AudioSource>();
        theContest = FindObjectOfType<DiningPuzzleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theContest.contestStarted)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime, transform.position.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            eat.Play();
            theContest.score++;
            theContest.promptNumber++;
            Destroy(this.gameObject);
        }
        if (collision.tag == "Destroyer")
        {
            theContest.promptNumber++;
            Destroy(this.gameObject);// cleans up the prompts
        }
    }
}
