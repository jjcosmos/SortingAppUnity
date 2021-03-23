using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayIcon : MonoBehaviour
{
    private enum ETrayIconType {none, classes, settings, credits};
    

    [SerializeField] ETrayIconType trayIconType;

    public void MyOnClick()
    {
        switch(trayIconType)
        {
            case ETrayIconType.classes:
                HandleClassClick();
                break;
            case ETrayIconType.settings:
                HandleSettingsClick();
                break;
            case ETrayIconType.credits:
                HandleSettingsClick();
                break;
            default:
                break;
        }
    }

    protected void HandleClassClick()
    {
        AsyncSceneLoader.LoadSceneByName("Classes");
    }

    protected void HandleSettingsClick()
    {

    }

    protected void HandleCreditsClick()
    {

    }

    
}
