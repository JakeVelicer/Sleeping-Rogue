using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour
{

    public GameObject Camera;
    private bool moveDown = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CameraMovement() {

        yield return new WaitForSeconds(20);
        moveDown = false;

    }
}
