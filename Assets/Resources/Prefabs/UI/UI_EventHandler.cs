using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler {
    public Action<PointerEventData> OnClickHandler = null;

    public void OnPointerClick(PointerEventData data) {
        if(OnClickHandler != null) {
            OnClickHandler.Invoke(data);
        }
    }
}
