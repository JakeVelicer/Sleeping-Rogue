using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxNoise : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip sliding;

    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.magnitude > 0)
        {
           audioSource.PlayOneShot(sliding, 1f);
        }


    }
}
