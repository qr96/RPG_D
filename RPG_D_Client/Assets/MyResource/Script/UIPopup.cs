using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{
    protected bool playerMoveLock = true;

    // This method will be called by InputManager's Update().
    public abstract void InputEvent();
    public abstract void OnCreate();

    public void Show()
    {
        gameObject.SetActive(true);
        Managers.input.PushInputEvent(InputEvent, playerMoveLock);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Managers.input.PopInputEvent();
    }
}
