﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{
    private Transform Player;
    [HideInInspector] public CinemachineVirtualCamera CameraController;
    private CinemachineFramingTransposer FramingTransposer;
    private float NormalZoom;
    public GameObject ZoomedOutCameraMover;
    public float ZoomOut;
    [HideInInspector] public bool Switching;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerFollower").transform;
        CameraController = gameObject.GetComponent<CinemachineVirtualCamera>();
        FramingTransposer = CameraController.GetCinemachineComponent<CinemachineFramingTransposer>();
        NormalZoom = CameraController.m_Lens.OrthographicSize;
        CameraController.Follow = Player;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetButtonDown("CameraZoom") && Player.GetComponent<PlatformerController>().grounded) {
            if (!Switching) {
                StartCoroutine(ZoomController());
            }
        }
        */
    }

    private IEnumerator ZoomController() {

        var CurrentSize = CameraController.m_Lens.OrthographicSize;
        Switching = true;

        // If the camera is zoomed in
        if (CurrentSize <= NormalZoom) {
            Player.GetComponent<PlatformerController>().canMove = false;
            FramingTransposer.m_DeadZoneWidth = 0;
            FramingTransposer.m_DeadZoneHeight = 0;
            FramingTransposer.m_LookaheadTime = 0;
            while (CameraController.m_Lens.OrthographicSize < ZoomOut) {
                CameraController.m_Lens.OrthographicSize += 1;
                yield return new WaitForSeconds(0.001f);
            }
            Instantiate(ZoomedOutCameraMover, Player.position, Quaternion.identity);
            CameraController.Follow = GameObject.Find("CameraMoverZoomedOut(Clone)").transform;
        }
        // If the camera is zoomed out
        else if (CurrentSize >= ZoomOut) {
            while (CameraController.m_Lens.OrthographicSize > NormalZoom) {
                if (GameObject.Find("CameraMoverZoomedOut(Clone)").transform != Player) {
                    CameraController.m_Lens.OrthographicSize -= 1;
                }
                yield return new WaitForSeconds(0.001f);
            }
            CameraController.Follow = Player;
            FramingTransposer.m_DeadZoneWidth = 0.2f;
            FramingTransposer.m_DeadZoneHeight = 0.1f;
            FramingTransposer.m_LookaheadTime = 0.1f;
            Player.GetComponent<PlatformerController>().canMove = true;
            Destroy(GameObject.Find("CameraMoverZoomedOut(Clone)"));
        }
        Switching = false;
    }
}
