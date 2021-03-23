using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PasteButton : MonoBehaviour
{
    [SerializeField] DynamicDisplayGrid grid;
    public void MyOnClick()
    {
        string myNames = GUIUtility.systemCopyBuffer;
        List<string> slicedNames = new List<string>();
        if(!string.IsNullOrEmpty(myNames))
        {
            string[] result = myNames.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            slicedNames = result.ToList<string>();
            foreach(var s in slicedNames)
            {
                StaticFileIO.GetActiveData().GetActiveClass().myStudents.Add(new StudentDataset(s));
            }
            grid.PopulateGrid();
        }


    }
}
