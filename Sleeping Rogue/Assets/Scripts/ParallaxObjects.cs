using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObjects : MonoBehaviour {

    public Transform[] Backgrounds;
    private float[] parallaxScales;
    public float Smoothing;

    private Transform Cam;
    private Vector3 previousCamPosition;

    void Awake() {
        
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Start() {

        previousCamPosition = Cam.position;
        parallaxScales = new float[Backgrounds.Length];

        for (int i = 0; i < Backgrounds.Length; i++) {
            parallaxScales[i] = Backgrounds[i].position.z* -1;
        }
        
    }

    void Update() {

        for (int i = 0; i < Backgrounds.Length; i++) {

            float parallax = (previousCamPosition.x - Cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = Backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, Backgrounds[i].position.y, Backgrounds[i].position.z);
        
            Backgrounds[i].position = Vector3.Lerp (Backgrounds[i].position, backgroundTargetPos, Smoothing * Time.deltaTime);
        }

        previousCamPosition = Cam.position;
        
    }
}