using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSleep : MonoBehaviour {

    public GameObject hidingspot;

    public float count;

    public bool touching;

    private void Start()
    {
        hidingspot = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerMovement.dream)
            {
                PlayerMovement.dream = false;

                transform.position = hidingspot.transform.position;

                hidingspot = null;

                GetComponent<SpriteRenderer>().color = Color.black;

                count = 0;
            }
            if (!PlayerMovement.dream && touching)
            {

                PlayerMovement.dream = true;

               

                GetComponent<SpriteRenderer>().color = Color.gray;

                count = 0;
            }
        }

        if (PlayerMovement.dream)
        {
            count += 4*Time.deltaTime;
        }
        
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Spot")
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            if (!PlayerMovement.dream)
    //            {

    //                PlayerMovement.dream = true;

    //                hidingspot = collision.gameObject;

    //                GetComponent<SpriteRenderer>().color = Color.gray;

    //                count = 0;
    //            }
    //        }
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spot")
        {
            touching = true;
            hidingspot = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spot")
        {
            touching = false;
            if (!PlayerMovement.dream)
            {
                hidingspot = null;
            }
        }
    }
}
