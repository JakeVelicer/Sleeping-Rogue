using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeathFade : MonoBehaviour
{
    public float FadeRate;
    private Image image;
    private float targetAlpha;
    
    // Use this for initialization
    void Start () {
        image = this.GetComponent<Image>();
        image.enabled = true;
        targetAlpha = image.color.a;
        FadeOut();
    }
    
    // Update is called once per frame
    void Update () {
        Color curColor = this.image.color;
        float alphaDiff = Mathf.Abs(curColor.a-this.targetAlpha);
        if (alphaDiff>0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a,targetAlpha,this.FadeRate*Time.deltaTime);
            image.color = curColor;
        }
    }

    public void FadeOut()
    {
        this.targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
    }
}
