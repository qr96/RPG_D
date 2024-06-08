using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutNotice : UIPopup
{
    GameObject dim;
    Button yesButton;
    Button noButton;

    GameObject notiPopup;
    TMP_Text desc;

    Action onYes;

    public override void OnCreate()
    {
        dim = gameObject.Find("Dim");
        yesButton = gameObject.Find<Button>("YesButton");
        noButton = gameObject.Find<Button>("NoButton");

        notiPopup = gameObject.Find("NoticePopup");
        desc = notiPopup.Find<TMP_Text>("Desc");

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            Hide();
            if (onYes != null)
                onYes();
        });
        noButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            yesButton.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            noButton.onClick.Invoke();
    }

    public void SetPopup(string desc, Action onYes)
    {
        this.desc.text = desc;
        this.onYes = onYes;
    }
}
