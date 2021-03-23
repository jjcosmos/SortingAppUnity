using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] string sName;
    
    public void LoadScene()
    {
        AsyncSceneLoader.LoadSceneByName(sName);
    }
}
