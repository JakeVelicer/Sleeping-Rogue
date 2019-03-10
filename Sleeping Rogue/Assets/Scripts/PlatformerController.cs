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
    public GameObject menuOptions;


    public bool grounded = false;
    public bool wall, wallBlock = false;
    public bool runInto = false;
    RaycastHit2D runIntoHit;

    [HideInInspector] public Rigidbody2D rb2d;
    private Collider2D playerCollider;


    //Layermasks for the different objects players may touch.
    LayerMask collidables;
    LayerMask flooring;
    LayerMask wallType;

    //Variables controlling jumping heights and interactions
    [HideInInspector] public int jumps = 0;
    [HideInInspector] public int maxJumps = 1;
    [HideInInspector] public float jumpTimer, wallJumpTimer, heightTime = 0.0f;
    public float maxFallSpeed = -2f;
    [HideInInspector] public float lowJumpMultiplier = 0.3f;
    [HideInInspector] public float dreamJumpMultiplier = 2f;

    //Horiz determines players horizontal movement. booleans that control the dream state
    [HideInInspector] public float horiz;
    [HideInInspector] public bool dreaming;
    [HideInInspector] public bool canDream;

    //Determines if the player is climbing or can climb
    public bool canLadder;
    [HideInInspector] public bool climbing;


    [HideInInspector] public bool canMove;
    private bool movingToBody;
    private bool RoamRight;


    //Determines if the player is currently in motion
    [HideInInspector] public bool isMoving;

    //Checks for new input after wall jumping
    float lastMove;
    [HideInInspector] public float showVert;



    //Stuff used for Decaying Wall Jumps?
    [HideInInspector] public Collider2D lastHit;
    private float wallJumpVert = 600f;


    //Particles systems driven by the player
    ParticleSystem jumpEffect;
    ParticleSystem wallEffect;
    public ParticleSystem returnEffect;


    //Default variables for the Pausing system
    public static bool paused;
    Vector2 velocHolder = Vector2.zero;


    //Variables used for dragging boxes in the scene
    [HideInInspector] public bool canDrag = true;
    [HideInInspector] public bool dragging;
    [HideInInspector] public bool boxTouching;
    float dragSpeed;

    //AudioManager to play sounds
    private AudioManager audioManager;


    //Start and Awake defaults all variables that need to be defaulted still. Set up the scene for gameplay.
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

        menuMain = GameObject.Find("Main");
        menuOptions = GameObject.Find("Options");

        GameObject.Find("Button 0").GetComponent<Button>().onClick.AddListener(Pause);

        menuMain.SetActive(false);
        menuOptions.SetActive(false);
    }

    void Start()
    {
        checkPointSave = this.transform.position;
        canMove = true;
        dragSpeed = groundSpeed / 2;
        wallJumpForce = new Vector2(650f, 600f);
        collidables = LayerMask.GetMask("Default", "Wall", "Ground", "Box");
        paused = false;

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
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

            //Changes the side checked if you are dragging a box
            if (!dragging)
            {
                runInto = Physics2D.Linecast(Right1.position, Right2.position, collidables);
            }
            else
            {
                runInto = Physics2D.Linecast(Left1.position, Left2.position, collidables);
            }

            //Checks for if the player has run into a box, then turns on the needed variable if so.
            runIntoHit = Physics2D.Linecast(Right1.position, Right2.position, LayerMask.GetMask("Box"));

            if (runIntoHit)
            {
                boxTouching = true;
            }
            else boxTouching = false;


            if (runInto && isMoving)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }


            //Controls jumping heights
            if (Input.GetButtonDown("Jump") && grounded && canMove && !dragging)
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

            //Cuts movement speed while dragging a box
            if (dragging)
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

            //Enters or exits the dream state if capable
            if (Input.GetButtonDown("Dream") && canMove)
            {
                if (!dreaming)
                {
                    if (grounded) EnterExitDreaming();
                }
                else EnterExitDreaming();
            }

            //Checks conditions to allow dragging of boxes
            if (GetAxisDown("Drag") && canDrag && boxTouching && grounded)
            {
                canDrag = false;
                StartCoroutine(StartStopDrag(runIntoHit.collider.gameObject));
            }
        }
	}

    float movementTimer = 0.35f;

    private void FixedUpdate()
    {
        //Blocks most inputs if the game is paused
        if (!paused)
        {
            //If the player is not wall jumping, checks horizontal input and applies forces according to various conditions
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

                //Adds initial speed burst to player
                if (GetAxisDown("Horizontal"))
                {
                    if (!isMoving && Mathf.Abs(Input.GetAxis("Horizontal")) > .1f && !runInto)
                    {
                        isMoving = true;
                        /*if (Mathf.Abs(rb2d.velocity.x) < maxSpeed && grounded)
                        {
                            rb2d.AddForce(Mathf.Sign(horiz) * (moveForce * 1.5f) * Vector2.right);
                        }*/

                    }
                    if (movementTimer < 1.0f)
                    {
                        movementTimer += Time.deltaTime;
                    }
                }
                else
                {

                    movementTimer = 0.35f;
                    if (Mathf.Abs(rb2d.velocity.x) > 0)
                    {
                        if (!jumpHeld)
                        {
                            float xVal = rb2d.velocity.x;
                            if (grounded)
                            {
                                xVal *= .9f;
                            }
                            else
                            {
                                xVal *= .99f;
                            }
                            rb2d.velocity = new Vector2(xVal, rb2d.velocity.y);
                        }
                    }
                    else
                    {
                        isMoving = false;
                    }
                }

                if (Input.GetButtonDown("Jump"))
                {
                    movementTimer = .35f;
                }

                if (Mathf.Sign(horiz) * rb2d.velocity.x < maxSpeed && (Mathf.Abs(Input.GetAxis("Horizontal")) > .1f) && GetAxisDown("Horizontal"))
                {
                    if (!runInto)
                    {
                        if (!grounded)
                        {
                            rb2d.AddForce(Vector2.right * horiz * (moveForce / 2) * movementTimer);
                        }
                        else
                        {
                            rb2d.AddForce(Vector2.right * horiz * moveForce * movementTimer);
                        }
                    }
                }

                // Climbing up ladder
                if (canLadder)
                {
                    if (Mathf.Abs(Input.GetAxis("Vertical")) > .25)
                    {
                        rb2d.velocity = Vector2.up * 10 * Mathf.Sign(Input.GetAxis("Vertical"));
                        if (this.transform.parent != null)
                            gameObject.transform.position = new Vector3(transform.parent.position.x, transform.position.y, 0);
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

            //handles which direction the player faces
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

            //Blocks the input from user until new input is given
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
                ReturnSpeed += 40 * Time.deltaTime;
                if (RoamRight) {
                    rb2d.AddForce(new Vector2(150, 150));
                }
                else if (!RoamRight) {
                    rb2d.AddForce(new Vector2(-150, -150));
                }
                returnEffect.transform.LookAt(ShadowInScene.transform.position);
                returnEffect.Play();
                if (transform.position == ShadowInScene.transform.position)
                {
                    rb2d.velocity = Vector3.zero;
                    returnEffect.Stop();
                    returnEffect.Clear();
                    movingToBody = false;
                    Destroy(ShadowInScene);
                    dreaming = false;
                    playerCollider.enabled = true;
                    rb2d.gravityScale = 1.0f;
                    canMove = true;
                    ReturnSpeed = 5;
                }
            }
        }

        //pauses and unpauses the game
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }

    }

    private void Wave()
    {
        if (RoamRight == true) {
            RoamRight = false;
        }
        else if (RoamRight == false) {
            RoamRight = true;
        }
    }

    //Handles players entering and exiting the dreamworld
    private void EnterExitDreaming()
    {
        if (dreaming)
        {
            canMove = false;
            movingToBody = true;
            playerCollider.enabled = false;
            canDream = true;
            //CancelInvoke("Wave");
        }
        else if (!dreaming && canDream == true)
        {
            dreaming = true;
            InvokeRepeating("Wave",0,0.8f);
            Instantiate(shadow, this.transform.position, Quaternion.identity);
            rb2d.gravityScale = 0.8f;
        }
    }

    //Opens the pause menu and stores necessary variables
    public void Pause()
    {

        paused = !paused;
        if (paused)
        {
            menuMain.SetActive(true);
            menuOptions.SetActive(false);
            velocHolder = rb2d.velocity;
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            menuMain.SetActive(false);
            menuOptions.SetActive(false);
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.velocity = velocHolder;
        }
    }


    //Gives players the proper boost when jumping off a wall
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

    //Flips the player sprite depending on direction moving
    void Flip()
    {
        if (!dragging)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            movementTimer = .35f;
            transform.localScale = theScale;
        }
    }

    //Starts and Ends the dragging condition
    public IEnumerator StartStopDrag(GameObject box)
    {
        dragging = !dragging;

        Debug.Log(dragging);
        if (!dragging)
        {
            box.GetComponent<FixedJoint2D>().connectedBody = box.GetComponent<Rigidbody2D>();
            box.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (dragging)
        {
            box.GetComponent<FixedJoint2D>().connectedBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            box.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        yield return new WaitForSeconds(.2f);

        canDrag = true;
    }


    //Reloads the player if they die
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

        // Unchilds the player from the object
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" ||
        collision.gameObject.tag == "Kill" ||
        collision.gameObject.tag == "Wall")
        {
            this.transform.SetParent(null);
        }
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
            this.transform.SetParent(null);
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
