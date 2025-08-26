using System.Collections.Generic;
using Assets.DataSet;
using DataSet;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Enemy enemy;
    public WeaponSelectUI ui;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SLASH!");
            enemy.OnHit(AttackType.Slash);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("BLUDGEON!");
            enemy.OnHit(AttackType.Bludgeon);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PIERCE");
            enemy.OnHit(AttackType.Pierce);
        }
        
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Select");
            List<WeaponData> selected = new List<WeaponData>(new WeaponData[3]);
            selected[0] = GameManager.instance.GetWeaponData("Sword");
            selected[1] = GameManager.instance.GetWeaponData("IceStaff");
            selected[2] = GameManager.instance.GetWeaponData("Hammer");
            ui.SetWeaponList(selected);
            ui.BuildWeapons();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ui.ChangeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ui.ChangeWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ui.ChangeWeapon(3);
        }
    }
}
