using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupNotice : UIPopup
{
    GameObject dim;
    Button yesButton;
    Button noButton;
    Button singleYesButton;

    GameObject notiPopup;
    TMP_Text desc;

    Action onYes;

    public override void OnCreate()
    {
        dim = gameObject.Find("Dim");
        yesButton = gameObject.Find<Button>("YesButton");
        noButton = gameObject.Find<Button>("NoButton");
        singleYesButton = gameObject.Find<Button>("SingleYesButton");

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

        singleYesButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    public void SetPopup(string desc)
    {
        this.desc.text = desc;
        this.onYes = null;

        singleYesButton.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
