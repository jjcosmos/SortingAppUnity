using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDataButton : MonoBehaviour
{
    [SerializeField] DynamicDisplayGrid linkedGrid;

    public void ClearLoadedStudents()
    {
        ConfirmationDialogue.AwaitConfirmation(FinishClearingStudents, ()=>{}, "You are about to delete all students in this class. Continue?");
    }

    private void FinishClearingStudents()
    {
        StaticFileIO.GetActiveData().GetActiveClass().myStudents.Clear();
        CleanUp();
    }
    
    public void ClearLoadedClasses()
    {
        StaticFileIO.GetActiveData().loadedClasses.Clear();
        CleanUp();
    }

    public void ClearAllUserData()
    {
        ClearLoadedClasses();
        CleanUp();
    }

    private void CleanUp()
    {
        StaticFileIO.SaveActiveData();
        if (linkedGrid)
        {
            linkedGrid.PopulateGrid();
        }
    }
}
