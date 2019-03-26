using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour {

    private AudioSource Source;
    public AudioClip[] Dialogue;

    // Start is called before the first frame update
    void Start()
    {
        Source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.tag == "Player") {
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(ActivateSound());
        }
    }

    private IEnumerator ActivateSound() {

        for (int i = 0; i < Dialogue.Length; i++) {
            Source.clip = Dialogue[i];
            Source.loop = false;
            Source.Play();
            yield return new WaitForSeconds(Source.clip.length + 1);
        }
    }
}
