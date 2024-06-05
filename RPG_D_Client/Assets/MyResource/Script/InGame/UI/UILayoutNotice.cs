using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutNotice : UILayout
{
    GameObject dim;
    Button yesButton;
    Button noButton;

    GameObject notiPopup;
    TMP_Text desc;

    Action onYes;
    Action onNo;

    private void Awake()
    {
        dim = gameObject.Find("Dim");
        yesButton = gameObject.Find<Button>("YesButton");
        noButton = gameObject.Find<Button>("NoButton");

        notiPopup = gameObject.Find("NoticePopup");
        desc = notiPopup.Find<TMP_Text>("Desc");

        yesButton.onClick.AddListener(() =>
        {
            HideNoticePopup();
            if (onYes != null)
                onYes();
        });
        noButton.onClick.AddListener(() =>
        {
            HideNoticePopup();
            if (onNo != null)
                onNo();
        });
    }

    private void Start()
    {
        HideNoticePopup();
    }

    public void ShowNoticePopup(string desc, Action onYes, Action onNo)
    {
        this.desc.text = desc;
        this.onYes = onYes;
        this.onNo = onNo;

        dim.SetActive(true);
        notiPopup.SetActive(true);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    public void HideNoticePopup()
    {
        dim.SetActive(false);
        notiPopup.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
