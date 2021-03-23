using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicDisplayGrid : MonoBehaviour
{
    [SerializeField] protected GridLayoutGroup displayGrid;
    public int maxElements = 16;

    void Awake()
    {
        StaticFileIO.GetOnDataLoadedEvent().AddListener(this.OnDataLoaded);
    }

    protected virtual void OnDataLoaded()
    {
        PopulateGrid();
    }

    public virtual void PopulateGrid()
    {
        ClearGrid();
    }

    protected void ClearGrid()
    {
        if(transform.childCount == 0) return;
        while(transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.parent = null;
            GameObject.Destroy(t.gameObject);
        }
    }

   

}
