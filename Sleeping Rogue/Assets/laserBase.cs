using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBase : MonoBehaviour
{

    public float rotateRange;

    bool right, left;

    float startRot;

    float leftRot, rightRot;

    float rot = 0.0f;

    bool rotating;

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
            //    if (transform.eulerAngles.z != leftRot && left)
            //    {
            //        float newRot = Mathf.SmoothDamp(transform.rotation.z, leftRot, ref rot, 0.5f);
            //        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
            //    }
            //    else if(transform.eulerAngles.z == leftRot)
            //    {
            //        left = false;
            //        right = true;
            //    }
            //    if (transform.eulerAngles.z != rightRot && right)
            //    {
            //        float newRot = Mathf.SmoothDamp(transform.rotation.z, rightRot, ref rot, 0.5f);
            //        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
            //    }
            //    else if (transform.eulerAngles.z == rightRot)
            //    {
            //        right = false;
            //        left = true;
            //    }
            
            while (left)
            {
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
                Quaternion targ = Quaternion.Euler(0, 0, leftRot);
                transform.rotation = Quaternion.Lerp(transform.rotation, targ, Time.deltaTime * 1.5f);
                if (transform.rotation == targ) 
                {
                    left = false;
                    right = true;
                    Debug.Log("Right Now");
                }
                yield return null;
            }
            while (right)
            {
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newRot);
                Quaternion targ = Quaternion.Euler(0, 0, rightRot);
                transform.rotation = Quaternion.Lerp(transform.rotation, targ, Time.deltaTime * 1.5f);
                if (transform.rotation == targ)
                {
                    left = true;
                    right = false;
                    Debug.Log("LEft Now");
                }

                yield return null;
            }
            yield return null;
        } while (rotating);
    }
}
