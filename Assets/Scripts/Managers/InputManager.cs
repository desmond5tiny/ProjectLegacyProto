using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of InputManager found!"); }
    }
    #endregion

    public LayerMask groundLayer;
    private Vector3 mouseLeftDownPos, mouseLeftUpPos;
    private Vector3 mouseRightDownPos, mouseRightUpPos;
    public Camera mainCam;

    [HideInInspector]
    public bool buildMode;

    private UnitManager unitManager;

    public ItemData wood;
    public enum InputMode { SelectMode, BuildMode, UnitSelected, BuildingSelected, GroupSelected}
    [HideInInspector]
    private InputMode inputMode;
    public enum DragMode { NoDrag, LeftDrag, RightDrag}
    [HideInInspector]
    private DragMode dragMode;

    public static Action<InputMode> OnLeftMouseClick;
    public static Action OnRightMouseClick;
    public static Action<DragMode> OnLeftMouseDrag;
    public static Action OnRightMouseHold;
    public static Action OnLeftMouseUp, OnRightMouseUp;
    public static Action OnKeyT, OnKeyN;

    void Start()
    {
        unitManager = UnitManager.Instance;
        buildMode = false;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        CheckClickDown();
        CheckClickUp();
        CheckMouseDrag();
        CheckKeyDown();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 5000.0f, groundLayer);

            GameObject woodLogs = new GameObject(wood.name);
            woodLogs.AddComponent<Item>().SetItemData(wood);
            woodLogs.transform.position = hit.point;
        }

        if (Input.GetKeyDown(KeyCode.U)) //temp spawn units
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 5000.0f,groundLayer);
            unitManager.SpawnUnit(hit.point);
        }
    }

    private void CheckKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.T)) { OnKeyT?.Invoke(); }

        if (Input.GetKeyDown(KeyCode.N)) { OnKeyN?.Invoke(); }
    }

    private void CheckClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLeftDownPos = Input.mousePosition;
            var pos = RaycastGround();
            if (pos != null) { OnLeftMouseClick?.Invoke(inputMode); }
        }
        if (Input.GetMouseButtonDown(1))
        {
            mouseRightDownPos = Input.mousePosition;
            var pos = RaycastAll();
            if (pos != null) { OnRightMouseClick?.Invoke(); }
        }
    }

    private void CheckClickUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseLeftUpPos = Input.mousePosition;

            OnLeftMouseUp?.Invoke();

            dragMode = DragMode.NoDrag;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouseRightUpPos = Input.mousePosition;

            OnRightMouseUp?.Invoke();

            dragMode = DragMode.NoDrag;
            inputMode = InputMode.SelectMode;
        }
    }
    private void CheckMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            if (dragMode != DragMode.LeftDrag && (mouseLeftDownPos - Input.mousePosition).magnitude > 20)
            {
                dragMode = DragMode.LeftDrag;
                OnLeftMouseDrag?.Invoke(dragMode);
            }
        }
    }

    public DragMode GetDragMode() { return dragMode; }
    
    public InputMode GetInputMode() { return inputMode; }

    public void SetInputMode(InputMode mode) { inputMode = mode; }
    
    public Vector3 GetMouseLDownPos() { return mouseLeftDownPos; }

    public Vector3 GetMouseLUpPos() { return mouseLeftUpPos; }

    public  Vector3? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 5000.0f, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            return hitPoint;
        }
        return null;
    }
    
    public Vector3 RaycastAll()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 5000.0f, groundLayer);

        Vector3 hitPoint = hit.point;
        return hitPoint;
    }


    /*private void OnGUI()
    {
        if (dragSelect)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }*/
}
