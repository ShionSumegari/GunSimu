using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheatOpener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    float counter = 0;

    public System.Action onOpen;

    public void OnPointerDown(PointerEventData eventData)
    {
        counter = Time.realtimeSinceStartup;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
         if(Time.realtimeSinceStartup - counter >= 2.5f)
        {
            onOpen?.Invoke();
        }

    }
}
