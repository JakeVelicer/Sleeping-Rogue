using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
	private Transform Target;
    private GameObject gameController;
    private Animator anim;
	private System.Action DestroyEnemySequence;

	// Number Elements
	public float EnemyHealth;
    public int enemyValue;
    public float MovementSpeed;
	public float ChaseRange;
	public float FireRange;
	public float ProjectileSpeed;
	public float ProjectileHeight;
	public float CoolDown;
	private float CoolDownTimer = 0;
	private int DashDirection;

	// Boolean Elements
	private bool CanRoam;
    private bool CanChase;
	public bool TouchStop;
	private bool Dead;
	private bool CanAttack = true;
	private bool CanFireRay = true;
	public bool ToTheRight;
	private bool CanSpawnIceBlock = true;

	// Attack Objects and Elements
	private PlatformerController Player;
	public GameObject BulletObject;
	public Material DefaultMaterial;
	public Material HotFlash;
	public GameObject[] OtherEnemies;
	private Collider2D AttackCollider;
	private Collider2D Collider;

	// The type of enemy this is
	public int AlienType;


    // Use this for initialization
    void Start () {


        // Assignment Calls
        anim = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
		Collider = gameObject.GetComponent<Collider2D> ();
		Player = GameObject.Find("Player").GetComponent<PlatformerController>();

		// Setting elements to their proper states
		InvokeRepeating ("Roam", 0, 1.5f);
		TouchStop = false;
		GetComponent<SpriteRenderer>().material = DefaultMaterial;
		DestroyEnemySequence += EnemyDeathSequence;
		
		
	}
	
	// Update is called once per frame
	void Update () {

		// Finds the Player's transform and stores it in target
		Target = GameObject.FindGameObjectWithTag ("Player").transform;

		ChaseTarget();
		//TrackOtherEnemies();

		if (EnemyHealth <= 0) {
			if (DestroyEnemySequence != null) {
				DestroyEnemySequence();
			}
		}

		if (transform.position.y <= -1.5 && AlienType != 5) {
			TouchStop = true;
		}
		else if (transform.position.y > -1.5 && AlienType != 5) {
			TouchStop = false;
		}

	}

	// Controls the actual movement of the Enemy
	void FixedUpdate() {

		// Checks if it is allowed to chase the player
		if (CanChase || CanRoam) {

			// Pushes the enemy in a direction based upon which side the player is on
			if (ToTheRight == false) {
     			if (TouchStop && CanAttack) {
					Vector2 myVel = Rigidbody.velocity;
                	myVel.x = -MovementSpeed;
					Rigidbody.velocity = myVel;
					anim.SetInteger("Near", 1);
				}
			}
			else if (ToTheRight == true) {
     			if (TouchStop && CanAttack) {
					Vector2 myVel = Rigidbody.velocity;
                	myVel.x = MovementSpeed;
					Rigidbody.velocity = myVel;
					anim.SetInteger("Near", 1);
				}
            }
		}
	}

	void ChaseTarget () {

		float Dist = Vector3.Distance(Target.position, transform.position);
		float DistX = Mathf.Abs(Target.position.x - transform.position.x);

		// Determines if the range of the player is close enough to be chased
		if (Dist <= ChaseRange && Dist > FireRange) {
			CanChase = true;
			CanRoam = false;
			ChaseDirection();
		}
		// Tells the player to attack if close enough
		else if (Dist <= FireRange && AlienType != 5) {
			CanChase = false;
			CanRoam = false;
			ChaseDirection();

			// This switch assigns the proper cooldown and attack phase for each enemy type.
			if (CanAttack) {
				if (TouchStop && !Dead) {
					switch (AlienType) {
						// Roly Poly Alien
						case 1:
							CanAttack = false;
							anim.SetTrigger("Attack");
							break;
				}
			}
		}
		// Roams out of range of chasing and attacking
		else {
			CanChase = false;
			CanRoam = true;
			CoolDownTimer = 0;
		}
	}
    }

	// Determines the direction the object faces when chasing
	void ChaseDirection () {

		if (CanRoam == false) {
			if (Target.position.x > transform.position.x + 0.5) {
				transform.localScale = new Vector3(-1, 1, 1);
				ToTheRight = true;
			}
			else if (Target.position.x < transform.position.x + 0.5) {
				transform.localScale = new Vector3(1, 1, 1);
				ToTheRight = false;
			}
		}
		else if (CanRoam == true) {
			if (ToTheRight == false) {
				transform.localScale = new Vector3(-1, 1, 1);
				ToTheRight = true;
			}
			else if (ToTheRight == true) {
				transform.localScale = new Vector3(1, 1, 1);
				ToTheRight = false;
			}
		}
	}

	void TrackOtherEnemies () {

		OtherEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		for (int i = 0; i < OtherEnemies.Length; i++) {

			Vector3 toTarget = (OtherEnemies[i].transform.position - transform.position);
			
			if (Vector3.Dot(toTarget, transform.right) < 0) {
				TouchStop = false;
			} else if (Vector3.Dot(toTarget, transform.right) > 0) {
				TouchStop = true;
			}

		}

	}

    // Instantiates a chosen projectile in the scene and propels it forward like a bullet
    private IEnumerator GunAttack () {

		Rigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.7f);
		if (AlienType == 4) {
		}
		if (ToTheRight == true)
		{
			GameObject Projectile = Instantiate(BulletObject, transform.position + new Vector3(1.0f, .10f, 0),
			Quaternion.identity) as GameObject;
			Projectile.GetComponent<Rigidbody2D>().AddForce(Vector3.right * ProjectileSpeed);
		}
		else if (ToTheRight == false)
		{
			GameObject Projectile = Instantiate (BulletObject, transform.position + new Vector3(-1.0f, .10f, 0), 
			Quaternion.identity) as GameObject;
			Projectile.GetComponent<Rigidbody2D>().AddForce(Vector3.left * ProjectileSpeed);
		}
		StartCoroutine(shootWait());
	}

	// Cooldown before allowed to attack again
    private IEnumerator shootWait()
	{
    	anim.SetInteger("Near", 2);
    	yield return new WaitForSeconds(CoolDown);
        CanAttack = true;
    }

	void EnemyDeathSequence () {

		Dead = true;
		DestroyEnemySequence = null;
		Rigidbody.velocity = Vector2.zero;
		GameObject.Find("Score").GetComponent<Animator>().SetTrigger("Bulge");
		anim.SetTrigger("Die");
		Destroy(gameObject, 0.5f);
	}

    void OnTriggerEnter2D(Collider2D collision) {

		if (collision.gameObject.name == "LightningBullet(Clone)") {


        }

	}

	private IEnumerator HitByAttack (int xSpeed, int ySpeed, float Seconds) {
		if (!Dead) {
			GetComponent<SpriteRenderer>().material = HotFlash;
			Rigidbody.AddForce(Vector3.up * ySpeed);
			if (Player.facingRight) {
				Rigidbody.AddForce(Vector3.right * xSpeed);
			}
			else if (!Player.facingRight) {
				Rigidbody.AddForce(Vector3.left * xSpeed);
			}
			yield return new WaitForSeconds(0.1f);
			GetComponent<SpriteRenderer>().material = DefaultMaterial;
			yield return new WaitForSeconds(Seconds);
		}
	}

	private void Roam () {
		if (CanRoam) {
			ChaseDirection();
		}
    }

}