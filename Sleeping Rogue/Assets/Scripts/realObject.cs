using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realObject : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    bool active;
    Collider2D m_Collider;
    // Use this for initialization
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        active = true;
        m_Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
            if (PlayerMovement.dream)
            {
                this.spriteRenderer.enabled = false;
                this.m_Collider.enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                active = false;
            }
            else
            {
                this.spriteRenderer.enabled = true;
                this.m_Collider.enabled = true;
            if (gameObject.tag == "Box")
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            active = true;
            }
    }
}