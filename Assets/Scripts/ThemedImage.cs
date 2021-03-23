using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemedImage : MonoBehaviour
{
    [SerializeField] EColorType colorType;
    private Image myImage;
    IEnumerator Start()
    {
        yield return null;
        RefreshColor();
        ColorThemer.OnUpdateTheme.AddListener(this.RefreshColor);
    }

    void RefreshColor()
    {
        myImage = GetComponent<Image>();
        if (myImage == null) return;

        if (StaticFileIO.GetActiveData().currentColorTheme != null)
        {
            switch (colorType)
            {
                case EColorType.Primary:
                    myImage.color = StaticFileIO.GetActiveData().currentColorTheme.ColorBase;
                    break;
                case EColorType.Secondary:
                    myImage.color = StaticFileIO.GetActiveData().currentColorTheme.ColorSecondary;
                    break;
                case EColorType.Tertiary:
                    myImage.color = StaticFileIO.GetActiveData().currentColorTheme.ColorTertiary;
                    break;
                case EColorType.Four:
                    myImage.color = StaticFileIO.GetActiveData().currentColorTheme.ColorFour;
                    break;
            }
        }
    }
    
}
public enum EColorType{Primary, Secondary, Tertiary, Four};