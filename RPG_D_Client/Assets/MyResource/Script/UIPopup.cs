using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{
    // This method will be called by InputManager's Update().
    public abstract void InputEvent();
    public abstract void OnCreate();

    public void Show()
    {
        gameObject.SetActive(true);
        Managers.input.PushInputEvent(InputEvent);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Managers.input.PopInputEvent();
    }
}
