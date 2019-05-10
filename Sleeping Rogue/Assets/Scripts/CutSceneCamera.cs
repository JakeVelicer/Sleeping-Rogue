using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour
{

    public AudioSource audioManager;
    public GameObject[] Dialogue;
    private DeathFade fade;

    // Start is called before the first frame update
    void Start()
    {
        fade = GameObject.Find("DeathFade").GetComponent<DeathFade>();
        StartCoroutine(CameraMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit")) {
            if (SceneManager.GetActiveScene().name == "CutScene") {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (SceneManager.GetActiveScene().name == "CutScene2") {
                SceneManager.LoadScene(0);
            }
        }
    }

    private IEnumerator CameraMovement() {

        audioManager.Play(0);
        for (int i = 0; i <= Dialogue.Length; i++) {
            if (i == 0) {
                yield return new WaitForSeconds(2);
                Dialogue[i].SetActive(true);
            }
            else if (i != 0) {
                yield return new WaitForSeconds(8);
                if (i == Dialogue.Length) {
                    Dialogue[i - 1].SetActive(false);
                }
                else {
                    Dialogue[i - 1].SetActive(false);
                    Dialogue[i].SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "CutScene") {
            GameObject.Find("PlayerAnim").GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(4);
        }
        fade.FadeOut();
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "CutScene") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (SceneManager.GetActiveScene().name == "CutScene2") {
            SceneManager.LoadScene(0);
        }
    }
}
