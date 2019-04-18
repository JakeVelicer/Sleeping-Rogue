using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPeek : MonoBehaviour
{

    private Transform Player;
    private Rigidbody2D Rigidbody;
    private float horizontalInput;
    private float verticalInput;
    private float Distance;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 50;
        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {

        horizontalInput = Input.GetAxisRaw("RightStickHor");
        verticalInput = Input.GetAxisRaw("LeftStickVert");

        Distance = Vector3.Distance(Player.position, transform.position);
        
        speed = Mathf.Clamp (Distance, 10f, 0f);

        if (Vector3.Distance(Player.position, transform.position) >= 10 ) {
            //transform.position = new Vector2((Mathf.Abs(parent.position.x - transform.position.x) + 1), (Mathf.Abs(parent.position.y - transform.position.y) + 1));
        }

        Vector3 movement = new Vector3 (horizontalInput, verticalInput, 0);
        Rigidbody.velocity = movement * speed;

        if (Mathf.Abs(horizontalInput) <= 0.5 && Mathf.Abs(verticalInput) <= 0.5) {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (200 * Time.deltaTime));
        }
    }

}
