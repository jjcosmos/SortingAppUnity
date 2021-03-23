using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelHelper : MonoBehaviour
{
    [SerializeField] List<ThemePalatte> themePalattes;
    [SerializeField] Toggle showWarningsToggle;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMPro.TMP_InputField winStartingScaleInput;
    private void Awake() 
    {
        StaticFileIO.GetOnDataLoadedEvent().AddListener(LoadInAllUI);
        LoadInAllUI();
    }

    public void LoadInAllUI()
    {
        showWarningsToggle.isOn = StaticFileIO.GetActiveData().bRequireConfirmation;
        fullscreenToggle.isOn = StaticFileIO.GetActiveData().bFullscreen;
        winStartingScaleInput.text = StaticFileIO.GetActiveData().fDefaultGroupWindowScale.ToString();
    }

    public void ValidateAllUI()
    {
        StaticFileIO.GetActiveData().bRequireConfirmation = showWarningsToggle.isOn;
        StaticFileIO.GetActiveData().bFullscreen = fullscreenToggle.isOn;
        float scaleInput;
        if (float.TryParse(winStartingScaleInput.text, out scaleInput)){
            scaleInput = Mathf.Clamp(scaleInput, 1f, 2f);
            StaticFileIO.GetActiveData().fDefaultGroupWindowScale = scaleInput;
            winStartingScaleInput.text = StaticFileIO.GetActiveData().fDefaultGroupWindowScale.ToString();
        }
        
        StaticFileIO.SaveActiveData();
        StaticFileIO.Load();
    }

    private void Start() {
        int index = 0;
        foreach(var t in themePalattes)
        {
            t.themeIndex = index;
            t.DrawColors();
            index ++;
        }
    }
}
