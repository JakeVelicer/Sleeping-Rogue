using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBase : MonoBehaviour
{

    public float rotateRange;

    bool right, left;

    float startRot;

    public bool smooth;

    float leftRot, rightRot;

    float rot = 0.0f;

    public bool rotating;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.eulerAngles.z;
        leftRot = startRot + rotateRange;
        rightRot = startRot - rotateRange;
        if (rotating)
        {
            StartCoroutine(Rotate());
        }
    }
    
    IEnumerator Rotate()
    {
        left = true;
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
                if (smooth)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, targ, Time.deltaTime * 1.5f);
                }
                else transform.Rotate(new Vector3(0, 0, leftRot) * Time.deltaTime * 1.5f);

                if (transform.eulerAngles.z >= leftRot && transform.eulerAngles.z < 100) 
                {
                    left = false;
                    right = true;
                    Debug.Log("Right Now");
                }
                if(transform.rotation == targ)
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
                Debug.Log(rightRot);
                if (smooth)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, targ, Time.deltaTime * 1.5f);
                }
                else transform.Rotate(new Vector3(0, 0, rightRot) * Time.deltaTime * 1.5f);
                if (transform.eulerAngles.z <= (360 + rightRot) && transform.eulerAngles.z > 300)
                {
                    left = true;
                    right = false;
                    Debug.Log("LEft Now");
                }
                if(transform.rotation == targ)
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
