using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehavior : MonoBehaviour
{
    private Transform Player;
    private CinemachineVirtualCamera CameraController;
    public float NormalZoom;
    public float ZoomOut;
    private float ZoomAmount;
    private bool Switching;

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

        if (CurrentSize >= ZoomOut) {
            ZoomAmount = NormalZoom;
            Player.GetComponent<PlatformerController>().canMove = true;
            while (CameraController.m_Lens.OrthographicSize > ZoomAmount) {
                CameraController.m_Lens.OrthographicSize -= 1;
                yield return new WaitForSeconds(0.001f);
            }
        }
        else if (CurrentSize <= NormalZoom) {
            ZoomAmount = ZoomOut;
            Player.GetComponent<PlatformerController>().canMove = false;
            while (CameraController.m_Lens.OrthographicSize < ZoomAmount) {
                CameraController.m_Lens.OrthographicSize += 1;
                yield return new WaitForSeconds(0.001f);
            }
        }
        Switching = false;
    }
}
