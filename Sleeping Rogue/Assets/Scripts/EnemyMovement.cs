using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public GameObject current;

    public static GameObject player;

    public bool spotted;

    public int speed;

	// Use this for initialization
	void Start () {
        spotted = false;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 look;

        float walkTime = speed * Time.deltaTime;

        if (!spotted)
        {

            look = new Vector2(current.transform.position.z, current.transform.position.y);
            
            transform.position = Vector2.MoveTowards(transform.position, current.transform.position, walkTime);

            transform.right = -(current.transform.position - transform.position);
            if (transform.position.Equals(current.transform.position))
            {
                current = current.GetComponent<DirectionNode>().getNext();
            }
        }
        else
        {
            look = new Vector2(player.transform.position.z, player.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, walkTime);
            transform.right = -(player.transform.position - transform.position);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            current = current.GetComponent<DirectionNode>().getNext();
        }
    }

}
