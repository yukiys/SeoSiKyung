using System.Collections.Generic;
using DataSet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    public WeaponSelectionButton[] buttons;
    public static System.Action<int, bool> onClickWeapon; // (index, isSelected)

    int selectedCount = 0;
    const int MaxSelect = 3;

    void OnEnable()
    {
        onClickWeapon += HandleSelect;
    }

    void OnDisable()
    {
        onClickWeapon -= HandleSelect;
    }

    void HandleSelect(int index, bool wantSelect)
    {
        if (wantSelect)
        {
            if (selectedCount >= MaxSelect)
            {
                buttons[index].SetSelected(false);
                return;
            }
            selectedCount++;
        }
        else
        {
            selectedCount--;
        }
    }
}
