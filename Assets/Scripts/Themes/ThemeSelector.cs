using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelector : MonoBehaviour
{
    public Theme theme;
    internal ThemeInfo themeInfo;
    private void Start()
    {
        themeInfo = ThemeManager.instace.GetTheme(theme);
        if (themeInfo == null)
        {
            Debug.Log("null");
        }
        GetComponent<MeshRenderer>().material = themeInfo.material;
        GetComponent<CubeWorldGenerator>().CreateWater();
    }
    public ThemeInfo GetThemeInfo()
    {
        return themeInfo;
    }

    /*private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        FindObjectOfType<Light>().color = lightColor;
        RenderSettings.skybox.SetColor("_Tint", backGroundColor);
    }*/
}
