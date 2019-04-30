using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour {

    private Scene[] AllScenes;
    private int SceneCount;
    private DeathFade fade;

    void Start() {

        SceneCount = SceneManager.sceneCountInBuildSettings;
        fade = GameObject.Find("DeathFade").GetComponent<DeathFade>();

    }

    private IEnumerator Fade() {

        yield return new WaitForSeconds(1f);
        fade.FadeOut();
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneCount) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player") {
            StartCoroutine(Fade());
            /*
            else if (SceneManager.GetActiveScene().name != "In-Between Level" &&
            SceneManager.GetActiveScene().buildIndex > 1) {
                SceneManager.LoadScene("In-Between Level");
            }
            */
        }
    }
    
}
