using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVScript : MonoBehaviour {

    private PlatformerController player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && player.dreaming == false)
        {
            GetComponentInParent<EnemyMovement>().spotted = true;
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    GetComponentInParent<EnemyMovement>().spotted = false;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GetComponentInParent<EnemyMovement>().spotted = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && player.dreaming == false)
        {
            GetComponentInParent<EnemyMovement>().spotted = true;
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    GetComponentInParent<EnemyMovement>().spotted = false;
            //}
        }
    }
}
