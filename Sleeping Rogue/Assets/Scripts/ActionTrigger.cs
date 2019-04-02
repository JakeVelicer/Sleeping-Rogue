using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    public GameObject[] connected;
    private bool Triggered = false;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Player") {
            if (!Triggered) {
                Triggered = true;
                foreach(GameObject i in connected)
                {
                    if (i.GetComponent<InteractableObject>().isActive)
                    {
                        i.GetComponent<InteractableObject>().isActive = false;
                    }
                    else
                    {
                        i.GetComponent<InteractableObject>().isActive = true;
                    }
                }
            }
        }
    }

}
