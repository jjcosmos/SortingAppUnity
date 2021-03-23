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

    public void OnLoadFromBackupsPressed()
    {
        ConfirmationDialogue.AwaitConfirmation(OnConfirmLoadFromBackup,() => { }, "Overwrite current class data from a backup?");
    }

    private void OnConfirmLoadFromBackup()
    {
        string json = GUIUtility.systemCopyBuffer;
        List<ClassDataset> newClassData = ClassDataOnly.DeSerialize(json);
        if(newClassData!=null)
        {
            StaticFileIO.GetActiveData().loadedClasses = newClassData;
        }
        else
        {
            Debug.Log("Failed");
            ConfirmationDialogue.AwaitConfirmation(()=>{}, () => { }, "Backup could not be loaded. Make sure the entire .gdat file is copied to your clipboard.");
        }
    }

    public void OnClearUserDataPressed()
    {
        ConfirmationDialogue.AwaitConfirmation(OnConfirmDeleteUserData, ()=>{}, "You are about to remove all application data from the registry. This does not remove backups. Continue?");
    }

    private void OnConfirmDeleteUserData()
    {
        PlayerPrefs.DeleteAll();
        StaticFileIO.Load();
        ColorThemer.RefreshAll();
    }
}
