using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractableObject {

    public bool MoveUp;
    public float MoveTime;
    private  bool CanMove;

	// Update is called once per frame
	void Update () {
        if (isActive && !CanMove)
        {
            StartCoroutine(Activation());
        }
        Movement();
	}

    private IEnumerator Activation () {
        isActive = false;
        CanMove = true;
        yield return new WaitForSeconds(MoveTime);
        CanMove = false;
        if (MoveUp) {
            MoveUp = false;
        }
        else if (!MoveUp) {
            MoveUp = true;
        }
    }

    private void Movement() {
        if (CanMove) {
            if (MoveUp) {
                transform.Translate(Vector3.up * 0.1f, Space.World);
            }
            else if (!MoveUp) {
                transform.Translate(Vector3.down * 0.1f, Space.World);
            }
        }
    }
}
