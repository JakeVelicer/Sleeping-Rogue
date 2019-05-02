using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayerPeek : MonoBehaviour
{

    private Transform Player;
    private Rigidbody2D Rigidbody;
    public CinemachineVirtualCamera CameraController;
    private CinemachineFramingTransposer FramingTransposer;
    private float horizontalInput;
    private float verticalInput;
    private float Distance;
    private float speed = 50;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        FramingTransposer = CameraController.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update() {

        horizontalInput = Input.GetAxisRaw("RightStickHor");
        verticalInput = Input.GetAxisRaw("LeftStickVert");

        Vector3 movement = new Vector3 (horizontalInput, verticalInput, 0);
        
        Distance = Mathf.Abs(Vector3.Distance(Player.position, transform.position));
        Debug.Log("Input" + movement);

        if (Distance <= 8f) {
            Rigidbody.velocity = movement * speed;
        }
        else if (Distance > 7f) {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (100 * Time.fixedDeltaTime));
        }

        if (Mathf.Abs(horizontalInput) <= 0.5 && Mathf.Abs(verticalInput) <= 0.5) {
            transform.position = Player.transform.position;
        }

    }

}
