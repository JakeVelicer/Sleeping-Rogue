using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoverZoomedOut : MonoBehaviour
{
    private CinemachineVirtualCamera CameraMachine;
    private CameraBehavior Camera;
    private Rigidbody2D Rigidbody;
    private GameObject Player;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        CameraMachine = GameObject.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>();
        Camera = GameObject.Find("CM vcam1").gameObject.GetComponent<CameraBehavior>();
        Player = GameObject.FindGameObjectWithTag("PlayerFollower");
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {

        if (!Camera.Switching) {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0);

            Rigidbody.velocity = movement * speed;
        }
        else if (Camera.Switching) {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (200 * Time.deltaTime));
        }
    }
}
