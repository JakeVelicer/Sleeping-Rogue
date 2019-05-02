using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxNoise : MonoBehaviour
{
    public AudioSource audioSource;

   

    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.magnitude > 0)
        {
           audioSource.Play();
        }

        else if(rb.velocity.magnitude == 0)
        {
            audioSource.Stop();
        }
    }
}
