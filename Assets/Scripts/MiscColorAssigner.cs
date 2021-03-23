using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscColorAssigner : MonoBehaviour
{
    [SerializeField] EColorType colorType;
    private Camera myCamera;
    private MeshRenderer myMeshRend;
    IEnumerator Start()
    {
        yield return null;
        RefreshColor();
        ColorThemer.OnUpdateTheme.AddListener(this.RefreshColor);
    }

    void RefreshColor()
    {
        myCamera = GetComponent<Camera>();
        myMeshRend = GetComponent<MeshRenderer>();

        if (StaticFileIO.GetActiveData().currentColorTheme != null)
        {
            switch (colorType)
            {
                case EColorType.Primary:
                    DoColorCam(StaticFileIO.GetActiveData().currentColorTheme.ColorBase);
                    DoColorMesh(StaticFileIO.GetActiveData().currentColorTheme.ColorBase);
                    break;
                case EColorType.Secondary:
                    DoColorCam(StaticFileIO.GetActiveData().currentColorTheme.ColorSecondary);
                    DoColorMesh(StaticFileIO.GetActiveData().currentColorTheme.ColorSecondary);
                    break;
                case EColorType.Tertiary:
                    DoColorCam(StaticFileIO.GetActiveData().currentColorTheme.ColorTertiary);
                    DoColorMesh(StaticFileIO.GetActiveData().currentColorTheme.ColorTertiary);
                    break;
                case EColorType.Four:
                    DoColorCam(StaticFileIO.GetActiveData().currentColorTheme.ColorFour);
                    DoColorMesh(StaticFileIO.GetActiveData().currentColorTheme.ColorFour);
                    break;
            }
        }
    }

    void DoColorCam(Color color)
    {
        if(myCamera == null) return;
        myCamera.backgroundColor = color;
    }

    void DoColorMesh(Color color)
    {
        if(myMeshRend == null) return;
        myMeshRend.material.color = color;
    }
}
