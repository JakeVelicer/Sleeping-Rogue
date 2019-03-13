using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceptor : MonoBehaviour
{
    public GameObject[] connected;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < connected.Length; i++)
        {
            if (connected[i] != null)
                Gizmos.DrawLine(transform.position, connected[i].transform.position);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            Debug.Log("hit receptor");
            foreach (GameObject i in connected)
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Kill")
        {
            Debug.Log("left receptor");
            foreach (GameObject i in connected)
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
