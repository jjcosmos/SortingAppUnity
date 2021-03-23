using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIElement : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text classNameTxt;
    [SerializeField] Button editButton;
    [SerializeField] Button openButton;
    [SerializeField] Button deleteButton;

    private ClassDataset myData;
    public DynamicDisplayGrid MyOwner { get; set; }//Will not be a gameobject in the future
    
    public void SetClassDataRef(ClassDataset classDataSet)
    {
        myData = classDataSet;
    }

    public void LoadInfoFromData()
    {
        if(myData == null) return;
        classNameTxt.text = myData.GetClassName();
    }

    public void MyOpenClicked()
    {
        //TODO Set some stuff in the dataLoader and load the grouper scene
        StaticFileIO.GetActiveData().SetClassAsFocused(myData.GetHash());
        int studentCount = StaticFileIO.GetActiveData().GetActiveClass().myStudents.Count;
        if(studentCount < StaticFileIO.GetActiveData().numGroups)
        {
            ConfirmationDialogue.AwaitConfirmation(LowerGroupCountAndProceed,()=>{}, $"Student count must be higher than group count. Group count will be lowered to {studentCount}.");
            return;
        }
        AsyncSceneLoader.LoadSceneByName("Grouping");
    }

    public void LowerGroupCountAndProceed()
    {
        StaticFileIO.GetActiveData().numGroups = StaticFileIO.GetActiveData().GetActiveClass().myStudents.Count;
        StaticFileIO.SaveActiveData();
        AsyncSceneLoader.LoadSceneByName("Grouping");
    }

    public void MyOnEditClicked() 
    {
        //TODO loads the class inspector scene with the specified class
        StaticFileIO.GetActiveData().SetClassAsFocused(this.myData.GetHash());
        AsyncSceneLoader.LoadSceneByName("ClassCreator");
    }

    public void MyOnDeleteClicked()
    {
        ConfirmationDialogue.AwaitConfirmation(this.ConfirmDelete, this.CancelDelete, $"You are deleting {classNameTxt.text}. Continue?");
    }

    private void ConfirmDelete()
    {
        Debug.Log($"Deleting {classNameTxt.text} ({myData.GetHash()})");
        StaticFileIO.GetActiveData().RemoveClass(myData.GetHash());
        MyOwner.PopulateGrid();
        //TODO notify owner
    }

    private void CancelDelete()
    {
        Debug.Log($"Canceled deleting {classNameTxt.text} (not implemeted)");
    }

}
