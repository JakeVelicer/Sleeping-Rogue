using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private PlatformerController PlayerScript;
    private Animator anim;
    private Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerScript = GetComponent<PlatformerController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // Controls Idle and Running anim
        if (PlayerScript.isMoving && PlayerScript.grounded)
        {
            anim.SetInteger("BasicAnimSwitch", 1);
        }
        else if (!PlayerScript.isMoving && PlayerScript.grounded)
        {
            anim.SetInteger("BasicAnimSwitch", 0);
        }
        
        // Activates jumping anim and falling anim
        if (rb2d.velocity.y > 0 && PlayerScript.jumping == true &&
            PlayerScript.canMove && !PlayerScript.grounded && !Drag.boxDrag)
        {
            anim.Play("PlayerJumpIni");
        }
        if (rb2d.velocity.y > 0 && PlayerScript.wallJumping == true && 
            PlayerScript.canMove && !PlayerScript.grounded && !Drag.boxDrag)
        {
            anim.Play("PlayerJumpIni");
        }
        if (rb2d.velocity.y < 1)
        {
            anim.SetTrigger("Falling");
        }

        // Checks if player is grounded so it can switch back from jump
        if (PlayerScript.grounded)
        {
            anim.SetBool("AnimGrounded", true);
        }
        else if (!PlayerScript.grounded)
        {
            anim.SetBool("AnimGrounded", false);
        }
        //Debug.Log("Grounded?: " + PlayerScript.grounded);
        
    }
}
