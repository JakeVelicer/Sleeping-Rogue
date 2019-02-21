using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private PlatformerController PlayerScript;
    private Animator anim;
    private Rigidbody2D rb2d;
    private float FallingTimer;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerScript = GetComponent<PlatformerController>();
        rb2d = GetComponent<Rigidbody2D>();
        FallingTimer = 0.1f;
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
        if (rb2d.velocity.y > 0 && PlayerScript.jumping == true
        && PlayerScript.canMove && !PlayerScript.grounded && !Drag.boxDrag)
        {
            anim.Play("PlayerJumpIni");
        }
        if (rb2d.velocity.y > 0 && PlayerScript.wallJumping == true
        && PlayerScript.canMove && !PlayerScript.grounded && !Drag.boxDrag)
        {
            anim.Play("PlayerJumpIni");
        }
        if (rb2d.velocity.y < 1)
        {
            FallingTimer -= Time.deltaTime;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpIni"))
            {
                anim.SetTrigger("Falling");
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpIni")
            && FallingTimer <= 0 && !PlayerScript.grounded)
            {
                anim.Play("PlayerJumpFall");
            }
        }

        // Checks if player is grounded so it can switch back from jump anims
        if (PlayerScript.grounded)
        {
            anim.SetBool("AnimGrounded", true);
            FallingTimer = 0.1f;
        }
        else if (!PlayerScript.grounded)
        {
            anim.SetBool("AnimGrounded", false);
        }
        //Debug.Log("Grounded?: " + PlayerScript.grounded);
        //Debug.Log("FallingTimer: " + FallingTimer);
    }
}
