using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{
    private Transform Player;
    private CinemachineVirtualCamera CameraController;
    public GameObject ZoomedOutCameraMover;
    public float NormalZoom;
    public float ZoomOut;
    [HideInInspector] public bool Switching;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        CameraController = gameObject.GetComponent<CinemachineVirtualCamera>();
        CameraController.Follow = Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CameraZoom") && Player.GetComponent<PlatformerController>().grounded) {
            if (!Switching) {
                StartCoroutine(ZoomController());
            }
        }

    }

    private IEnumerator ZoomController() {

        var CurrentSize = CameraController.m_Lens.OrthographicSize;
        Switching = true;

        // If the camera is zoomed in
        if (CurrentSize <= NormalZoom) {
            Player.GetComponent<PlatformerController>().canMove = false;
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
            Player.GetComponent<PlatformerController>().canMove = true;
            Destroy(GameObject.Find("CameraMoverZoomedOut(Clone)"));
        }
        Switching = false;
    }
}
