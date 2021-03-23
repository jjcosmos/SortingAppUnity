using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassNameUpdater : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField classInputField;

    private void Awake() {
        StaticFileIO.GetOnDataLoadedEvent().AddListener(this.RefreshText);
    }

    private void RefreshText()
    {
        classInputField.text = StaticFileIO.GetActiveData().GetActiveClass().GetClassName();
    }

    public void UpdateClassName()
    {
        StaticFileIO.GetActiveData().GetActiveClass().SetClassName(classInputField.text);
        StaticFileIO.SaveActiveData();
    }
}
