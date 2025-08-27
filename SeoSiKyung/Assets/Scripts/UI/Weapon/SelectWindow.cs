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
    Canvas canvas;
    WeaponSelectUI ui;
    Button confirmButton;
    Image confirmImg;
    List<WeaponData> selectedWeapons = new List<WeaponData>();

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        ui = canvas.GetComponentInChildren<WeaponSelectUI>();
        confirmButton = GetComponentInChildren<Button>();
        confirmButton.interactable = false;
        confirmImg = confirmButton.GetComponent<Image>();
        confirmImg.color = new Color(1f, 1f, 1f, 0.4f);
    }
    void OnEnable() { onClickWeapon += HandleSelect; }

    void OnDisable() { onClickWeapon -= HandleSelect; }

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
            selectedWeapons.Add(GameManager.instance.weaponDataList[index]);
        }
        else
        {
            selectedCount--;
            selectedWeapons.Remove(GameManager.instance.weaponDataList[index]);
        }

        UpdateConfirm();
    }

    void UpdateConfirm()
    {
        bool ready = (selectedCount == MaxSelect);

        confirmButton.interactable = ready;
        Color c = confirmImg.color;
        c.a = ready ? 1f : 0.4f;
        confirmImg.color = c;
    }

    public void ConfirmSelection()
    {
        if (selectedCount == MaxSelect)
        {
            ui.SetWeaponList(selectedWeapons);
            ui.BuildWeapons();

            GameManager.instance.selectedWeaponList = new List<WeaponData>(selectedWeapons);
            GameManager.instance.curWeaponIdx = 0;

            ui.ChangeWeapon(1);

            gameObject.SetActive(false);
        }
    }
}