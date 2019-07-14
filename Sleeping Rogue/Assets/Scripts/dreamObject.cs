using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dreamObject : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Collider2D m_Collider;
    private PlatformerController Player;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameObject.GetComponent<Collider2D>() != null) {
            m_Collider = GetComponent<Collider2D>();
        }
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.dreaming)
        {
            this.spriteRenderer.enabled = true;
            if (m_Collider != null) {
                this.m_Collider.enabled = true;
            }
        }
        else
        {
            this.spriteRenderer.enabled = false;
            if (m_Collider != null) {
                this.m_Collider.enabled = false;
            }
        }
    }
}
