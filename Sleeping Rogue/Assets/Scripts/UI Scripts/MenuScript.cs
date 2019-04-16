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
    public static int collectibles;
    public static Button[] Pause;

    public GameObject mMenu, opMenu;

    public float Volume = 100;

    public static int Selected;
    public bool canInteract = true;

    public int playAdded, menuAdded;

    private void Start()
    {
        opMenu = GameObject.Find("Options");
        mMenu = GameObject.Find("Main");
        Main = FindObjectsOfType<Button>();
        collectibles = 0;
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
            for(int i = 0; i < Main.Length; i++)
            {
                Main[i] = GameObject.Find("Button " + i).GetComponent<Button>();
            }
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
        }
        else
        {
            if (mMenu != null)
            {
                if (menuAdded == 0)
                {
                    Main[2].onClick.AddListener(() => MainMenu());
                    menuAdded++;
                }
                Main[Selected].Select();
            }
        }


    }
    


    private IEnumerator ChangeMenu(float val)
    {
        if (val < 0)
        {

            Selected++;
            if(Selected > Main.Length - 1)
            {
                Selected = 0;
            }

            
        }
        if (val > 0 )
        {
            Selected--;
            if(Selected < 0)
            {
                Selected = Main.Length - 1;
            }
        }
        Debug.Log(Selected);


        if (opMenu != null)
        {
            if(Selected == 0)
            {
                FindObjectOfType<Button>().Select();
            }
            else
            {
                FindObjectOfType<Slider>().Select();
            }
        }

           // Main[Selected].Select();
        yield return new WaitForSeconds(.2f);

        canInteract = true;
    }

}
