using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemePalatte : MonoBehaviour
{
    public int themeIndex;

    public void DrawColors()
    {
        transform.GetChild(0).GetComponent<Image>().color = ColorThemer.GetColorThemes()[themeIndex].ColorBase;
        transform.GetChild(1).GetComponent<Image>().color = ColorThemer.GetColorThemes()[themeIndex].ColorSecondary;
        transform.GetChild(2).GetComponent<Image>().color = ColorThemer.GetColorThemes()[themeIndex].ColorTertiary;
        transform.GetChild(3).GetComponent<Image>().color = ColorThemer.GetColorThemes()[themeIndex].ColorFour;
    }

    public void SetSelfAsCurrent()
    {
        ColorThemer.SetNewTheme(themeIndex);
    }
}
