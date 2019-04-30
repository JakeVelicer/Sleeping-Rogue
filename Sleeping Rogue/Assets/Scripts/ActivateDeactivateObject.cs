using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateObject : InteractableObject
{

    private SpriteRenderer spriteRenderer1;
    private SpriteRenderer spriteRenderer2;
    private Collider2D m_Collider1;
    private Collider2D m_Collider2;
    private MeshRenderer m_meshRenderer;
    private bool isText = false;
    public float showDistance = 7.5f;

    void Start()
    {
        if (this.gameObject.tag == "Text")
        {
            isText = true;
        }
        if (GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer1 = GetComponent<SpriteRenderer>();
        }
        if (GetComponent<Collider2D>() != null)
        {
            m_Collider1 = GetComponent<Collider2D>();
        }
        if (transform.childCount > 0)
        {
            if (gameObject.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
            {
                m_meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            }
            if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
            {
                spriteRenderer2 = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
            {
                m_Collider2 = gameObject.transform.GetChild(0).GetComponent<Collider2D>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isText == false)
        {
            if (isActive)
            {
                if (spriteRenderer1 != null)
                {
                    this.spriteRenderer1.enabled = true;
                }
                if (spriteRenderer2 != null)
                {
                    this.spriteRenderer2.enabled = true;
                }
                if (m_Collider1 != null)
                {
                    this.m_Collider1.enabled = true;
                }
                if (m_Collider2 != null)
                {
                    this.m_Collider2.enabled = true;
                }
                if (m_meshRenderer != null)
                {
                    this.m_meshRenderer.enabled = true;
                }
            }
            else
            {
                if (spriteRenderer1 != null)
                {
                    this.spriteRenderer1.enabled = false;
                }
                if (spriteRenderer2 != null)
                {
                    this.spriteRenderer2.enabled = false;
                }
                if (m_Collider1 != null)
                {
                    this.m_Collider1.enabled = false;
                }
                if (m_Collider2 != null)
                {
                    this.m_Collider2.enabled = false;
                }
                if (m_meshRenderer != null)
                {
                    this.m_meshRenderer.enabled = false;
                }
            }
        }
        else
        {
            if (isActive && (Vector2.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < showDistance))
            {
                if (spriteRenderer1 != null)
                {
                    this.spriteRenderer1.enabled = true;
                }
                if (spriteRenderer2 != null)
                {
                    this.spriteRenderer2.enabled = true;
                }
                if (m_Collider1 != null)
                {
                    this.m_Collider1.enabled = true;
                }
                if (m_Collider2 != null)
                {
                    this.m_Collider2.enabled = true;
                }
                if (m_meshRenderer != null)
                {
                    this.m_meshRenderer.enabled = true;
                }
            }
            else
            {
                if (spriteRenderer1 != null)
                {
                    this.spriteRenderer1.enabled = false;
                }
                if (spriteRenderer2 != null)
                {
                    this.spriteRenderer2.enabled = false;
                }
                if (m_Collider1 != null)
                {
                    this.m_Collider1.enabled = false;
                }
                if (m_Collider2 != null)
                {
                    this.m_Collider2.enabled = false;
                }
                if (m_meshRenderer != null)
                {
                    this.m_meshRenderer.enabled = false;
                }
            }

        }
    }
}