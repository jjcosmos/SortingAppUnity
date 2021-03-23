using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ConfirmationDialogue : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text dialogueText;

    static Action acceptAction;
    static Action cancelAction;
    static string prompt;
    static bool busy = false;
    CanvasGroup mainCanvas;

    private static ConfirmationDialogue _inst;

    private void Awake() {
        _inst = this;
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Start() {
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<CanvasGroup>();
        if(mainCanvas == null)
        {
            throw new Exception("No canvas marked as \"Main canvas\" in the scene");
        }
        _inst.gameObject.SetActive(false);
    }

    public static void AwaitConfirmation(Action myAcceptAction, Action myCancelAction, string myPrompt)
    {
        if(busy) return;
        acceptAction = myAcceptAction;
        cancelAction = myCancelAction;
        prompt = myPrompt;
        
        if(UIStatics.RequireConfirmation)
            ShowDialogue();
        else
        {
            acceptAction.Invoke();
        }
    }

    private static void ShowDialogue()
    {
        _inst.gameObject.SetActive(true);
        _inst.mainCanvas.interactable = false;
        _inst.dialogueText.text = prompt;
    }

    public void OnConfirmClicked()
    {
        HideDialogue();
        acceptAction.Invoke();
    }

    public void OnCancelClicked()
    {
        HideDialogue();
        cancelAction.Invoke();
    }

    private static void HideDialogue()
    {
        busy = false;
        _inst.mainCanvas.interactable = true;
        _inst.gameObject.SetActive(false);
    }

}
