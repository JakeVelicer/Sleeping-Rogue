using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    public bool wallJumpEnabled = false;
    public float moveForce;
    float maxSpeed;
    public float groundSpeed;
    public float airSpeed;
    public float jumpForce;
    public Transform groundCheck1, groundCheck2;
    public Transform wallCheck1;

    public bool grounded = false;
    public bool wall = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    public int jumps = 0;
    private int maxJumps = 1;

    public float jumpTimer, wallJumpTimer = 0.0f;
    public static Vector3 check;

    float horiz;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        horiz = 0;
    }

    // Update is called once per frame
    void Update () {

        jumpTimer += Time.deltaTime;

        grounded = Physics2D.Linecast(groundCheck2.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
        wall = Physics2D.Linecast(transform.position, wallCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

        

        if(Input.GetButtonDown("Jump") && jumps < maxJumps)
        {
            jumpTimer = 0;
            Jump();
            jumps++;
        }

        if (grounded)
        {
            if (jumpTimer > .1)
            {
                jumps = 0;
            }
        }
        else if (wall && !grounded && rb2d.velocity.y <= 0)
        {
            wallJumpEnabled = true;
            rb2d.velocity = new Vector2(0, -1f);
            wallJumpTimer = 0;
            if (Input.GetButtonDown("Jump"))
            {
                WallJump();
            }
        }

        if (!wall && !grounded)
        {
            maxSpeed = airSpeed;
            jumps = 1;
        }
        else maxSpeed = groundSpeed;
        
	}

    private void FixedUpdate()
    {
       
            horiz = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(horiz));
       
            if (horiz * rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(Vector2.right * horiz * moveForce);
            }
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            }
        if(horiz > 0 && !facingRight)
        {
            Flip();
        }
        else if (horiz < 0 && facingRight)
        {
            Flip();
        }
        

        if (wallJumpEnabled)
        {
            horiz = 0;
            if (!wall)
            {
                wallJumpTimer += Time.deltaTime;
            }
        }
        
        if(wallJumpTimer >= .25)
        {
            wallJumpEnabled = false;
        }
        
    }

    void Jump()
    {
        anim.SetTrigger("Jump");
        rb2d.AddForce(new Vector2(0f, jumpForce));
    } 

    void WallJump()
    {
        anim.SetTrigger("Jump");
        if (facingRight)
        {
            rb2d.AddForce(new Vector2(-jumpForce * 1.3f, jumpForce * 1.2f));
        } else
        {

            rb2d.AddForce(new Vector2(jumpForce * 1.3f, jumpForce * 1.2f));
        }
        Flip();
    }
    

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            jumpTimer = 0;
        }
    }
}
