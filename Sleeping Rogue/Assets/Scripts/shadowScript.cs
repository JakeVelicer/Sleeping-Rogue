using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowScript : MonoBehaviour {

    public GameObject player;
    bool dream;
    bool left = false;
    int count;

	// Use this for initialization
	void Start () {
        dream = PlayerMovement.dream;
        count = 2;


        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        count++;
        dream = PlayerMovement.dream;
        if (Input.GetKeyDown("e"))
        {
            count = 0;
        }
        if (!dream && count > 2)
        {
            this.transform.position = player.transform.position;
        }
        turn();

        
    }
    void turn()
    {
        if (Input.GetKey(KeyCode.A))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
             if (PlayerMovement.dream)
             {
                  PlayerMovement.dream = false;
             }

             player.transform.position = PlayerMovement.check;
             GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
