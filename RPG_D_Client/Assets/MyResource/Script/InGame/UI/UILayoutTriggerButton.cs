using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILayoutTriggerButton : UILayout
{
    Button triggerButton;
    TMP_Text triggerButtonText;
    Action onClick;

    private void Awake()
    {
        triggerButton = gameObject.Find<Button>("TriggerButton");
        triggerButtonText = triggerButton.gameObject.Find<TMP_Text>("Text");
    }

    private void Start()
    {
        HideButton();
    }

    public void ShowButton(string buttonText, Action onClickEvent)
    {
        triggerButtonText.text = buttonText;
        triggerButton.gameObject.SetActive(true);
        onClick = onClickEvent;

        triggerButton.onClick.RemoveAllListeners();
        triggerButton.onClick.AddListener(() => OnClickEvent());
        Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickEvent);
    }

    public void HideButton()
    {
        triggerButton.gameObject.SetActive(false);
        Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickEvent);
    }

    void OnClickEvent()
    {
        onClick?.Invoke();
        HideButton();
    }
}
