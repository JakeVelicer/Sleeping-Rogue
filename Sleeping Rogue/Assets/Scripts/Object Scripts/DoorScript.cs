using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractableObject {

    public bool MoveUp;
    private  bool CanMove;

    private SpriteRenderer render;
    public AudioClip door;
    public AudioSource audioSource;

    void Start() {

        render = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        
        if (isActive && !CanMove)
        {
            StartCoroutine(Activation());
        }
	}

    private IEnumerator Activation() {
        isActive = false;
        CanMove = true;
        for (int i = 0; i < (render.bounds.size.y * 2); i++) {
            audioSource.PlayOneShot(door);
            if (MoveUp) {
                transform.Translate(Vector3.up * 0.5f, Space.World);
            }
            else if (!MoveUp) {
                transform.Translate(Vector3.down * 0.5f, Space.World);
            }
            yield return new WaitForSeconds(0.0005f);
        }
        if (MoveUp) {
            MoveUp = false;
        }
        else if (!MoveUp) {
            MoveUp = true;
        }
        CanMove = false;
    }

}
