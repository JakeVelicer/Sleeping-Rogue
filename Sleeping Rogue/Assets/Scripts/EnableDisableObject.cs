using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableObject : MonoBehaviour
{
    public GameObject[] connected;
    private bool Triggered = false;

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Player") {
            if (!Triggered) {
                Triggered = true;
                foreach(GameObject i in connected)
                {
                    if (i.activeSelf == true)
                    {
                        i.SetActive(false);
                    }
                    else
                    {
                        i.SetActive(true);
                    }
                }
            }
        }
    }
}
