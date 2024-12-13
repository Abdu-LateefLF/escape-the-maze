using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    FixedTouchField touchField;

    bool activatedFromButton = false;

    float timeHeldDown = 0;

    private void Start()
    {
        touchField = GetComponentInParent<FixedTouchField>();
    }

    private void Update()
    {
        if (activatedFromButton)
        {
            timeHeldDown += Time.deltaTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchField.OnPointerDown(eventData);
        activatedFromButton = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (activatedFromButton)
        {
            touchField.OnPointerUp(eventData);
            activatedFromButton = false;  

            if (timeHeldDown > 0.5f)
            {
                eventData.eligibleForClick = false;
            }
        }

        timeHeldDown = 0;
    }
}
