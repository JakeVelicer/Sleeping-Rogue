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
    public GameObject shadow;


    bool grounded = false;
    public bool wall, wallBlock = false;
    public bool runInto = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private Collider2D playerCollider;

    LayerMask collidables;
    LayerMask flooring;
    LayerMask wallType;

    [HideInInspector] public int jumps = 0;
    [HideInInspector] public int maxJumps = 1;

    [HideInInspector] public float jumpTimer, wallJumpTimer, heightTime = 0.0f;

    public Color real, dream;
    [HideInInspector] public float horiz;
    [HideInInspector] public bool dreaming;
    public bool canDream;
    private bool canLadder;
    private bool climbing;
    private bool canMove;
    private bool movingToBody;

    public bool isMoving;
    float lastMove;
    public float showVert;

    float dragSpeed;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        horiz = 0;
        dreaming = false;
        isMoving = false;
        flooring = LayerMask.GetMask("Ground", "Box");
        wallType = LayerMask.GetMask("Ground");
    }

    void Start()
    {
        checkPointSave = this.transform.position;
        canMove = true;
        dragSpeed = groundSpeed / 2;
	}

    // Update is called once per frame
    void Update() {

        showVert = Input.GetAxis("Vertical");
        jumpTimer += Time.deltaTime;

        grounded = Physics2D.Linecast(groundCheck2.position, groundCheck1.position, flooring);
        if(!wallBlock) wall = Physics2D.Linecast(wallCheck2.position, wallCheck1.position, wallType);
        runInto = Physics2D.Linecast(wallCheck2.position, wallCheck1.position, collidables);

        

        if (dreaming)
        {
            Camera.main.backgroundColor = dream;

            collidables = LayerMask.GetMask("Default", "Wall", "Box", "Ground");
        }
        else
        {
            Camera.main.backgroundColor = real;
            collidables = LayerMask.GetMask("Default", "Wall", "Box");
        }

        if (Input.GetButtonDown("Jump") && grounded && canMove && !Drag.boxDrag)
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
        if (wall && !grounded && rb2d.velocity.y <= 0 && !dreaming && !wallBlock)
        {
            wallJumpEnabled = true;
            lastMove = Input.GetAxisRaw("Horizontal");
            rb2d.velocity = new Vector2(0, -1f);
            jumping = false;
            wallJumpTimer = 0;
            if (Input.GetButtonDown("Jump") && canMove)
            {
                WallJump();
            }
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                wallBlock = true;
            }
        }

        if (Input.GetAxisRaw("Vertical") != -1) wallBlock = false;

        if (Drag.boxDrag)
        {
            maxSpeed = dragSpeed;
        }
        else if (!wall && !grounded)
        {
            maxSpeed = airSpeed;
            jumps = 1;
        }
        else maxSpeed = groundSpeed;
        
        if (!canMove)
        {
            rb2d.velocity = Vector3.zero;
        }

        if (grounded || !wall) wallBlock = false;
        

        if(!grounded && runInto)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
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

            if (GetAxisDown("Horizontal"))
            {
                if (!isMoving && !climbing)
                {
                    isMoving = true;
                    if(Mathf.Abs(rb2d.velocity.x) < maxSpeed)
                    {
                        rb2d.AddForce(Mathf.Sign(horiz) * (moveForce * 5) * Vector2.right);
                    }
                }
            }
            if (!GetAxisDown("Horizontal"))
            {
                isMoving = false;
                if(Mathf.Abs(rb2d.velocity.x) > 0)
                {
                    rb2d.AddForce(Vector2.left * horiz * moveForce);
                }
            }

            if (horiz * rb2d.velocity.x < maxSpeed && !climbing)
            {
                if (!grounded)
                {
                    if (!runInto)
                    {
                        rb2d.AddForce(Vector2.right * horiz * moveForce);
                    }
                }
                else rb2d.AddForce(Vector2.right * horiz * moveForce);
            }

            if (GetAxisDown("Vertical") && canLadder && !dreaming)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                if(Mathf.Abs(rb2d.velocity.y) < maxSpeed)
                {
                    rb2d.AddForce(Mathf.Sign(Input.GetAxisRaw("Vertical")) * (moveForce * 2) * Vector2.up);
                }
                climbing = true;
            }
            else if (!GetAxisDown("Vertical"))
            {
                climbing = false;
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
                if(Input.GetAxisRaw("Horizontal") != lastMove)
                {
                    wallJumping = false;
                }
            }
        }
        
        /*if(wallJumpTimer >= .25)
        {
            wallJumping = false;
        }*/

        if (Input.GetButtonDown("Dream") && canMove)
        {
            if (!dreaming)
            {
                if(grounded) EnterExitDreaming();
            }
            else EnterExitDreaming();
        }

        if (Input.GetButtonDown("Cancel") && canMove)
        {
            StartCoroutine(Respawn());
        }

        // Handles returning player to real body when exiting dream mode
        if (movingToBody)
        {
            GameObject ShadowInScene = GameObject.FindGameObjectWithTag("Shadow");
            transform.position = Vector2.MoveTowards(transform.position, ShadowInScene.transform.position, (30 * Time.deltaTime));
            if (transform.position == ShadowInScene.transform.position)
            {
                movingToBody = false;
                Destroy(ShadowInScene);
                dreaming = false;
                playerCollider.enabled = true;
                rb2d.gravityScale = 1.0f;
                rb2d.velocity = Vector3.zero;
                canMove = true;
            }
        }

    }

    private void EnterExitDreaming()
    {
        if (dreaming)
        {
            canMove = false;
            movingToBody = true;
            playerCollider.enabled = false;
        }
        else if (!dreaming && canDream == true)
        {
            dreaming = true;
            Instantiate(shadow, this.transform.position, Quaternion.identity);
            rb2d.gravityScale = 0.5f;
        }
    }

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
        if (!Drag.boxDrag)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private IEnumerator Respawn()
    {
        var Image = GameObject.Find("DeathFade").GetComponent<DeathFade>();
        Image.FadeIn();
        canMove = false;
        yield return new WaitForSeconds(2);
        Image.FadeOut();
        this.transform.position = checkPointSave;
        yield return new WaitForSeconds(0.5f);
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
        if (collision.gameObject.tag == "Ladder")
        {
            canLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inhibitor")
        {
            if (dreaming) {
                EnterExitDreaming();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inhibitor")
        {
            canDream = true;
        }
        if (collision.gameObject.tag == "Ladder")
        {
            canLadder = false;
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
