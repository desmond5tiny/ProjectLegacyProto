using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_CharacterMenu : MonoBehaviour
{
    public GameObject characterButton;
    public GameObject buttonContainer;

    public Image unitProfile;
    public TextMeshProUGUI nameStat;
    public TextMeshProUGUI ageStat;
    public TextMeshProUGUI genderStat;
    public TextMeshProUGUI healthStat;
    public TextMeshProUGUI lvlStat;
    public TextMeshProUGUI xpStat;

    private Dictionary<int, GameObject> buttonDict = new Dictionary<int, GameObject>();

    private void OnEnable()
    {
        UnitManager.PopulationIncreased += AddButton;
        UnitManager.PopulationDecreased += RemoveButton;

        UpdateButtonContainer();

        if (GlobalSelection.Instance.selectionDictionary.selectedDict != null)
        {
            if (GlobalSelection.Instance.selectionDictionary.selectedDict.Count == 1)
            {
                foreach (KeyValuePair<int, GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
                {
                    ChangeProfile(pair.Value.GetComponent<PlayerUnit>());
                    //scroll to unitButton and set selected
                } 
            }
        }
    }

    private void OnDisable()
    {
        UnitManager.PopulationIncreased -= AddButton;
        UnitManager.PopulationDecreased -= RemoveButton;
    }

    public void ChangeProfile(PlayerUnit _unit)
    {
        var stats = _unit.stats;
        nameStat.text = stats.unitName;
        ageStat.text = stats.age.ToString();
        genderStat.text = stats.gender;
        healthStat.text = stats.currentHealth.ToString();
        lvlStat.text = stats.currentLevel.ToString();
    }

    public void AddButton(PlayerUnit _unit)
    {
        GameObject unitButton = Instantiate(characterButton, buttonContainer.transform);
        unitButton.GetComponent<UI_characterButton>().Initialize(this, _unit);
        Debug.Log("add bdict:" + _unit.gameObject.GetInstanceID());
        buttonDict.Add(_unit.gameObject.GetInstanceID(), unitButton);
    }
    
    public void RemoveButton(PlayerUnit _unit)
    {
        buttonDict.Remove(_unit.gameObject.GetInstanceID());
    }

    public void UpdateButtonContainer()
    {
        if(buttonDict.Count < UnitManager.Instance.unitDict.Count)
        {
            foreach(var pair in UnitManager.Instance.unitDict)
            {
                if (!buttonDict.ContainsKey(pair.Key))
                {
                    Debug.Log("pair key " + pair.Key);
                    AddButton(pair.Value.GetComponent<PlayerUnit>());
                }
            }
        }
        if (buttonDict.Count > UnitManager.Instance.unitDict.Count)
        {
            foreach(var pair in buttonDict)
            {
                if (!UnitManager.Instance.unitDict.ContainsKey(pair.Key))
                {
                    buttonDict.Remove(pair.Key);
                }
            }
        }
    }
}
