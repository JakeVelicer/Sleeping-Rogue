using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBase : MonoBehaviour
{

    public float rotateRange;

    public bool right, left;

    float startRot;

    float leftRot, rightRot;

    float rot = 0.0f;

    public bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.eulerAngles.z;
        leftRot = startRot + rotateRange;
        rightRot = startRot - rotateRange;
        StartCoroutine(Rotate());
    }
    
    IEnumerator Rotate()
    {
        rotating = true;
        do
        {
            if (transform.rotation.z != leftRot && left)
            {
                float newRot = Mathf.SmoothDamp(transform.rotation.z, leftRot, ref rot, 0.5f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
            }
            else
            {
                left = false;
                right = true;
            }
            if (transform.rotation.z != rightRot && right)
            {
                float newRot = Mathf.SmoothDamp(transform.rotation.z, rightRot, ref rot, 0.5f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
            }
            else
            {
                right = false;
                left = true;
            }


            yield return null;
        } while (rotating);
    }
}
