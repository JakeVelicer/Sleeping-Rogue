using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private PlatformerController PlayerScript;
    public GameObject Player;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = Player.GetComponent<Animator>();
        PlayerScript = Player.GetComponent<PlatformerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerScript.isMoving)
        {
            anim.SetInteger("BasicAnimSwitch", 1);
        }
        else if (!PlayerScript.isMoving)
        {
            anim.SetInteger("BasicAnimSwitch", 0);
        }
        
    }
}
