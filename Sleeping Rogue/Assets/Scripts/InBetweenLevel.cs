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

    // Start is called before the first frame update
    void Start() {

        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update() {

        
    }

    private IEnumerator CountDown() {

        for (int Timer = DetermineTime; Timer >= 0; Timer--) {

            TimerText.text = "Countdown: " + Timer;
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
