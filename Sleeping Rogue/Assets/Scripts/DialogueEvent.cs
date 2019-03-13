using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour {

    private CameraBehavior MainCameraScript;
    private AudioSource Source;
    public AudioClip[] Dialogue;
    public GameObject RoofBorder;
    public GameObject FloorBorder;
    public GameObject LeftBorder;
    public GameObject RightBorder;
    [HideInInspector] public bool LockTheCamera;

    // Start is called before the first frame update
    void Start()
    {
        Source = gameObject.GetComponent<AudioSource>();
        MainCameraScript = GameObject.Find("CM vcam1").GetComponent<CameraBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LockTheCamera == true) {
            RoofBorder.SetActive(true);
            FloorBorder.SetActive(true);
            LeftBorder.SetActive(true);
            RightBorder.SetActive(true);                
        }
        else {
            RoofBorder.SetActive(false);
            FloorBorder.SetActive(false);
            LeftBorder.SetActive(false);
            RightBorder.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Player") {
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(ActivateSound());
        }
    }

    private IEnumerator ActivateSound() {

        MainCameraScript.OnDialogueTrigger = true;
        MainCameraScript.CameraController.Follow = gameObject.transform;
        LockTheCamera = true;
        for (int i = 0; i < Dialogue.Length; i++) {
            Source.clip = Dialogue[i];
            Source.loop = false;
            Source.Play();
            yield return new WaitForSeconds(Source.clip.length + 1);
        }
        LockTheCamera = false;
        MainCameraScript.CameraController.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        MainCameraScript.OnDialogueTrigger = false;
    }
}
