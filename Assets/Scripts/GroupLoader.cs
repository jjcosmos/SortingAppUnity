using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroupLoader : MonoBehaviour
{

    [SerializeField] StudentUIElement studentUIElementPrefab;
    [SerializeField] GroupCanvas moveableCanvasPrefab;
    [SerializeField] Transform canvasParent;
    [SerializeField] int MaxHCount;
    [SerializeField] float hSpacing;
    [SerializeField] float vSpacing;
    [SerializeField] Vector2 worldOffset;
    private static readonly float over12Shrink = .9f;
    
    int realNumGroups;
    int targetNumgroups;
    List<StudentDataset> loadedStudents;
    List<StudentDataset> copiedStudents;
    List<StudentGridPanel> renderedPanels;
    List<GroupCanvas> groupCanvass;

    private static GroupLoader _inst;

    private void Awake() {
        _inst = this;
        StaticFileIO.GetOnDataLoadedEvent().AddListener(this.OnFileLoaded);
        renderedPanels = new List<StudentGridPanel>();
        groupCanvass = new List<GroupCanvas>();
    }

    private void OnFileLoaded()
    {
        CalcNumGroups();
        ShuffleInCopy();
        DistributeToGridPanels();
    }

    public static void RefreshAll()
    {
        if(_inst.renderedPanels == null) return;
        foreach(var p in _inst.renderedPanels)
        {
            p.PopulateGrid();
        }
    }

    private void CalcNumGroups()
    {
        targetNumgroups = StaticFileIO.GetActiveData().numGroups;
        loadedStudents = StaticFileIO.GetActiveData().GetActiveClass().myStudents;
        if(StaticFileIO.GetActiveData().keepExtrasSeparate)
        {
            if (loadedStudents.Count % targetNumgroups != 0)
            {
                realNumGroups = targetNumgroups + 1;
            }
            else
            {realNumGroups = targetNumgroups;}
        }
        else
        {
            realNumGroups = targetNumgroups;
        }
        Debug.Log($"target num groups is {targetNumgroups}, real is {realNumGroups}. file says {StaticFileIO.GetActiveData().numGroups}");
    }

    private void ShuffleInCopy()
    {
        copiedStudents = new List<StudentDataset>();
        foreach(var s in loadedStudents)
        {
            copiedStudents.Add(s);
        }
        copiedStudents = copiedStudents.OrderBy(x => Random.value).ToList();
    }

    private void DistributeToGridPanels()
    {
        int maxPerGroup = copiedStudents.Count / targetNumgroups;
        int currentStudentIndex = 0;
        int canvasesCreated = 0;

        for (int i = 0; i < targetNumgroups; i++)
        {
            GroupCanvas newCanvas = Instantiate(moveableCanvasPrefab, canvasParent);
            newCanvas.SetGroupTitle(i.ToString());
            canvasesCreated ++;
            renderedPanels.Add(newCanvas.gridPanel);
            groupCanvass.Add(newCanvas);

            int j;
            for(j = 0; j < maxPerGroup; j++)
            {
                newCanvas.gridPanel.overrideData.Add(copiedStudents[currentStudentIndex]);
                currentStudentIndex ++;
            }
            // if not keep extras separate and student count is less than it should be
            if(i == targetNumgroups - 1 && StaticFileIO.GetActiveData().keepExtrasSeparate == false && currentStudentIndex < copiedStudents.Count)
            {
                Debug.LogError("Adding exras to group");
                while(currentStudentIndex < copiedStudents.Count)
                {
                    newCanvas.gridPanel.overrideData.Add(copiedStudents[currentStudentIndex]);
                    currentStudentIndex++;
                }
            }
            newCanvas.gridPanel.PopulateGrid();
        }
        //Should Only hit this if the number is uneven
        if(canvasesCreated < realNumGroups)
        {
            Debug.LogError("Hit uneven canvas");
            GroupCanvas newCanvas = Instantiate(moveableCanvasPrefab, canvasParent);
            newCanvas.SetGroupTitle("Extras");
            renderedPanels.Add(newCanvas.gridPanel);
            groupCanvass.Add(newCanvas);
            //Is last, feel free to add extras
            while (currentStudentIndex < copiedStudents.Count)
            {
                newCanvas.gridPanel.overrideData.Add(copiedStudents[currentStudentIndex]);
                currentStudentIndex++;
            }
            newCanvas.gridPanel.PopulateGrid();
        }

        int counter = 0;
        float adjustedScalar = (realNumGroups > 11) ? over12Shrink : 1;
        foreach(GroupCanvas g in groupCanvass)
        {
            Vector2 newPos;
            newPos.x = counter % MaxHCount * hSpacing * adjustedScalar;
            newPos.y = counter/MaxHCount * -vSpacing * adjustedScalar * ((realNumGroups > 12) ? .8f : 1f);
            g.GetComponent<RectTransform>().anchoredPosition = newPos + worldOffset;
            counter++;
        }
    }
}
