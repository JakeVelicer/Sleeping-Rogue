using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeathFade : MonoBehaviour
{
    public float FadeRate;
    private Image image;
    private Animator anim;
    
    // Use this for initialization
    void Start () {
        
        image = GetComponent<Image>();
        image.enabled = true;
        anim = GetComponent<Animator>();
        FadeIn();
    }
    
    // Update is called once per frame
    void Update () {

    }

    public void FadeOut()
    {
        anim.Play("FadeOut");
    }

    public void FadeIn()
    {
        anim.Play("FadeIn");
    }
}
