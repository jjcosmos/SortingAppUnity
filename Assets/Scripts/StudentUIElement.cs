using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentUIElement : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField studentNameTXT;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;
    [SerializeField] EditableNameElement editableNameElement;

    private StudentDataset myData;
    public bool isDraggable = false;
    public DynamicDisplayGrid MyOwner { get; set; }//Will not be a gameobject in the future

    public static GroupCanvas currentMouseCanvas;
    public static StudentUIElement currentMouseStudentUIElement;
    public static StudentUIElement draggingUIElement;


    public void SetStudentDataRef(StudentDataset studentDataset)
    {
        myData = studentDataset;
    }

    public void LoadInfoFromData()
    {
        if (myData == null) return;
        studentNameTXT.text = myData.GetName();
    }

    public void HideInteractiveElements()
    {
        editButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        editableNameElement.SetInputWriteable(false);
        studentNameTXT.interactable = false;
    }

    public void OnEditableFinishedEditing()
    {
        myData.SetName(studentNameTXT.text);
        StaticFileIO.SaveActiveData();
    }

    public void MyOnEditClicked()
    {
    }

    public void MyOnDeleteClicked()
    {
        ConfirmationDialogue.AwaitConfirmation(this.ConfirmDelete, this.CancelDelete, $"You are deleting {studentNameTXT.text}. Continue?");
    }

    private void ConfirmDelete()
    {
        Debug.Log($"Deleting {studentNameTXT.text} ({myData.GetHash()})");
        StaticFileIO.GetActiveData().RemoveStudentFromActiveClass(myData.GetHash());
        MyOwner.PopulateGrid();
    }

    private void CancelDelete()
    {
        Debug.Log($"Canceled deleting {studentNameTXT.text} (not implemeted)");
    }

    public void OnDrag()
    {
        if(!isDraggable) return;
        Debug.Log($"Dragging {draggingUIElement.myData.GetName()}");

        if(currentMouseCanvas != null && currentMouseStudentUIElement == null)
        {
            transform.SetParent(currentMouseCanvas.gridPanelRect);
        }
        else
        {
            transform.SetParent(MyOwner.transform);
        }
    }

    public void OnDragBegin()
    {
        draggingUIElement = this;
    }

    public void OnDragEnd()
    {
        if(draggingUIElement == null) return;
        if(currentMouseStudentUIElement != null && currentMouseCanvas != null)
        {
            currentMouseCanvas.gridPanel.Swap(myData, currentMouseStudentUIElement.myData, MyOwner.GetComponent<StudentGridPanel>());
        }
        else if(currentMouseCanvas != null)
        {
            currentMouseCanvas.gridPanel.MoveToMyPanel(myData, MyOwner.GetComponent<StudentGridPanel>());
        }
        GroupLoader.RefreshAll();
        draggingUIElement = null;
    }

    public void OnMouseOver()
    {
        if(draggingUIElement == this) return;
        if(draggingUIElement != null)
        {
            currentMouseStudentUIElement = this;
            Debug.Log($"Mousing over new element for swapping ({this.myData.GetName()})");
        }
    }

    public void OnMouseExit() {
        if (draggingUIElement == this) return;
        currentMouseStudentUIElement = null;
        Debug.Log("Mouse exit " + this.myData.GetName());
    }
}
