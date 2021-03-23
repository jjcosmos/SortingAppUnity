using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassGridPanel : DynamicDisplayGrid
{
    [SerializeField] ClassUIElement classUIElementPrefab;

    public override void PopulateGrid()
    {
        base.PopulateGrid();

        List<ClassDataset> classes = StaticFileIO.GetActiveData().loadedClasses;
        if(classes == null || classes.Count < 1) return;
        foreach(var classInst in classes)
        {
            ClassUIElement e = GameObject.Instantiate(classUIElementPrefab, this.transform);
            e.MyOwner = this;
            e.SetClassDataRef(classInst);
            e.LoadInfoFromData();
        }
    }
}
