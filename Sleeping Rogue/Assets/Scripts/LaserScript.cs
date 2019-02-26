using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : InteractableObject {

    public float length;
    public float width;
    public LineRenderer line;
    private PlatformerController PlayerScript;


	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
    }
	
	// Update is called once per frame
	void Update () {
        
            if (!isActive)
            {
                line.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                line.enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        if (PlayerScript.dreaming == true)
        {
            this.gameObject.layer = 9;
        }
        else this.gameObject.layer = 8;
        
        line.startWidth = width;
        line.endWidth = width;
        float pos = line.GetPosition(1).x - line.GetPosition(0).x;
        GetComponent<BoxCollider2D>().size = new Vector2(pos, width);
        GetComponent<BoxCollider2D>().offset = new Vector2(pos/2, 0);

	}

    private void FixedUpdate()
    {
        LayerMask lasers = ~LayerMask.GetMask("Laser", "Background", "UI");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, length, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, transform.right, Color.black);

        if(ray.collider)
        {
            Debug.Log(ray.collider.name);
            line.SetPosition(1, new Vector3(ray.distance, 0, 0));
        } else
        {
            line.SetPosition(1, new Vector3(length, 0, 0));
        }
        
    }
}
