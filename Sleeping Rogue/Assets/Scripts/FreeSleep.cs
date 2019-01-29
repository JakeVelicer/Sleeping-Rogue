using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSleep : MonoBehaviour {


    public GameObject shadow;
    public GameObject dreamBG;

    private void Start()
    {
        shadow = Instantiate(shadow);
        EnemyMovement.player = shadow;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!PlayerMovement.dream)
            {
                PlayerMovement.dream = true;
                gameObject.layer = 9;

            }
            else
            {
                PlayerMovement.dream = false;
                gameObject.layer = 0;
            }
            transform.position = shadow.transform.position;
        }
    }
}
