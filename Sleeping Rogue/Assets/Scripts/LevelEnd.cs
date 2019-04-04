using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    private Scene[] AllScenes;
    private int SceneCount;

    void Start() {

        SceneCount = SceneManager.sceneCountInBuildSettings;

    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player") {

            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneCount) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else {
                SceneManager.LoadScene(0);
            }
            /*
            else if (SceneManager.GetActiveScene().name != "In-Between Level" &&
            SceneManager.GetActiveScene().buildIndex > 1) {
                SceneManager.LoadScene("In-Between Level");
            }
            */
        }
    }
    
}
