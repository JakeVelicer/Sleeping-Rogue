using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamSetActive : MonoBehaviour {
    public SpriteRenderer BG;

	void Start () {
        BG = GetComponent<SpriteRenderer>();
	}
	
	
	void Update ()
    {
            if (!PlayerMovement.dream)
            {
                BG.enabled = true;
            }
            else
            {
                BG.enabled = false;
            }
            
    }
}
