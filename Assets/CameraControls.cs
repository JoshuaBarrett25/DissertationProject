using UnityEngine;


public class CameraControls : MonoBehaviour
{
    public Rigidbody rb;
    private float moveSpeed = 50.0f;
    private float rotationSpeed = 10.0f;
    private float zoomSpeed = 1000.0f;
    private string mouseY = "Mouse Y";
    private string mouseX = "Mouse X";
    private string zoomAxis = "Mouse ScrollWheel";

    private KeyCode forwardKey = KeyCode.W;
    private KeyCode backKey = KeyCode.S;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;

    private KeyCode flatMoveKey = KeyCode.LeftShift;

    private KeyCode anchoredMoveKey = KeyCode.Mouse2;

    private KeyCode anchoredRotateKey = KeyCode.Mouse1;


    private void LateUpdate()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(forwardKey))
            move += Vector3.forward * moveSpeed;
        if (Input.GetKey(backKey))
            move += Vector3.back * moveSpeed;
        if (Input.GetKey(leftKey))
            move += Vector3.left * moveSpeed;
        if (Input.GetKey(rightKey))
            move += Vector3.right * moveSpeed;

        //By far the simplest solution I could come up with for moving only on the Horizontal plane - no rotation, just cache y
        if (Input.GetKey(flatMoveKey))
        {
            float origY = transform.position.y;

            transform.Translate(move);
            transform.position = new Vector3(transform.position.x, origY, transform.position.z);

            return;
        }

        float mouseMoveY = Input.GetAxis(mouseY);
        float mouseMoveX = Input.GetAxis(mouseX);

        //Move the camera when anchored
        if (Input.GetKey(anchoredMoveKey))
        {
            move += Vector3.up * mouseMoveY * -moveSpeed;
            move += Vector3.right * mouseMoveX * -moveSpeed;
        }

        //Rotate the camera when anchored
        if (Input.GetKey(anchoredRotateKey))
        {
            transform.RotateAround(transform.position, transform.right, mouseMoveY * -rotationSpeed);
            transform.RotateAround(transform.position, Vector3.up, mouseMoveX * rotationSpeed);
        }

        transform.Translate(move);

        //Scroll to zoom
        float mouseScroll = Input.GetAxis(zoomAxis);
        transform.Translate(Vector3.forward * mouseScroll * zoomSpeed);
    }
}