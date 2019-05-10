using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour {

    private Scene[] AllScenes;
    private int SceneCount;
    private DeathFade fade;
    private PlatformerController Player;

    void Start() {

        SceneCount = SceneManager.sceneCountInBuildSettings;
        Player = GameObject.Find("Player").GetComponent<PlatformerController>();
        fade = GameObject.Find("DeathFade").GetComponent<DeathFade>();

    }

    private IEnumerator Fade() {

        Player.Freeze = true;
        Player.rb2d.velocity = Vector2.zero;
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

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Player" && !Player.dreaming) {
            
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
