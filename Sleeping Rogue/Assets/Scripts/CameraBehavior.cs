using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.GetComponent<CinemachineVirtualCamera>().Follow = Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
