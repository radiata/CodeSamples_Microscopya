using Cinemachine;
using UnityEngine;

public class PuzzleCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera puzzleVirtualCamera;
    [SerializeField] private CameraZoomController cameraZoomController;
    [SerializeField] private float baseZoom;

    private float baseDutch = 0;

    public void Activate()
    {
        puzzleVirtualCamera.m_Lens.Dutch = baseDutch;
        puzzleVirtualCamera.m_Lens.OrthographicSize = baseZoom;
        cameraZoomController.enabled = true;
        CameraZoomController.SetZoomTo(baseZoom);
        puzzleVirtualCamera.enabled = true;
    }

    public void Deactivate()
    {
        cameraZoomController.enabled = false;
        puzzleVirtualCamera.enabled = false;
    }

    private void Awake()
    {
        baseDutch = puzzleVirtualCamera.m_Lens.Dutch;
        puzzleVirtualCamera.enabled = false;
        cameraZoomController.enabled = false;
    }
}
