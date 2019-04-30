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

        if (!PlayerScript.Freeze) {

            // Controls Idle and Running anim
            if (PlayerScript.isMoving && PlayerScript.grounded)
            {
                anim.SetInteger("BasicAnimSwitch", 1);
            }
            else if (!PlayerScript.isMoving && PlayerScript.grounded)
            {
                anim.SetInteger("BasicAnimSwitch", 0);
            }
            
            // Activates jumping anim, falling anim, and slide fall anim
            if (rb2d.velocity.y > 0 && PlayerScript.jumping == true
            && PlayerScript.canMove && !PlayerScript.grounded && !PlayerScript.dragging && !PlayerScript.climbing)
            {
                anim.Play("PlayerJumpIni");
            }
            if (rb2d.velocity.y > 0 && PlayerScript.wallJumping == true
            && PlayerScript.canMove && !PlayerScript.grounded && !PlayerScript.dragging)
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
                && FallingTimer <= 0 && !PlayerScript.grounded && !PlayerScript.climbing && !PlayerScript.wall)
                {
                    anim.Play("PlayerJumpFall");
                }
                else if (PlayerScript.wall && !PlayerScript.grounded && !PlayerScript.dreaming)
                {
                    anim.Play("Wallslide");
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

            // Activates and controls climbing on ladder animation
            if (PlayerScript.climbing == true && PlayerScript.canMove && !PlayerScript.grounded && !PlayerScript.dragging)
            {
                anim.Play("PlayerClimb");
            }
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.7f && PlayerScript.climbing == true)
            {
                anim.speed = 1;
            }
            else if (Input.GetAxis("Vertical") < 0.6f && PlayerScript.climbing == true)
            {
                anim.speed = 0;
            }
            
            // Controls animation for dragging boxes
            if (PlayerScript.dragging && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerPush"))
            {
                anim.Play("PlayerPush");
            }
            else if (!PlayerScript.dragging && anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerPush"))
            {
                anim.SetTrigger("LeaveDrag");
            }
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > .1f && PlayerScript.dragging)
            {
                anim.speed = 1;
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0 && PlayerScript.dragging)
            {
                anim.speed = 0;
            }

            // Sets the anim speed back to 1 if needed
            if (!PlayerScript.climbing && !PlayerScript.dragging)
            {
                anim.speed = 1;
            }

        }
        else if (PlayerScript.Freeze && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerCrawl")) {
            anim.Play("PlayerCrawl");
        }

    }
}
