﻿using System.Collections;
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
        for(int i = 0; i < allSound.Length; i++)
        {
            if(allSound[i] != null)
            {
                allSound[i].volume = initVolumes[i] * (GetComponent<Slider>().value / 100);
            }
        }
        
        
    }
}