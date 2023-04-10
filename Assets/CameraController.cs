using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    Vector3 vectorMove = Vector3.zero;
    Vector3 vectorDown;
    GameSettings gameSettings;
    public float speed = 500f;
    public float rotationSpeed = 500f;
    Vector3 rotation;
    public CinemachineVirtualCamera camera;
    public float[] fovs;
    public float cameraSensitivity = 10f;

    private void Start()
    {
        fovs[0] = camera.m_Lens.FieldOfView;
        gameSettings = FindObjectOfType<GameSettings>();
    }

    public void CameraSens()
    {
        speed = gameSettings.mouseSens * 10 + 200;
    }

    public void CameraZoom()
    {
        fovs[0] += Input.mouseScrollDelta.y * cameraSensitivity;
        fovs[0] = Mathf.Clamp(fovs[0],fovs[1], fovs[2]);
        camera.m_Lens.FieldOfView = fovs[0];
    }

    public void CameraMoveHorizontal()
    {
        //vectorMove.x = Input.GetAxisRaw("Horizontal");
        vectorMove.z = Input.GetAxisRaw("Horizontal");

        vectorMove = transform.forward * vectorMove.x + transform.forward * vectorMove.z;

        transform.position += vectorMove * Time.deltaTime * speed;
    }

    public void CameraMoveVertical()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Input.GetAxisRaw("Vertical") * Time.deltaTime * speed, transform.position.z);
    }

    public void CameraRotate()
    {
        rotation = Vector3.zero;
        if (Input.GetKey(KeyCode.E))
        {
            rotation.y = rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotation.y = -(rotationSpeed * Time.deltaTime);
        }
        transform.eulerAngles += rotation;
    }

    void Update()
    {
        CameraMoveHorizontal();
        CameraMoveVertical();
        CameraZoom();
        CameraRotate();
    }
}
