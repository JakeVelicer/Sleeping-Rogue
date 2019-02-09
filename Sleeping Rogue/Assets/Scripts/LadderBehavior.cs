using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBehavior : MonoBehaviour
{
    public Collider2D Collider;
    public LayerMask PlayerLayer;


    // Start is called before the first frame update
    void Start()
    {
        Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 5, PlayerLayer);
        if (hitInfo.collider != null)
        {
            Collider.enabled = true;
        }
        else
        {
            Collider.enabled = false;
        }
    }
}
