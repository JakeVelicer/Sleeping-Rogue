using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public GameObject current;
    public static GameObject player;
    public bool spotted;
    public int roamSpeed;
    public int trackSpeed;
    private int speed;

	// Use this for initialization
	void Start () {

        spotted = false;
        player = GameObject.FindGameObjectWithTag("Player");
        speed = roamSpeed;

	}
	
	// Update is called once per frame
	void Update () {
        
        //Vector2 look;
        float walkTime = speed * Time.deltaTime;

        if (spotted && player.GetComponent<PlatformerController>().dreaming == false)
        {
            //look = new Vector2(player.transform.position.z, player.transform.position.y);
            transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
            speed = trackSpeed;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, walkTime);
            transform.right = -(player.transform.position - transform.position);
        }
        else if (!spotted || (spotted && player.GetComponent<PlatformerController>().dreaming == true))
        {
            //look = new Vector2(current.transform.position.z, current.transform.position.y);
            speed = roamSpeed;
            transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
            transform.position = Vector2.MoveTowards(transform.position, current.transform.position, walkTime);
            transform.right = -(current.transform.position - transform.position);
            if (transform.position.Equals(current.transform.position))
            {
                current = current.GetComponent<DirectionNode>().getNext();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            current = current.GetComponent<DirectionNode>().getNext();
        }
    }

}
