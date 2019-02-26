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

    public GameObject mMenu, opMenu;

    public int Selected;
    public bool canInteract = true;

    private void Start()
    {
        Main[Selected].Select();
        canInteract = true;
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

    private void Update()
    {
        Debug.Log(mMenu.activeSelf);
        if(Input.GetAxisRaw("Vertical") != 0 && mMenu.activeSelf && canInteract)
        {
            canInteract = false;
            StartCoroutine(ChangeMenu(Input.GetAxisRaw("Vertical")));
        }

        if (opMenu.activeSelf)
        {
            Options[0].Select();
        }
        if (mMenu.activeSelf)
        {
            Main[Selected].Select();
        }
        
        
    }

    IEnumerator ChangeMenu(float val)
    {
        if(val < 0 && Selected < Main.Length - 1)
        {
            Selected++;
        }
        if(val > 0 && Selected > 0 )
        {
            Selected--;
            
        }
        Debug.Log(Selected);

        Main[Selected].Select();

        yield return new WaitForSeconds(.2f);

        canInteract = true;
    }

}
