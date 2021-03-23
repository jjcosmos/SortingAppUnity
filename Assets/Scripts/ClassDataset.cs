using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClassDataset
{
    [SerializeField] int hash;
    [SerializeField] string className;
    [SerializeField] public List<StudentDataset> myStudents;

    public ClassDataset(string className)
    {
        hash = Random.Range(0, int.MaxValue);
        this.className = className;
        myStudents = new List<StudentDataset>();
    }

    public string GetClassName()
    {
        return className;
    }

    public void SetClassName(string newClassName)
    {
        className = newClassName;
    }

    public int GetHash()
    {
        return hash;
    }
}
