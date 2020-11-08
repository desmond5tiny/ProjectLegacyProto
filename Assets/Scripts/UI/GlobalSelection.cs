using UnityEngine;
public class GlobalSelection : MonoBehaviour
{
    #region Singleton
    public static GlobalSelection Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of GlobalSelection found!"); }
    }
    #endregion

    private InputManager inputManager;

    public LayerMask groundLayer;
    [HideInInspector]
    public SelectionDictionary selectionDictionary;
    private RaycastHit hit;

    //collider vars
    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector2[] corners; //corners of the 2d selection
    Vector3[] verts; //vertices of the meshcollider

    private void OnEnable()
    {
        InputManager.OnLeftMouseUp += MakeSelection;
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        selectionDictionary = GetComponent<SelectionDictionary>();
    }

    public void MakeSelection( )
    {
        if(inputManager.GetInputMode() == InputManager.InputMode.BuildMode) { return; }

        Vector3 mouseDownClickPos = inputManager.GetMouseLDownPos();
        Vector3 mouseUpClickPos = inputManager.GetMouseLUpPos();

        if (inputManager.GetDragMode() != InputManager.DragMode.LeftDrag) //single select
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseDownClickPos);
            bool isHit = Physics.Raycast(ray, out hit, 50000.0f);
            //Debug.Log("hit: "+ hit.transform.name);

            if (isHit && hit.transform.CompareTag("Unit"))
            {
                if (Input.GetKey(KeyCode.LeftShift)) // add to selection
                {
                    selectionDictionary.addSelected(hit.transform.gameObject);
                    if (selectionDictionary.selectedDict.Count > 1) { inputManager.SetInputMode(InputManager.InputMode.GroupSelected); }
                    else { inputManager.SetInputMode(InputManager.InputMode.UnitSelected); }
                    //Debug.Log("add single selection");
                }
                else // exclusive selected
                {
                    selectionDictionary.deselectAll();
                    selectionDictionary.addSelected(hit.transform.gameObject);
                    inputManager.SetInputMode(InputManager.InputMode.UnitSelected);
                    //Debug.Log("excl single select");
                }
            }
            else if (isHit && hit.transform.CompareTag("Building")) //building select && hit.transform.GetComponent<Unit>() == null
            {
                //nothing yet
                Debug.Log("build structure");
            }
            else //deselect all when nothing is hit
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    //Debug.Log("do nothing");
                }
                else
                {
                    selectionDictionary.deselectAll();
                    inputManager.SetInputMode(InputManager.InputMode.SelectMode);
                    //Debug.Log("deselect all");
                }
            }
            isHit = false;
        }
        else
        {
            verts = new Vector3[4];
            int i = 0;
            //p2 = Input.mousePosition;
            corners = getBoundingBox(mouseDownClickPos, mouseUpClickPos);

            foreach (Vector2 corner in corners)
            {
                Ray ray = Camera.main.ScreenPointToRay(corner);

                if (Physics.Raycast(ray, out hit, 50000.0f, groundLayer))
                {
                    verts[i] = new Vector3(hit.point.x, 0, hit.point.z);
                    Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                }
                i++;
            }
            //generate the mesh
            selectionMesh = GenerateSelectionMesh(verts);

            selectionBox = gameObject.AddComponent<MeshCollider>();
            selectionBox.sharedMesh = selectionMesh;
            selectionBox.convex = true;
            selectionBox.isTrigger = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                selectionDictionary.deselectAll();
                inputManager.SetInputMode(InputManager.InputMode.SelectMode);
            }

            Destroy(selectionBox, 0.02f);
            
            if (selectionDictionary.selectedDict.Count > 1) { inputManager.SetInputMode(InputManager.InputMode.GroupSelected); }
            else { inputManager.SetInputMode(InputManager.InputMode.UnitSelected); }
        }
    }

    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) // p1 left of p2
        {
            if (p1.y > p2.y) // p1 above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else // p1 below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else // p1 right of p2
        {
            if (p1.y > p2.y) // p1 above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }
        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }

    //generate a mesh from the bottom  4 points
    Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map tris to cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100.0f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            selectionDictionary.addSelected(other.gameObject);
        }
    }

    private void OnGUI()
    {
        if (inputManager.GetDragMode() == InputManager.DragMode.LeftDrag && inputManager.GetInputMode() != InputManager.InputMode.BuildMode)
        {
            var rect = Utils.GetScreenRect(inputManager.GetMouseLDownPos(), Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
