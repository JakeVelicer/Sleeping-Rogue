using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {


    public GameObject[] connected;
    public Sprite Normal;
    public Sprite Clicked;
    public Sprite NearNormal;
    public Sprite NearClicked;
    public bool touch = false;

    public AudioClip button;
    public AudioSource audioSource;

    public bool pressed;
    SpriteRenderer spr;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < connected.Length; i++)
        {
            if (connected[i] != null)
                Gizmos.DrawLine(transform.position, connected[i].transform.position);
        }

    }

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (touch)
            {
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
                audioSource.PlayOneShot(button);
                pressed = !pressed;
            }
        }

        if (pressed && !touch)
        {
            spr.sprite = Clicked;
        }
        else if (!pressed && !touch)
        {
            spr.sprite = Normal;
        }
        else if (pressed && touch)
        {
            spr.sprite = NearClicked;
        }
        else if (!pressed && touch)
        {
            spr.sprite = NearNormal;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            touch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            touch = false;
        }
    }
}
