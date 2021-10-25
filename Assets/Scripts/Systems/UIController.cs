using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject upgradeMenu;
    public GameObject shopMenu;
    public enum Menus
    {
        UpgradeMenu,
        ShopMenu
    }

    public Menus menus;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void EnableUpdateMenu()
    {
        upgradeMenu.SetActive(true);
    }

    public void DisableUpdateMenu()
    {
        upgradeMenu.SetActive(false);
    }

    public void SetMenuActive()
    {
        switch (menus)
        {
            case Menus.UpgradeMenu:
                upgradeMenu.SetActive(true);

                break;
            case Menus.ShopMenu:
                shopMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetMenuInactive()
    {
        switch (menus)
        {
            case Menus.UpgradeMenu:
                upgradeMenu.SetActive(false);
                break;
            case Menus.ShopMenu:
                shopMenu.SetActive(false);
                break;
            default:
                break;
        }
    }

}

[CustomEditor(typeof(UIController))]
public class UIControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        UIController uIcontroller = (UIController) target;
        DrawDefaultInspector();
        if (GUILayout.Button("Enable"))
        {
            uIcontroller.SetMenuActive();
        }

        if (GUILayout.Button("Disable"))
        {
            uIcontroller.SetMenuInactive();
        }
    }

}


