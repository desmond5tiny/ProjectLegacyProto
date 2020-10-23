using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCam;
    public float panSpeed = 20f;
    public float scrollSpeed = 20f;
    public float panBorderThickness = 10;
    public float zoomMin = 20f;
    public float zoomMax = 120f;

    public Vector2 moveLimit;
    public bool mousePan;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w")|| Input.mousePosition.y>= Screen.height-panBorderThickness && mousePan)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness && mousePan)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness && mousePan)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness && mousePan)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        //limit pan area of camera
        //pos.x = Mathf.Clamp(pos.x, -moveLimit.x, moveLimit.x);
        //pos.z = Mathf.Clamp(pos.z, -moveLimit.y, moveLimit.y);

        transform.position = pos;
        float posY = mainCam.transform.localPosition.y;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        posY -= scroll * scrollSpeed * 100f* Time.deltaTime;
        posY = Mathf.Clamp(posY, zoomMin, zoomMax);
        mainCam.transform.localPosition = new Vector3(mainCam.transform.localPosition.x,posY, mainCam.transform.localPosition.z);
    }
}
