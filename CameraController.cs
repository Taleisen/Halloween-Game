using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;// target the camera follows
    Vector3 targetPosition;// position of the target the camera follows
    public float moveSpeed;// speed of the camera movement
    GameObject player;// reference to the player

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;// identifies the player game object
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);// gathers position of the target
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);// moves camera toward the target
    }

    public void FollowPlayer()// sets player as the camera's follow target
    {
        followTarget = player;// sets the player as the follow target
    }
}
