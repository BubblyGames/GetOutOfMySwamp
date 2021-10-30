using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelector : MonoBehaviour
{
    public Color lightColor = Color.white;
    public Color backGroundColor = Color.black;
    public ThemeInfo GetThemeInfo()
    {
        ThemeInfo ti = new ThemeInfo();
        ti.lightColor = lightColor;
        ti.backGroundColor = backGroundColor;
        ti.material = GetComponent<MeshRenderer>().material;
        return ti;
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
            return;

        FindObjectOfType<Light>().color = lightColor;
        RenderSettings.skybox.SetColor("_Tint", backGroundColor);
    }
}
