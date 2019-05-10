using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    GameObject manage;

    List<float> initVolumes = new List<float>();

    AudioSource[] allSound;
    // Start is called before the first frame update
    void Start()
    {
        manage = GameObject.Find("Game Manager");
        GetComponent<Slider>().value = manage.GetComponent<MenuScript>().Volume;

        manage.GetComponent<MenuScript>().MaxVolume = GetComponent<Slider>().maxValue;


        allSound = FindObjectsOfType<AudioSource>();
        foreach (AudioSource i in allSound)
        {
            initVolumes.Add(i.volume);
        }

        setVolumes();

    }

    // Update is called once per frame
    void Update()
    {
        if(manage.GetComponent<MenuScript>().Volume != GetComponent<Slider>().value)
        {
            manage.GetComponent<MenuScript>().Volume = GetComponent<Slider>().value;
            setVolumes();
        }

    }


    public void setVolumes()
    {
        for(int i = 0; i < initVolumes.Count; i++)
        {
            if(allSound[i] != null)
            {
                Debug.Log("Setting");
                allSound[i].volume = initVolumes[i] * (manage.GetComponent<MenuScript>().Volume / manage.GetComponent<MenuScript>().MaxVolume);
            }
        }
        
        
    }
}
