using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;

    [HideInInspector] public bool wallJumpEnabled, wallJumping, jumping, jumpHeld = false;
    private Vector3 checkPointSave;
    public float moveForce;
    float maxSpeed;
    public float groundSpeed;
    public float airSpeed;
    public float JumpForce;
    public float DreamJumpForce;
    private float ReturnSpeed = 5;
    public Vector2 wallJumpForce;
    public Transform groundCheck1, groundCheck2;
    public Transform Right1, Right2;
    public Transform Left1, Left2;
    public GameObject shadow;
    public GameObject menuMain;
    public GameObject menu1;
    public GameObject menuOptions;



    public bool grounded = false;
    public bool wall, wallBlock = false;
    public bool runInto = false;

    [HideInInspector] public Rigidbody2D rb2d;
    private Collider2D playerCollider;

    LayerMask collidables;
    LayerMask flooring;
    LayerMask wallType;

    [HideInInspector] public int jumps = 0;
    [HideInInspector] public int maxJumps = 1;

    [HideInInspector] public float jumpTimer, wallJumpTimer, heightTime = 0.0f;

    public float maxFallSpeed = -2f;
    [HideInInspector] public float lowJumpMultiplier = 0.3f;
    [HideInInspector] public float dreamJumpMultiplier = 2f;

    [HideInInspector] public float horiz;
    [HideInInspector] public bool dreaming;
    [HideInInspector] public bool canDream;
    public bool canLadder;
    private bool climbing;
    [HideInInspector] public bool canMove;
    private bool movingToBody;

    [HideInInspector] public bool isMoving;
    float lastMove;
    [HideInInspector] public float showVert;

    float dragSpeed;

    [HideInInspector] public Collider2D lastHit;
    private float wallJumpVert = 600f;

    ParticleSystem jumpEffect;
    ParticleSystem wallEffect;
    public ParticleSystem returnEffect;

    public static bool paused;
    Vector2 velocHolder = Vector2.zero;

    private void Awake()
    {
        jumpEffect = GameObject.Find("Ground Effects").GetComponent<ParticleSystem>();
        wallEffect = GameObject.Find("Wall Effects").GetComponent<ParticleSystem>();
        rb2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
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
        wallJumpForce = new Vector2(650f, 600f);
        collidables = LayerMask.GetMask("Default", "Wall", "Ground");
        paused = false;
    }

    // Update is called once per frame
    void Update() {

        if (!paused)
        {
            wallJumpForce = new Vector2(650f, wallJumpVert);

            showVert = Input.GetAxis("Vertical");
            jumpTimer += Time.deltaTime;

            grounded = Physics2D.Linecast(groundCheck2.position, groundCheck1.position, flooring);
            if (!wallBlock) wall = Physics2D.Linecast(Right1.position, Right2.position, wallType);

            if (!Drag.boxDrag)
            {
                runInto = Physics2D.Linecast(Right1.position, Right2.position, collidables);
            }
            else runInto = Physics2D.Linecast(Left1.position, Left2.position, collidables);


            if (runInto && isMoving)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && grounded && canMove && !Drag.boxDrag)
            {
                //jumpTimer = 0;
                jumpEffect.transform.position = groundCheck1.position;
                jumpEffect.Play();
                jumpHeld = true;
                jumping = true;
                if (!dreaming) rb2d.AddForce(Vector2.up * JumpForce);
                if (dreaming) rb2d.AddForce(Vector2.up * DreamJumpForce);
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpHeld = false;
            }

            // Wall sliding is handled in this if statement

            if (wall && !grounded && !dreaming && !wallBlock && !jumpHeld)
            {
                wallJumpEnabled = true;
                lastMove = Input.GetAxisRaw("Horizontal");
                jumping = false;
                wallJumpTimer = 0;

                if (Input.GetButtonDown("Jump") && canMove)
                {
                    WallJump();
                }
                if (Input.GetAxis("Vertical") < -0.1f)
                {
                    wallBlock = true;
                }

                CapVelocity();
            }

             if (grounded)
            {
                wallJumping = false;
            }

            if (Input.GetAxisRaw("Vertical") != -1) wallBlock = false;

            if (Drag.boxTouch)
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

            if (Input.GetButtonDown("Dream") && canMove)
            {
                if (!dreaming)
                {
                    if (grounded) EnterExitDreaming();
                }
                else EnterExitDreaming();
            }
        }
	}

    private void FixedUpdate()
    {
        if (!paused)
        {

            if (!Input.GetButton("Drag"))
            {
                Drag.boxDrag = false;
            }
            if (!wallJumping && canMove)
            {
                horiz = Input.GetAxis("Horizontal");

                if (wallJumpEnabled)
                {
                    if (rb2d.velocity.y <= 0 && !wallBlock)
                    {
                        CapVelocity();
                    }

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
                    if (!isMoving && Mathf.Abs(Input.GetAxis("Horizontal")) > .1 && !runInto)
                    {
                        isMoving = true;
                        if (Mathf.Abs(rb2d.velocity.x) < maxSpeed)
                        {
                            rb2d.AddForce(Mathf.Sign(horiz) * (moveForce * 2f) * Vector2.right);
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(rb2d.velocity.x) > 0)
                    {
                        rb2d.AddForce(Vector2.left * horiz * moveForce);
                    }
                    else isMoving = false;
                }

                if (horiz * rb2d.velocity.x < maxSpeed && (Mathf.Abs(Input.GetAxis("Horizontal")) > .25f))
                {
                    if (!runInto)
                    {
                        rb2d.AddForce(Vector2.right * horiz * moveForce);
                    }
                }

                // Climbing up ladder
                if (canLadder)
                {
                    /*if (Input.GetAxis("Vertical") > .25) {
                        Debug.Log("Player is climbing ladder");
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        rb2d.gravityScale = 0;
                        rb2d.velocity = Vector2.up * 10;
                    }
                    else if (Input.GetAxis("Vertical") < -.25) {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        rb2d.gravityScale = 0;
                        rb2d.velocity = Vector2.down * 10;
                    }
                    else if (Input.GetAxisRaw("Vertical") == 0) {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    }*/
                    if (Mathf.Abs(Input.GetAxis("Vertical")) > .25)
                    {
                        rb2d.velocity = Vector2.up * 10 * Mathf.Sign(Input.GetAxis("Vertical"));
                        climbing = true;
                    }

                    if (grounded)
                    {
                        climbing = false;
                    }

                    if (climbing)
                    {
                        rb2d.gravityScale = 0;
                        if (!GetAxisDown("Vertical"))
                        {
                            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                        }
                    }
                    else rb2d.gravityScale = 1;

                }
            }



            if (!GetAxisDown("Horizontal")) horiz = 0;

            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            }

            if (horiz > 0 && !facingRight)
            {
                Flip();
            }
            else if (horiz < 0 && facingRight)
            {
                Flip();
            }

            if (jumping)
            {

                if (!dreaming && !jumpHeld && rb2d.velocity.y > 0)
                {
                    rb2d.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;

                }
                if (dreaming && !jumpHeld && rb2d.velocity.y > 0)
                {
                    rb2d.velocity += Vector2.up * Physics2D.gravity.y * dreamJumpMultiplier * Time.deltaTime;
                }

            }

            if (wallJumpEnabled && !wall) wallJumpEnabled = false;

            if (wallJumping)
            {
                if (!wall)
                {
                    if (Input.GetAxisRaw("Horizontal") != lastMove ||
                        Mathf.Abs(Input.GetAxis("Vertical")) >= .2f)
                    {
                        wallJumping = false;
                    }

                }
            }

            if (Input.GetButtonDown("Cancel") && canMove)
            {
                StartCoroutine(Respawn());
            }

            // Handles returning player to real body when exiting dream mode
            if (movingToBody)
            {
                GameObject ShadowInScene = GameObject.FindGameObjectWithTag("Shadow");
                transform.position = Vector2.MoveTowards(transform.position, ShadowInScene.transform.position, (ReturnSpeed * Time.deltaTime));
                ReturnSpeed += 60 * Time.deltaTime;
                returnEffect.transform.LookAt(ShadowInScene.transform.position);
                returnEffect.Play();
                if (transform.position == ShadowInScene.transform.position)
                {

                    returnEffect.Stop();
                    returnEffect.Clear();
                    movingToBody = false;
                    Destroy(ShadowInScene);
                    dreaming = false;
                    playerCollider.enabled = true;
                    rb2d.gravityScale = 1.0f;
                    rb2d.velocity = Vector3.zero;
                    canMove = true;
                    ReturnSpeed = 5;
                }
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }

    }

    private void EnterExitDreaming()
    {
        if (dreaming)
        {
            canMove = false;
            movingToBody = true;
            playerCollider.enabled = false;
            canDream = true;
        }
        else if (!dreaming && canDream == true)
        {
            dreaming = true;
            Instantiate(shadow, this.transform.position, Quaternion.identity);
            rb2d.gravityScale = 0.7f;
        }
    }

    public void Pause()
    {

        paused = !paused;
        if (paused)
        {
            menuMain.SetActive(true);
            menu1.SetActive(true);
            menuOptions.SetActive(false);
            velocHolder = rb2d.velocity;
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            menuMain.SetActive(false);
            menu1.SetActive(false);
            menuOptions.SetActive(false);
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.velocity = velocHolder;
        }
    }

    void WallJump()
    {
        wallJumpEnabled = false;
        wallJumping = true;

        wallEffect.transform.position = Right2.position;
        wallEffect.Play();

        rb2d.velocity = Vector2.zero;
        if (facingRight)
        {
            rb2d.AddForce(new Vector2(-wallJumpForce.x, wallJumpForce.y));
            horiz = -1;
        } else
        {

            rb2d.AddForce(new Vector2(wallJumpForce.x, wallJumpForce.y));
            horiz = 1;
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
        yield return new WaitForSeconds(0.75f);
        Image.FadeOut();
        this.transform.position = checkPointSave;
        yield return new WaitForSeconds(0.3f);
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
            if (!dreaming)
            {
                StartCoroutine(Respawn());
            }
            else if (dreaming)
            {
                EnterExitDreaming();
            }
        }


        //if (lasthit == collision.gameobject.getcomponent<boxcollider2d>())
        //{
        //    walljumpvert /= 1.1f;
        //}
        //else
        //{
        //    walljumpvert = 600;
        //}
        //lasthit = collision.gameobject.getcomponent<boxcollider2d>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            if (!dreaming) {
                checkPointSave = this.transform.position;
            }
        }
        if (collision.gameObject.tag == "Kill")
        {
            if (!dreaming)
            {
                StartCoroutine(Respawn());
            }
            else if (dreaming)
            {
                EnterExitDreaming();
            }
        }
        if (collision.gameObject.tag == "Ladder")
        {
            if (!dreaming)
            {
                canLadder = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inhibitor")
        {
            canDream = false;
            if (dreaming) {
                EnterExitDreaming();
            }
        }

        if(collision.gameObject.tag == "Ladder")
        {
            if (dreaming)
            {
                canLadder = false;
                rb2d.gravityScale = .7f;
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
            if (!dreaming)
            {
                canLadder = false;
                climbing = false;
                rb2d.gravityScale = 1.0f;
                if (rb2d.velocity.y > 0) rb2d.velocity = new Vector2(0, 0);
            }
            else if (dreaming)
            {
                rb2d.gravityScale = .7f;
            }
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
    private IEnumerator waitToPush()
    {
        yield return new WaitForSeconds(.5f);
    }

    //putting a limit on the wall fall speed for the player
    public void CapVelocity()
    {
        float cappedYVelocity = Mathf.Max(rb2d.velocity.y, maxFallSpeed);

        rb2d.velocity = new Vector2(0, cappedYVelocity);
        if (rb2d.velocity.y == cappedYVelocity)
        {
            Debug.Log("capping velocity");
        }
    }
}
