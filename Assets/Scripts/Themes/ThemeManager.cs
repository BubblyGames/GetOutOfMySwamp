using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager instace;
    public ThemeInfo[] themes;

    private void Awake()
    {
        if(instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ThemeInfo GetTheme(Theme theme)
    {
        return themes[((int)theme)];
    }
}

public enum Theme
{
    Forest = 0,
    Ice = 1,
    Autumn = 2,
    CherryBlossom = 3,
    Swamp = 4
}
