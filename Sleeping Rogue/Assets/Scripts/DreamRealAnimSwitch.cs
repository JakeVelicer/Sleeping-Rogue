using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamRealAnimSwitch : MonoBehaviour
{
    private Animator anim;
    private PlatformerController playerScript;
    private bool dreamActivated;
    private bool realActivated;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerScript.dreaming && !dreamActivated) {
            realActivated = false;
            dreamActivated = true;
            anim.Play("DreamWall");
        }
        else if (!playerScript.dreaming && !realActivated) {
            dreamActivated = false;
            realActivated = true;
            anim.Play("RealWall");
        }
    }
}
