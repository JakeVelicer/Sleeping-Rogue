﻿using System.Collections;
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
    public static int collectibles;
    public GameObject mMenu, opMenu;

    public static int Selected;
    public bool canInteract = true;

    public int playAdded, menuAdded;

    private void Start()
    {
        collectibles = 0;
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
        menuAdded = 0;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        playAdded = 0;
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
        if (mMenu != null)
        {
            Main = FindObjectsOfType<Button>();
            Main[0] = GameObject.Find("Button 0").GetComponent<Button>();
            Main[1] = GameObject.Find("Button 1").GetComponent<Button>();
            Main[2] = GameObject.Find("Button 2").GetComponent<Button>();
        }
        if (opMenu != null)
        {
            Options = FindObjectsOfType<Button>();
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {

            if (mMenu != null)
            {
                if (playAdded == 0)
                {
                    Main[0].onClick.AddListener(PlayGame);
                    playAdded++;
                }
                Main[Selected].Select();
            }
            else if (opMenu != null)
            {
                Options[0].Select();
            }
        }
        else
        {
            if (mMenu != null)
            {
                if (menuAdded == 0)
                {
                    Main[2].onClick.AddListener(MainMenu);
                    menuAdded++;
                }
                Main[Selected].Select();
            }
            else if (opMenu != null)
            {
                Options[0].Select();
            }
        }


    }



    private IEnumerator ChangeMenu(float val)
    {
            if (mMenu != null)
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
        Debug.Log(Selected);
        
            Main[Selected].Select();
        yield return new WaitForSeconds(.2f);

        canInteract = true;
    }

}
