using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ReactiveButton : MonoBehaviour
{
    private Vector3 startingScale;
    private Vector3 targetScale;
    private Vector3 endingScale;
    EventTrigger myTrigger;

    void Start()
    {
        startingScale = transform.localScale;
        endingScale = (startingScale.x + .1f) * Vector3.one;
        targetScale = startingScale;

        myTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { MyOnMouseOver(); });
        myTrigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((eventData) => { MyOnMouseExit(); });
        myTrigger.triggers.Add(entry2);
    }

    public void MyOnMouseOver()
    {
        targetScale = endingScale;
    }

    public void MyOnMouseExit()
    {
        targetScale = startingScale;
    }
    
    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * UIStatics.AnimationSpeed);
    }
}
