using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionChange : MonoBehaviour
{
    private PlatformerController Player;
    public PhysicsMaterial2D Floor;
    public PhysicsMaterial2D Slick;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.wall) {
            gameObject.GetComponent<Collider2D>().sharedMaterial = Slick;
        }
        else {
            gameObject.GetComponent<Collider2D>().sharedMaterial = Floor;
        }
    }
}
