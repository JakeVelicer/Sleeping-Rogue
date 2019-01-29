using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractableObject {

	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}
