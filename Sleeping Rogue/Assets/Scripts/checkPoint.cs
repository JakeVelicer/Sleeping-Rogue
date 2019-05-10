using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private Animator anim;
    public bool isHit = false;

    private void Start() {

        anim = GetComponent<Animator>();
    }

    public void ActivateCheckpoint() {

        isHit = true;
        anim.Play("CheckpointActivated");
    }

    public void DeactivateCheckpoint() {

        anim.Play("CheckpointDead");
    }
}
