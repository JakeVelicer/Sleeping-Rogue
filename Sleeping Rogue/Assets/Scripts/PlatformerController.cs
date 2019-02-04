using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    bool wallJumpEnabled, wallJumping = false;
    private Vector3 checkPointSave;
    public float moveForce;
    float maxSpeed;
    public float groundSpeed;
    public float airSpeed;
    public float jumpForce;
    public float longJumpForce;
    public Transform groundCheck1, groundCheck2;
    public Transform wallCheck1, wallCheck2;

    bool grounded = false;
    bool wall = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    private int jumps = 0;
    public int maxJumps = 1;

    public float jumpTimer, wallJumpTimer, heightTime = 0.0f;

    public Color real, dream;
    public float horiz;
    public bool dreaming;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        horiz = 0;
        dreaming = false;
    }

    void Start()
    {
        checkPointSave = this.transform.position;
	}

    // Update is called once per frame
    void Update() {

        jumpTimer += Time.deltaTime;

        grounded = Physics2D.Linecast(groundCheck2.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
        wall = Physics2D.Linecast(wallCheck2.position, wallCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

        if (dreaming)
        {
            Camera.main.backgroundColor = dream;
            rb2d.gravityScale = 0.5f;
        }
        else
        {
            Camera.main.backgroundColor = real;
            rb2d.gravityScale = 1.0f;

        }
        if (Input.GetButtonDown("Jump") && jumps < maxJumps)
        {
            jumpTimer = 0;
            heightTime = 0;
            jumps++;
        }

        if (Input.GetButton("Jump") && grounded)
        {
            heightTime += Time.deltaTime;
        }

        if(Input.GetButtonUp("Jump") && heightTime < 0.2f && heightTime > 0)
        {
            Jump(jumpForce);
        }

        if(heightTime > 0.2f)
        {
            Jump(longJumpForce);
        }

        if (grounded)
        {
            if (jumpTimer > .1)
            {
                jumps = 0;
            }
        }
        else if (wall && !grounded && rb2d.velocity.y <= 0 && !dreaming)
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
        if (!wallJumping)
        {
            horiz = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(horiz));

            if (wallJumpEnabled)
            {
                if (facingRight)
                {
                    if (horiz > 0)
                    {
                        horiz = 0;
                    }
                }
                if (!facingRight)
                {
                    if (horiz < 0)
                    {
                        horiz = 0;
                    }
                }
            }

            if (horiz * rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(Vector2.right * horiz * moveForce);
            }
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

        if (wallJumpEnabled && !wall) wallJumpEnabled = false;

        if (wallJumping)
        {
            horiz = 0;
            if (!wall)
            {
                wallJumpTimer += Time.deltaTime;
            }
        }
        
        if(wallJumpTimer >= .25)
        {
            wallJumping = false;
        }

        if (Input.GetButtonDown("Dream"))
        {
            dreaming = !dreaming;
        }

    }

    void Jump(float height)
    {
        anim.SetTrigger("Jump");
        heightTime = 0;
        rb2d.AddForce(new Vector2(0f, height));
    } 

    void WallJump()
    {
        anim.SetTrigger("Jump");
        wallJumpEnabled = false;
        wallJumping = true;
        if (facingRight)
        {
            rb2d.AddForce(new Vector2(-longJumpForce * 1.3f, longJumpForce * 1.2f));
        } else
        {

            rb2d.AddForce(new Vector2(longJumpForce * 1.3f, longJumpForce * 1.2f));
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

    private IEnumerator Respawn()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0); //Going to be used to display death animation
        this.transform.position = checkPointSave;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            jumpTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            checkPointSave = this.transform.position;
        }
        if (collision.gameObject.tag == "Kill")
        {
            if (dreaming == false)
            {
                StartCoroutine(Respawn());
            }
        }
    }

}
