using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamRealSpriteSwitch : MonoBehaviour
{
    public Sprite RealitySprite;
    public Sprite DreamSprite;
    private PlatformerController Player;
    private SpriteRenderer Renderer;

    private float width;
    private bool swapped;

    // Start is called before the first frame update
    void Start() {

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
        Renderer = gameObject.GetComponent<SpriteRenderer>();
        swapped = false;
    }

    // Update is called once per frame
    void Update() {

        if (Player.dreaming && !swapped) {

            swapSprites();
        }
        else if (!Player.dreaming && swapped) {

            swapSprites();
        }
    }

    public void swapSprites()
    {
        swapped = !swapped;
        if (swapped)
        {
            width = Renderer.size.x;
            Renderer.sprite = DreamSprite;
        }
        else
        {
            Renderer.sprite = RealitySprite;
            Renderer.size = new Vector2(width, Renderer.size.y);
        }
    }

}
