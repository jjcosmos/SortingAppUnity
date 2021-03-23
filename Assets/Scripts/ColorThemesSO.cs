using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorTheme", menuName = "ScriptableObjects/ThemeSO", order = 1)]
public class ColorThemesSO : ScriptableObject
{
    [SerializeField] public List<ColorTheme> themes;
}
