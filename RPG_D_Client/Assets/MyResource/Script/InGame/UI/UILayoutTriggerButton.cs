using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILayoutTriggerButton : UIPopup
{
    Button triggerButton;
    TMP_Text triggerButtonText;
    Action onClick;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            triggerButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        triggerButton = gameObject.Find<Button>("TriggerButton");
        triggerButtonText = triggerButton.gameObject.Find<TMP_Text>("Text");
    }

    public void SetButton(string buttonText, Action onClickEvent)
    {
        triggerButtonText.text = buttonText;
        triggerButton.gameObject.SetActive(true);
        onClick = onClickEvent;

        triggerButton.onClick.RemoveAllListeners();
        triggerButton.onClick.AddListener(() => OnClickEvent());
    }

    void OnClickEvent()
    {
        Hide();
        onClick?.Invoke();
    }
}
