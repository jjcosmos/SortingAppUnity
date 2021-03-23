using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StudentDataset
{
    [SerializeField] int hash;
    [SerializeField] string studentName;

    public StudentDataset(string name)
    {
        studentName = name;
        hash = Random.Range(0, int.MaxValue);
    }

    public void SetName(string newName)
    {
        studentName = newName;
    }

    public int GetHash()
    {
        return hash;
    }

    public string GetName()
    {
        return studentName;
    }
}
