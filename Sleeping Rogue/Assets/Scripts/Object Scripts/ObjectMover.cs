using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject current;
    public float roamSpeed;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        
        // Movement
        float walkTime = roamSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, current.transform.position, walkTime);
        if (transform.position.Equals(current.transform.position))
        {
            current = current.GetComponent<DirectionNode>().getNext();
        }

    }

    // If it runs into a wall or a floor, go to the next node
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            current = current.GetComponent<DirectionNode>().getNext();
        }
    }

    // Sets the player to be a child of the object so the player doesn't fly off
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(gameObject.transform);
        }
    }

    // Unchilds the player from the object
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            
            collision.collider.transform.SetParent(null);
        }
    }
}
