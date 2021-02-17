using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class UI_InteractableMenu : MonoBehaviour
{
    public Transform taskMenu;
    public Button repairButton;
    //public Transform taskPanel;
    //public Button roundButton;
    //public Interactable interactable;

    private InputManager inputManager;
    private UnitManager unitManager;
    private bool onHitBox = false;
    private bool setInactive = false;

    private void Awake()
    {
        if (repairButton != null) { repairButton.gameObject.SetActive(false); }
        taskMenu.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        InputManager.OnRightMouseUp += EnableMenu;
        onHitBox = true;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        unitManager = UnitManager.Instance;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            if (repairButton != null)
            {
                if (!repairButton.IsActive() && transform.GetComponent<RepairTask>().structureDamaged()) { repairButton.gameObject.SetActive(true); }
                else if (repairButton.IsActive() && !transform.GetComponent<RepairTask>().structureDamaged()) { repairButton.gameObject.SetActive(false); }
            }
            if (setInactive) 
            {
                StopCoroutine("DisableMenu"); 
                setInactive = false;
            }
            return; 
        }

        if (!setInactive && !EventSystem.current.IsPointerOverGameObject() && !onHitBox) { StartCoroutine("DisableMenu"); setInactive = true; }
    }

    public void SetTaskButtons() //dynamically fill the taskpanel with buttons
    {
        //grab all script with ITask interface component

        //add button for each task to taskPanel
            //grab icon from taskScript
    }

    public void EnableMenu()
    {
        taskMenu.gameObject.SetActive(true);

        //set rotation to face camera
        Vector3 direction = (taskMenu.transform.position - Camera.main.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        taskMenu.transform.rotation = lookRotation;

        Vector3 mouseClickPos = inputManager.RaycastAll().point;
        taskMenu.transform.position = new Vector3(transform.position.x, mouseClickPos.y + 1.5f, transform.position.z);
        //onHitBox = true;
    }

    IEnumerator DisableMenu()
    {
        yield return new WaitForSeconds(.1f);
        taskMenu.gameObject.SetActive(false);
        StopCoroutine("DisableMenu");
    }

    private void OnMouseExit()
    {
        onHitBox = false;
        InputManager.OnRightMouseUp -= EnableMenu;
    }

    private void OnDisable()
    {
        InputManager.OnRightMouseUp -= EnableMenu;
    }
}
