using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    public GameObject forward1, forward2;

    public GameObject player;

    public static bool boxDrag;
    public bool boxTouch;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("WallCheck");

        forward1 = checks[0];
        forward2 = checks[1];

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        boxTouch = Physics2D.Linecast(forward1.transform.position, forward2.transform.position, LayerMask.GetMask("Box"));



        if(boxTouch && Input.GetButtonDown("Drag"))
        {
            boxDrag = true;
        }
        if (Input.GetButtonUp("Drag"))
        {
            boxDrag = false;
        }

       
    }
    private void FixedUpdate()
    {
        if (boxDrag)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<FixedJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
