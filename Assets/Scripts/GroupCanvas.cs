using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCanvas : MonoBehaviour
{
    [SerializeField] public StudentGridPanel gridPanel;
    public RectTransform gridPanelRect;
    [SerializeField] public TMPro.TMP_Text groupTitle;
    [SerializeField] public Canvas canvasComponent;
    private static float maxScaleUp = 2f;
    private static float maxScaleDown = 1f;
    private static float scaleIncrement = .2f;
    private float currentScale = 1f;
    private Vector3 offset;
    static int currentMaxInLayer = 10;

    public void SetGroupTitle(string title)
    {
        groupTitle.text = "Group " + title;
    }

    private void Start() {
        gridPanelRect = gridPanel.GetComponent<RectTransform>();
        transform.localScale = UIStatics.DefaultGroupWindowScale * Vector2.one;
    }

    public void OnScaleUpPressed()
    {
        currentScale += scaleIncrement;
        UpdateScale();
    }

    public void OnScaleDownPressed()
    {
        currentScale -= scaleIncrement;
        UpdateScale();
    }

    private void UpdateScale()
    {
        currentScale = Mathf.Clamp(currentScale, maxScaleDown, maxScaleUp);
        transform.localScale = Vector2.one * currentScale;
    }

    public void OnDragBegin()
    {
        offset =  transform.position - Input.mousePosition;
        canvasComponent.sortingOrder = currentMaxInLayer ++;
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition + offset;
    }

    public void OnMouseOver()
    {
        if(StudentUIElement.draggingUIElement != null)
        {
            StudentUIElement.currentMouseCanvas = this;
        }
    }

    public void OnMouseExit()
    {
        if (StudentUIElement.draggingUIElement != null)
        {
            StudentUIElement.currentMouseCanvas = null;
        }
    }
}
