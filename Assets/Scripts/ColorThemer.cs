using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorThemer : MonoBehaviour
{
    //[SerializeField] List<ColorTheme> availibleThemes;
    [SerializeField] ColorThemesSO themeSet;

    public static UnityEvent OnUpdateTheme = new UnityEvent();
    private ColorTheme currentTheme;
    private static ColorThemer _inst;

    private void Awake() {
        _inst = this;
        StaticFileIO.GetOnDataLoadedEvent().AddListener(this.OnLoadFile);
    }
    private void Start() {
        
    }

    public static void RefreshAll()
    {
        _inst.OnLoadFile();
    }

    private void OnLoadFile()
    {
        Debug.Log("Themer on file load");
        if(StaticFileIO.GetActiveData().currentColorTheme == null)
        {
            SetNewTheme(0);
            Debug.Log("Setting Default Theme");
        }
        //ColorTheme realTheme = themeSet.themes.Find(x=>x.themeName == StaticFileIO.GetActiveData().currentColorTheme.themeName);
        //currentTheme = realTheme;
        //StaticFileIO.GetActiveData().currentColorTheme = realTheme;
        //StaticFileIO.SaveActiveData();
        //UpdateTheme();
        OnUpdateTheme.Invoke();
    }

    public void UpdateTheme()
    {
        StaticFileIO.GetActiveData().currentColorTheme = currentTheme;
        StaticFileIO.SaveActiveData();
        OnUpdateTheme.Invoke();
    }

    public static List<ColorTheme> GetColorThemes()
    {
        return _inst.themeSet.themes;
    }

    public static void SetNewTheme(int themeIndex)
    {
        _inst.currentTheme = _inst.themeSet.themes[themeIndex];
        _inst.UpdateTheme();
    }
}

[System.Serializable]
public class ColorTheme
{
    [SerializeField] public string themeName;
    [SerializeField][ColorUsage(true)] public Color ColorBase;
    [SerializeField][ColorUsage(true)] public Color ColorSecondary;
    [SerializeField][ColorUsage(true)] public Color ColorTertiary;
    [SerializeField][ColorUsage(true)] public Color ColorFour;
}