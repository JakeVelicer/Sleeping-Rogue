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

    public int Selected;
    public bool canInteract = true;

    private void Start()
    {
        Main[Selected].Select();
        canInteract = true;
        GameObject[] manage = GameObject.FindGameObjectsWithTag("Manager");
        if(manage.Length > 1)
        {
            Destroy(manage[1]);
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

    private void Update()
    {
        if(Input.GetAxisRaw("Vertical") != 0 && canInteract)
        {
            canInteract = false;
            StartCoroutine(ChangeMenu(Input.GetAxisRaw("Vertical")));
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {
            if (opMenu.activeSelf)
            {
                Options[0].Select();
            }
            if (mMenu.activeSelf)
            {
                Main[Selected].Select();
            }
        }
        else
        {
            if(Pause.Length > 0)
            {
                Pause[Selected].Select();
            }
            else
            {
                Pause[0].Select();
            }
        }
        
    }

    IEnumerator ChangeMenu(float val)
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
