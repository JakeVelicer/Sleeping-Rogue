using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InBetweenLevel : MonoBehaviour
{

    public int DetermineTime;
    public Text TimerText;
    private DeathFade fade;
    private int Timer;
    private PlatformerController Player;
    private bool CountingDown = true;

    // Start is called before the first frame update
    void Start() {

        fade = GameObject.Find("DeathFade").GetComponent<DeathFade>();
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update() {

        
    }

    private IEnumerator CountDown() {

        int Timer = DetermineTime;

        while (CountingDown) {

            if (!PlatformerController.paused) {
                Timer--;
            }
            TimerText.text = "Countdown: " + Timer;
            if (Timer <= 0) {
                CountingDown = false;
            }
            yield return new WaitForSeconds(1);
        }
        fade.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

}
