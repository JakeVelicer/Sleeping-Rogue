using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLasers : InteractableObject
{

    private Transform line;
    public Transform hit;

    public float width = .1f;
    public float maxLength = 10.0f;
    public float length;

    public float drawSpeed = 5.0f;

    public AudioClip death;
    public AudioClip hum;

    public AudioSource audioSource;

    private PlatformerController playerScript;

    LayerMask collides;
    // Start is called before the first frame update
    void Start()
    {
        line = this.transform;
        //line.useWorldSpace = false;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
        length = 0;
        line.localScale = new Vector3( width, line.localScale.y, line.localScale.z);
    }

    RaycastHit2D ray;
    bool extending = false;
    // Update is called once per frame
    void Update()
    {

        ray = Physics2D.Raycast(transform.position, transform.up, length, collides);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawLine(transform.position, hit.position, Color.blue);
      

        if (!isActive)
        {
            line.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().enabled = false;
            length = 0.5f;
            laserPulse = false;
        }
        else
        {
            GetComponent<AudioSource>().enabled = true;
            line.GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            if (ray.collider != null)
            {
                hit.position = ray.point;
                length = hit.localPosition.y;
                extending = false;
            }
            if (length < maxLength && !extending && ray.collider == null)
            {
                StartCoroutine(drawLaser());
            }
            if (!laserPulse)
            {

                StartCoroutine(pulsing());
            }
        }

        if (playerScript.dreaming == true)
        {
            //this.gameObject.layer = 9;

            collides = ~LayerMask.GetMask("Laser", "Background", "UI", "Sleep", "Player");
        }
        else
        {
            //this.gameObject.layer = 8;

            collides = ~LayerMask.GetMask("Laser", "Background", "UI", "Sleep");
        }
        line.GetComponent<SpriteRenderer>().size = new Vector2(line.GetComponent<SpriteRenderer>().size.x, length);
        

        GetComponent<BoxCollider2D>().size = new Vector2(width, length);
        GetComponent<BoxCollider2D>().offset = new Vector2(0, length/2);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(death);
        }
    }

    private void FixedUpdate()
    {
    }

    IEnumerator drawLaser()
    {

        Debug.Log("hit");
        extending = true;
        while (length < maxLength && ray.collider == null)
        {
            
            length += drawSpeed * Time.deltaTime;
            hit.localPosition = new Vector3(0, length, 0);
            yield return null;
        }
    }

    bool laserPulse = false;

    [Header("0-1 values")]
    public float pulseAmount = .4f;
    [Header("how often lasers pulse")]
    public float pulseTimer = .5f;
    IEnumerator pulsing()
    {
        bool pulse = true;
        laserPulse = true;

        float pulseWidth = width;
        while (laserPulse)
        {
            pulseWidth = width;
            while (pulse)
            {
                pulseWidth += (width / 20);
                transform.localScale = new Vector3(pulseWidth, transform.localScale.y, 1);
                if(pulseWidth >= (width + (width * pulseAmount)))
                {
                    pulse = false;
                    transform.localScale = new Vector3(width, transform.localScale.y, 1);
                }
                yield return null;
            }

            yield return new WaitForSeconds(pulseTimer);

            pulse = true;
            yield return null;
        }
    }
}
