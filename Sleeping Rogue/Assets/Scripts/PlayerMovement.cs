using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {


    public float timer = 0.0f;
    public static Vector3 check;
    public int jumps = 0;
    int maxJumps = 1;


    public static bool dream;

    public Vector2 power;

    private bool left = false;

    Vector2 direction;
    
    // Use this for initialization
    void Start () {
        direction = new Vector2(0, 0);
        check = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (dream)
        {
            GetComponent<Rigidbody2D>().gravityScale = .8f;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 1.25f;
        }
           

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(dream);
        }


        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(6, GetComponent<Rigidbody2D>().velocity.y);
        }	
        if(Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-6, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumps < maxJumps)
            {
                timer = 0.0f;
                jumps++;

                if (direction.x > 0 && direction.y > 0)
                {
                    power = new Vector2(.25f, 1);
                }
                else if (direction.x < 0 && direction.y < 0)
                {
                    power = new Vector2(-.25f, 1);
                }
                else
                {
                    power = new Vector2(0, 1);
                }
            }

        }



        if (Input.GetKeyUp(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            direction = new Vector2(0, 0);
        }
        turn();
    }

    private void FixedUpdate()
    {
        Debug.Log(direction);
        if (Input.GetKey(KeyCode.Space))
        {
                timer += Time.deltaTime;
                if (timer <= .5f)
                {
                    GetComponent<Rigidbody2D>().AddForce(power * (55 - (50 * timer)));
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        direction = collision.GetContact(1).point - new Vector2(transform.position.x, transform.position.y);

        direction = -direction.normalized;
        if (collision.gameObject.tag == "Floor")
        {
            if(direction.x < 0 && direction.y > 0)
            {
                jumps = 0;
            }

        }
        if (!dream)
        {
            if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "Wall")
            {
                jumps = 0;
            }
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            jumps = 1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (direction.x < 0 && direction.y > 0)
            {
                jumps = 0;
            }
        }

        if (collision.gameObject.tag == "Wall")
        {
            if (!dream)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                if (Input.GetKey(KeyCode.W))
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, -3);
                }


                direction = collision.GetContact(1).point - new Vector2(transform.position.x, transform.position.y);

                direction = -direction.normalized;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            check = this.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            if (dream == false)
            {
                this.transform.position = check;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
    }
    
    void turn()
    {
        if ( Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            left = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            left = false;
        }

    }
}
