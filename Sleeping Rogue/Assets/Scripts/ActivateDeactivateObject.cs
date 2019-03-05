using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateObject :InteractableObject
{

    private SpriteRenderer spriteRenderer1;
    private SpriteRenderer spriteRenderer2;
    private Collider2D m_Collider;
    private MeshRenderer m_meshRenderer;

    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) {
            spriteRenderer1 = GetComponent<SpriteRenderer>();
        }
        if (GetComponent<Collider2D>() != null) {
            m_Collider = GetComponent<Collider2D>();
        }
        if (gameObject.transform.GetChild(0).GetComponent<MeshRenderer>() != null) {
            m_meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        if (gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>() != null) {
           spriteRenderer2 = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

	// Update is called once per frame
	void Update () {
        
        if (isActive)
        {
            if (spriteRenderer1 != null) {
                this.spriteRenderer1.enabled = true;
            }
            if (spriteRenderer2 != null) {
                this.spriteRenderer2.enabled = true;
            }
            if (m_Collider != null) {
                this.m_Collider.enabled = true;
            }
            if (m_meshRenderer != null) {
                this.m_meshRenderer.enabled = true;
            }
        }
        else
        {
            if (spriteRenderer1 != null) {
                this.spriteRenderer1.enabled = false;
            }
            if (spriteRenderer2 != null) {
                this.spriteRenderer2.enabled = false;
            }
            if (m_Collider != null) {
                this.m_Collider.enabled = false;
            }
            if (m_meshRenderer != null) {
                this.m_meshRenderer.enabled = false;
            }
        }
	}
}
