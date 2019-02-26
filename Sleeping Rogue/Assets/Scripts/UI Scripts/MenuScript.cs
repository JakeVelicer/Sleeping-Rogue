using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public Button[] Main;
    public Button[] Options;

    public static Button[] Pause;

    public GameObject mMenu, opMenu;

    public static int Selected;
    public bool canInteract = true;

    private void Start()
    {
        opMenu = GameObject.Find("Options");
        mMenu = GameObject.Find("Main");
        Main = FindObjectsOfType<Button>();

        Main[Selected].Select();
        canInteract = true;
        GameObject[] manage = GameObject.FindGameObjectsWithTag("Manager");
        if (manage.Length > 1)
        {
            Destroy(manage[0]);
        }
        DontDestroyOnLoad(this.gameObject);

    }


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Resume()
    {
        PlatformerController.paused = false;
    }

    public void resetSelected()
    {
        Selected = 0;
    }

    private void Update()
    {

        opMenu = GameObject.Find("Options");
        mMenu = GameObject.Find("Main");

        if (Input.GetAxisRaw("Vertical") != 0 && canInteract)
        {
            canInteract = false;
            StartCoroutine(ChangeMenu(Input.GetAxisRaw("Vertical")));
        }
        if (mMenu.activeSelf)
        {
            Main = FindObjectsOfType<Button>();
            Main[0] = GameObject.Find("Button 0").GetComponent<Button>();
            Main[1] = GameObject.Find("Button 1").GetComponent<Button>();
            Main[2] = GameObject.Find("Button 2").GetComponent<Button>();
        }
        if (opMenu.activeSelf)
        {
            Options = FindObjectsOfType<Button>();
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {

            if (mMenu.activeSelf)
            {
                Main[0].onClick.AddListener(PlayGame);
                Main[Selected].Select();
            }
            else if (opMenu.activeSelf)
            {
                Options[0].Select();
            }
        }
        else
        {
            if (mMenu.activeSelf)
            {
                if (Pause.Length > 0)
                {
                    Pause[Selected].Select();
                }
            }
            else if (opMenu.activeSelf)
            {
                Options[0].Select();
            }
        }


    }



    private IEnumerator ChangeMenu(float val)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {
            if (mMenu.activeSelf)
            {
                if (val < 0 && Selected < Main.Length - 1)
                {
                    Selected++;
                }
                if (val > 0 && Selected > 0)
                {
                    Selected--;

                }
            }
        }
        else
        {
            if (val < 0 && Selected < Pause.Length - 1)
            {
                Selected++;
            }
            if (val > 0 && Selected > 0)
            {
                Selected--;
            }
        }
        Debug.Log(Selected);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {
            Main[Selected].Select();
        }
        else
        {
            Pause[Selected].Select();
        }
        yield return new WaitForSeconds(.2f);

        canInteract = true;
    }

}

