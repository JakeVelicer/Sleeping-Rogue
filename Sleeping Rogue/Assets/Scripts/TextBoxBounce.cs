using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxBounce : MonoBehaviour
{
    private bool up;
    private float timeToStart;

    void Start()
    {
        timeToStart = Random.Range(0.1f, 0.9f);
        InvokeRepeating("Direction", timeToStart, 2);
    }

    void Update()
    {
        if (up) {
            transform.Translate(Vector3.up * 0.002f, Space.World);
        }
        else if (!up) {
            transform.Translate(Vector3.down * 0.002f, Space.World);
        }
    }

    void Direction()
    {
        if (up) {
            up = false;
        }
        else if (!up) {
            up = true;
        }
    }
}
