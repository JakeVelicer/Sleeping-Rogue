using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;

    bool wallJumpEnabled, wallJumping, jumping, jumpHeld = false;
    private Vector3 checkPointSave;
    public float moveForce;
    float maxSpeed;
    public float groundSpeed;
    public float airSpeed;
    public float JumpForce;
    public Vector2 wallJumpForce;
    public Transform groundCheck1, groundCheck2;
    public Transform wallCheck1, wallCheck2;

    bool grounded = false;
    bool wall = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    [HideInInspector] public int jumps = 0;
    [HideInInspector] public int maxJumps = 1;

    [HideInInspector] public float jumpTimer, wallJumpTimer, heightTime = 0.0f;

    public Color real, dream;
    [HideInInspector] public float horiz;
    [HideInInspector] public bool dreaming;
    public bool canDream;
    private bool canMove;

    GameObject shadow;
    Vector3 shadowPos;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        horiz = 0;
        dreaming = false;
        shadow = GameObject.Find("Shadow");
        shadowPos = shadow.transform.position;
    }

    void Start()
    {
        checkPointSave = this.transform.position;
        canMove = true;
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
            shadow.transform.position = shadowPos;
        }
        else
        {
            Camera.main.backgroundColor = real;
            rb2d.gravityScale = 1.0f;
            shadow.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }
        if (Input.GetButtonDown("Jump") && grounded && canMove)
        {
            //jumpTimer = 0;
            jumpHeld = true;
            jumping = true;
            rb2d.AddForce(Vector2.up * JumpForce);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpHeld = false;
        }
        //if (grounded)
        //{
        //    if (jumpTimer > .1)
        //    {
        //        jumps = 0;
        //    }
        //}
        else if (wall && !grounded && rb2d.velocity.y <= 0 && !dreaming)
        {
            wallJumpEnabled = true;
            rb2d.velocity = new Vector2(0, -1f);
            jumping = false;
            wallJumpTimer = 0;
            if (Input.GetButtonDown("Jump") && canMove)
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
        
        if (!canMove)
        {
            rb2d.velocity = Vector3.zero;
        }
	}

    private void FixedUpdate()
    {
        if (!wallJumping && canMove)
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

        if (jumping)
        {
            if(!jumpHeld && rb2d.velocity.y > 0)
            {
                //rb2d.AddForce(Vector2.down * (JumpForce-350));
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
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

        if (Input.GetButtonDown("Dream") && canDream == true && canMove)
        {
            if (dreaming)
            {
                this.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, 0);
                rb2d.velocity = Vector3.zero;
            }
            if (!dreaming)
            {
                shadowPos = shadow.transform.position;
            }
            dreaming = !dreaming;
        }

        if (Input.GetButtonDown("Cancel") && canMove)
        {
            StartCoroutine(Respawn());
        }
    }

    //void Jump()
    //{
    //    anim.SetTrigger("Jump");
    //    jumps++;
    //    rb2d.AddForce(new Vector2(0f, jumpHeight));
    //} 

    void WallJump()
    {
        anim.SetTrigger("Jump");
        wallJumpEnabled = false;
        wallJumping = true;
        if (facingRight)
        {
            rb2d.AddForce(new Vector2(-wallJumpForce.x, wallJumpForce.y));
        } else
        {

            rb2d.AddForce(new Vector2(wallJumpForce.x, wallJumpForce.y));
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
        canMove = false;
        this.transform.position = checkPointSave;
        yield return new WaitForSeconds(4); //Going to be used to display death animation
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            jumpTimer = 0;
        }
        if (collision.gameObject.tag == "Kill")
        {
            if (dreaming == false)
            {
                StartCoroutine(Respawn());
            }
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inhibitor")
        {
            canDream = false;
            if (dreaming) {
                dreaming = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inhibitor")
        {
            canDream = true;
        }
    }

    //Used the check if the player is holding a movement method
    public bool GetAxisDown(string name)
    {
        if (Input.GetAxisRaw(name) == 0)
        {
            return false;
        }
        else return true;
    }
}
