using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private SpriteRenderer Render;

    public bool isHit = false;

    private void Start() {

        Render = GetComponent<SpriteRenderer>();
    }

    private void Update() {

        /*
        if (isHit) {
            Render.sprite = Activated;
        }
        else {
             Render.sprite = Unactivated;
        }
        */
        
    }
}
