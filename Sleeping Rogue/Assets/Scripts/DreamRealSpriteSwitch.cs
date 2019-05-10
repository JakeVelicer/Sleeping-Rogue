using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamRealSpriteSwitch : MonoBehaviour
{
    public Sprite RealitySprite;
    public Sprite DreamSprite;
    private PlatformerController Player;
    private SpriteRenderer Renderer;

    // Start is called before the first frame update
    void Start() {

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerController>();
        Renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

        if (Player.dreaming) {

            Renderer.sprite = DreamSprite;
        }
        else {
            
            Renderer.sprite = RealitySprite;
        }
    }
}
