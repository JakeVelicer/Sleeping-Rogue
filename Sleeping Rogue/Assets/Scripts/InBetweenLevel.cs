using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InBetweenLevel : MonoBehaviour
{

    public int DetermineTime;
    public Text TimerText;
    private int Timer;
    private PlatformerController Player;
    private bool CountingDown = true;

    // Start is called before the first frame update
    void Start() {

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

}
