using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateObject :InteractableObject
{

    private SpriteRenderer spriteRenderer;
    private Collider2D m_Collider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
    }

	// Update is called once per frame
	void Update () {
        
        if (isActive)
        {
            this.spriteRenderer.enabled = true;
            this.m_Collider.enabled = true;
        }
        else
        {
            this.spriteRenderer.enabled = false;
            this.m_Collider.enabled = false;
        }
	}
}
