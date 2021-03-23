using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSettingsPanel : MonoBehaviour
{
    [SerializeField] Toggle KESToggle;
    [SerializeField] TMPro.TMP_InputField numGroupsInputField;

    private void Awake() {
        StaticFileIO.GetOnDataLoadedEvent().AddListener(LoadInValues);
    }

    private void LoadInValues()
    {
        numGroupsInputField.text = StaticFileIO.GetActiveData().numGroups.ToString();
        KESToggle.isOn = StaticFileIO.GetActiveData().keepExtrasSeparate;
        OnNumGroupsChanged();
    }

    public void OnNumGroupsChanged()
    {
        int result = 0;
        if(int.TryParse(numGroupsInputField.text, out result))
        {
            if(result > 9){result = 9;}
            if(result < 1){result = 1;}
            StaticFileIO.GetActiveData().numGroups = result;
            StaticFileIO.SaveActiveData();
            numGroupsInputField.text = StaticFileIO.GetActiveData().numGroups.ToString();
        }
        else{
            ConfirmationDialogue.AwaitConfirmation(()=>{},()=>{}, "Value entered is not valid. Please try again.");
        }

    }

    public void OnKESChanged()
    {
        StaticFileIO.GetActiveData().keepExtrasSeparate = KESToggle.isOn;
        StaticFileIO.SaveActiveData();
    }
}
