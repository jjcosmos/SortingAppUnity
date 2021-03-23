using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewGridElementButton : MonoBehaviour
{
    [SerializeField] EAdditionType additionType;
    [SerializeField] DynamicDisplayGrid grid;
    public void MyOnClick()
    {
        if(grid.transform.childCount >= grid.maxElements)
        {
            ConfirmationDialogue.AwaitConfirmation(()=>{}, ()=>{},$"There is currently a max of {grid.maxElements} elements allowed.");
            return;
        }
        switch (additionType)
        {
            case EAdditionType.typeClass:
                StaticFileIO.GetActiveData().AddClass(new ClassDataset("New Class"));
                break;
            case EAdditionType.typeStudent:
                StaticFileIO.GetActiveData().AddStudentToActiveClass(new StudentDataset("New Student"));
                break;
        }
        StaticFileIO.SaveActiveData();
        grid.PopulateGrid();
    }
}

public enum EAdditionType {typeClass, typeStudent};