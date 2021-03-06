using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;

public class StaticFileIO : MonoBehaviour
{
    private static FormattedJson activeData;
    private static UnityEvent OnDataLoaded = new UnityEvent();

    public static void ResetAllData()
    {
        activeData = new FormattedJson();
        Debug.LogError("Reset ALL data");
    }

    private void Start() 
    {
        AsyncSceneLoader.OnStartSceneLoad.AddListener(SaveActiveData);
        Load();
        OnDataLoaded.Invoke();

        ClassDataOnly cdo = new ClassDataOnly(activeData.loadedClasses);
        cdo.WriteToLocalDirectory();
    }

    public static UnityEvent GetOnDataLoadedEvent()
    {
        if(OnDataLoaded == null)
        {
            OnDataLoaded = new UnityEvent();
        }
        return OnDataLoaded;
    }

    public static void Load()
    {
        activeData = JsonUtility.FromJson<FormattedJson>(PlayerPrefs.GetString("FormattedJson", ""));
        if(activeData == null)
        {
            Debug.LogError("The file could not be loaded. Creating new.");
            activeData = new FormattedJson();
        }
        SetUIStatics();
        SetFullScreen(activeData.bFullscreen);
        Debug.LogWarning("Loaded.");
    }

    public static void SetFullScreen(bool fullScreenValue)
    {
        if (!fullScreenValue)
        {
            Screen.fullScreen = false;
            //Resolution resolution = Screen.currentResolution;
            //Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
        }
        else
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, fullScreenValue);
            Screen.fullScreen = fullScreenValue;
        }

    }

    public static FormattedJson GetActiveData()
    {
        if(activeData == null)
        {
            Debug.LogError("Trying to access null active data in GetActiveData");
            return null;
        }
        return activeData;
    }

    public static void SaveActiveData()
    {
        if(activeData == null)
        {
            Debug.LogError("Trying to save null active data");
            return;
        }
        Debug.LogWarning("Saving...");
        string json = JsonUtility.ToJson(activeData);
        PlayerPrefs.SetString("FormattedJson", json);
    }

    private static void SetUIStatics()
    {
        UIStatics.AnimationSpeed = activeData.fAnimationSpeed;
        UIStatics.RequireConfirmation = activeData.bRequireConfirmation;
        UIStatics.DefaultGroupWindowScale = activeData.fDefaultGroupWindowScale;
        UIStatics.Fullscreen = activeData.bFullscreen;
    }

    public static void SetActiveClassIndex(int index)
    {
        if (activeData == null)
        {
            Debug.LogError("Trying to edit null active data");
            return;
        }
        activeData.focusedClassIndex = index;
    }
}

[System.Serializable]
public class ClassDataOnly
{
    [SerializeField] List<ClassDataset> myClasses;

    public ClassDataOnly(List<ClassDataset> classes)
    {
        myClasses = classes;
    }

    public static List<ClassDataset> DeSerialize(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        ClassDataOnly dat = null;
        try
        {
            dat = JsonUtility.FromJson<ClassDataOnly>(json);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        if(dat != null)
        {
            return dat.myClasses;
        }
        return null;
    }

    public void WriteToLocalDirectory()
    {
        string path = Application.dataPath;
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            path += "/../../";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path += "/../";
        }

        string qualifiedPath = path + $"/Backups/backup{(int)System.DateTime.Now.TimeOfDay.TotalSeconds}.gdat";
        if(!Directory.Exists(path + "/Backups"))
        {
            Directory.CreateDirectory(path + "/Backups" );
        }

        if(Directory.GetFiles(path + "/Backups").Length > 100)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path + "/Backups");

            foreach (FileInfo file in di.GetFiles())
            {
                if(file.Extension == ".gdat" || file.Extension == ".meta")
                    file.Delete();
            }
        }

        if(!File.Exists(qualifiedPath))
        {
            FileStream fs = File.Create(qualifiedPath);
            fs.Close();
        }
        string jsonStr = JsonUtility.ToJson(this, true);
        File.WriteAllText(qualifiedPath, jsonStr);
        Debug.Log($"Backing up to {qualifiedPath}");
    }
}

[System.Serializable]
public class FormattedJson
{
    [SerializeField] public int numGroups = 5;
    [SerializeField] public bool keepExtrasSeparate = true;
    [SerializeField] public List<ClassDataset> loadedClasses;
    [SerializeField] public bool bRequireConfirmation;
    [SerializeField] public float fAnimationSpeed;
    [SerializeField] public float fDefaultGroupWindowScale;
    [SerializeField] public int focusedClassIndex = 0;
    [SerializeField] public ColorTheme currentColorTheme;
    [SerializeField] public bool bFullscreen = true;
    public FormattedJson()
    {
        loadedClasses = new List<ClassDataset>();
        bRequireConfirmation = true;
        fAnimationSpeed = 5f;
        fDefaultGroupWindowScale = 1f;
        bFullscreen = true;
    }

    public void RemoveClass(int hash)
    {
        ClassDataset dataToRemove = loadedClasses.Find(x => x.GetHash() == hash);
        if(dataToRemove != null)
            loadedClasses.Remove(dataToRemove);
        else{
            Debug.LogError($"Trying to remove class that doesn't exist ({hash})");
        }
        StaticFileIO.SaveActiveData();
    }

    public void RemoveStudentFromActiveClass(int studentHash)
    {
        StudentDataset studentDataToRemove = GetActiveClass().myStudents.Find(x => x.GetHash() == studentHash);
        if(studentDataToRemove != null)
        {
            GetActiveClass().myStudents.Remove(studentDataToRemove);
        }
        else{
            Debug.LogError($"Trying to remove student that doesn't exist ({studentHash})");
        }
    }

    public void AddClass(ClassDataset classToAdd)
    {
        if(classToAdd != null)
            loadedClasses.Add(classToAdd);
        else{
            Debug.LogError("Trying to add null class");
        }
    }

    public void AddStudentToActiveClass(StudentDataset studentToAdd)
    {
        if (studentToAdd != null)
            loadedClasses[focusedClassIndex].myStudents.Add(studentToAdd);
        else
        {
            Debug.LogError("Trying to add null student");
        }
    }

    public void SetClassAsFocused(int classHash)
    {
        ClassDataset classToSetActive = loadedClasses.Find(x => x.GetHash() == classHash);
        if(classToSetActive != null)
        {
            focusedClassIndex = loadedClasses.IndexOf(classToSetActive);
        }
        else{
            Debug.LogError($"Trying to set null class as active {classHash}.");
        }
    }

    public ClassDataset GetActiveClass()
    {
        ClassDataset dat = loadedClasses[focusedClassIndex];
        if(dat == null)
        {
            Debug.LogError("GetActiveClass resulted in null");
        }
        return dat;
    }
}