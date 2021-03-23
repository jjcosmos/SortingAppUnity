using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EditableNameElement : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;

    [SerializeField] Sprite editButtonDefault;
    [SerializeField] Sprite editButtonConfirmChanges;
    [SerializeField] Image editButtonImage;

    public UnityEvent OnEditFinished;

    bool isEditing = false;

    private void Start()
    {
        //editButtonImage.sprite = editButtonDefault;
    }

    public void SetInputWriteable(bool writeable)
    {
        inputField.readOnly = !writeable;
        //Debug.Log($"Setting writeable to {writeable}");
    }

}
