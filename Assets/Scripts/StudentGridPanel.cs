using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentGridPanel : DynamicDisplayGrid
{
    [SerializeField] StudentUIElement studentUIElementPrefab;
    [SerializeField] float defaultScale = 1;
    [SerializeField] bool hideInteractiveElements = false;
    [HideInInspector] public List<StudentDataset> overrideData;
    [SerializeField] bool setSpawnedAsDraggable = false;
 
    public override void PopulateGrid()
    {
        base.PopulateGrid();
        List<StudentDataset> students;

        if(overrideData != null && (overrideData.Count > 0 || setSpawnedAsDraggable))
        {
            students = overrideData;
        }
        else
        {
            students = StaticFileIO.GetActiveData().GetActiveClass().myStudents;
        }
        
        if (students == null || students.Count < 1) return;
        foreach (var s in students)
        {
            StudentUIElement e = GameObject.Instantiate(studentUIElementPrefab, this.transform);
            e.transform.localScale = Vector2.one * defaultScale;
            e.MyOwner = this;
            e.SetStudentDataRef(s);
            e.LoadInfoFromData();
            e.isDraggable = setSpawnedAsDraggable;
            if(hideInteractiveElements) e.HideInteractiveElements();
        }
    }

    public void Swap(StudentDataset studentToSwapIn, StudentDataset studentToSwapOut, StudentGridPanel requestingPanel)
    {
        if(overrideData.Contains(studentToSwapOut))
        {
            requestingPanel.overrideData.Add(studentToSwapOut);
            overrideData.Remove(studentToSwapOut);
            overrideData.Add(studentToSwapIn);
            requestingPanel.overrideData.Remove(studentToSwapIn);
            //Ask to refresh all
        }
    }

    public void MoveToMyPanel(StudentDataset studentToMove, StudentGridPanel requestingPanel)
    {
        overrideData.Add(studentToMove);
        if(requestingPanel.overrideData.Contains(studentToMove))
        {
            Debug.Log($"Requesting panel data set is");
            PrintDataSet(requestingPanel.overrideData);
            requestingPanel.overrideData.Remove(studentToMove);
            Debug.Log($"Requesting panel data set after removal is");
            PrintDataSet(requestingPanel.overrideData);
        }
        else{
            Debug.Log("data does not exist");
        }
        Debug.Log("moving to another panel");
        //ask to refresh all
    }

    private void PrintDataSet(List<StudentDataset> dat)
    {
        Debug.Log("__________________");
        foreach(var i in dat)
        {
            Debug.Log(i);
        }
    }
}
