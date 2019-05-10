using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip choice;
    public AudioClip select;

    public Button[] Main;
    public Button[] Options;
    public static int collectibles;
    public static Button[] Pause;

    public GameObject mMenu, opMenu;

    public float Volume = 100;

    public static int Selected;
    public bool canInteract = true;

    public bool canSelect = true;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        menuAdded = 0;
        audioSource.PlayOneShot(select);
        canSelect = false;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        audioSource.PlayOneShot(select);
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
            if (canSelect==true)
            {
                audioSource.PlayOneShot(choice);
            }

            StartCoroutine(ChangeMenu(Input.GetAxisRaw("Vertical")));
        }
        if (mMenu != null)
        {
            Main = FindObjectsOfType<Button>();
            for (int i = 0; i < Main.Length; i++)
            {
                if (GameObject.Find("Button " + i) != null)
                {
                    Main[i] = GameObject.Find("Button " + i).GetComponent<Button>();
                }
            }
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Main Menu"))
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    Main[0].onClick.Invoke();
                    audioSource.PlayOneShot(select);
                }
            }
        }
        if (opMenu != null)
        {

            Options = FindObjectsOfType<Button>();
            if (Input.GetButtonDown("Cancel"))
            {
                FindObjectOfType<Button>().onClick.Invoke();
                audioSource.PlayOneShot(select);
            }
            if (EventSystem.current.currentSelectedGameObject == FindObjectOfType<Slider>())
            {

                FindObjectOfType<Slider>().transform.GetChild(2).GetComponentInChildren<Image>().color = FindObjectOfType<Slider>().colors.highlightedColor;
            }
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

            if (GameObject.Find("Credits") != null)
            {
                FindObjectOfType<Button>().Select();
                if (Input.GetButtonDown("Cancel"))
                {
                    FindObjectOfType<Button>().onClick.Invoke();
                    audioSource.PlayOneShot(select);
                }
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
        if (mMenu != null)
        {
         
            if (val < 0)
            {

                Selected++;
                if (Selected > Main.Length - 1)
                {
                    Selected = 0;
                }


            }
            if (val > 0)
            {
                Selected--;
                if (Selected < 0)
                {
                    Selected = Main.Length - 1;
                }
            }
        }
        Debug.Log(Selected);


        if (opMenu != null)
        {
            /*if(Selected == 0)
            {
                FindObjectOfType<Button>().Select();
            }
            else
            {
                FindObjectOfType<Slider>().Select();
            }*/
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            if(EventSystem.current.currentSelectedGameObject == FindObjectOfType<Button>())
            {
                FindObjectOfType<Slider>().GetComponent<Slider>().Select();
            }
            if (EventSystem.current.currentSelectedGameObject == FindObjectOfType<Slider>())
            {
                
                FindObjectOfType<Button>().GetComponent<Button>().Select();
            }
        }

           // Main[Selected].Select();
        yield return new WaitForSeconds(.2f);

        canInteract = true;
    }

}
