using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    public GameObject forward1, forward2;

    public GameObject player;

    public static bool boxDrag;
    public RaycastHit2D boxTouch;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checks = GameObject.FindGameObjectsWithTag("WallCheck");

        forward1 = checks[0];
        forward2 = checks[1];

        player = GameObject.Find("Player");
       this.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        boxTouch = Physics2D.Linecast(forward1.transform.position, forward2.transform.position, LayerMask.GetMask("Box"));
       

        if (boxTouch && player.GetComponent<PlatformerController>().grounded && Input.GetButtonDown("Drag"))
        {
            boxDrag = true;
            boxTouch.collider.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            boxTouch.collider.gameObject.GetComponent<FixedJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }
        if (Input.GetButtonUp("Drag"))
        {
            boxDrag = false;
            boxTouch.collider.gameObject.GetComponent<FixedJoint2D>().connectedBody = boxTouch.collider.gameObject.GetComponent<Rigidbody2D>();
            boxTouch.collider.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
       
    }

}
