using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float scrollSpeed = 20f;
    public float panBorderThickness = 10;
    public float zoomMin = 20f;
    public float zoomMax = 120f;

    public Vector2 moveLimit;
    public bool mousePan;
    void Start()
    {
        
    }

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

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        pos.y -= scroll * scrollSpeed * 100f* Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, zoomMin, zoomMax);
        //limit pan area of camera
        //pos.x = Mathf.Clamp(pos.x, -moveLimit.x, moveLimit.x);
        //pos.z = Mathf.Clamp(pos.z, -moveLimit.y, moveLimit.y);

        transform.position = pos;
    }
}
