using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePrompt : MonoBehaviour
{
    [SerializeField] Image myImage;
    [SerializeField] Sprite swapSpr;
    [SerializeField] Sprite moveSpr;
    [SerializeField] Sprite failSpr;
    [SerializeField][ColorUsage(true)] Color moveColor;
    [SerializeField][ColorUsage(true)] Color swapColor;
    [SerializeField][ColorUsage(true)] Color failColor;
    [SerializeField] Vector3 offset;
    // Update is called once per frame
    void LateUpdate()
    {
        if(StudentUIElement.draggingUIElement == null) 
        {
            myImage.enabled = false;
            return;
        }

        if(StudentUIElement.currentMouseStudentUIElement)
        {
            myImage.enabled = true;
            myImage.sprite = swapSpr;
            myImage.color = swapColor;
            transform.position = Input.mousePosition + offset;
        }
        else if (StudentUIElement.currentMouseCanvas)
        {
            myImage.enabled = true;
            myImage.sprite = moveSpr;
            myImage.color = moveColor;
            transform.position = Input.mousePosition + offset;
        }
        else
        {
            myImage.enabled = true;
            myImage.sprite = failSpr;
            myImage.color = failColor;
            transform.position = Input.mousePosition + offset;
        }
    }
}
