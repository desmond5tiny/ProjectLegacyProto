using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_characterButton : MonoBehaviour
{
    [HideInInspector]
    public PlayerUnit unit;
    private UI_CharacterMenu charMenu;

    public void Initialize(UI_CharacterMenu _menu, PlayerUnit _unit)
    {
        charMenu = _menu;
        unit = _unit;
        name = unit.name;
    }

    public void SetProfile()
    {
        charMenu.ChangeProfile(unit);
    }
}
