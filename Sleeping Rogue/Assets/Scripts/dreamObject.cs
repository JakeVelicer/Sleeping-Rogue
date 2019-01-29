using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dreamObject : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    bool active;
    Collider2D m_Collider;
    // Use this for initialization
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = false;
        active = false;
        m_Collider = GetComponent<Collider2D>();
        this.m_Collider.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMovement.dream)
        {
            this.spriteRenderer.enabled = false;
            this.m_Collider.enabled = false;
            active = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
        else
        {
            this.spriteRenderer.enabled = true;
            this.m_Collider.enabled = true;
            active = true;
            if (gameObject.tag == "Box")
            {


                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
