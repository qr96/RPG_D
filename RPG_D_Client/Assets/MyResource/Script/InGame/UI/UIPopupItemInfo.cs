using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupItemInfo : UIPopup
{
    Button dimButton;
    GameObject infoPopup;
    TMP_Text title;
    Image itemImage;
    TMP_Text itemCount;
    TMP_Text desc;
    Button useButton;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dimButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dimButton = gameObject.Find<Button>("Dim");
        
        infoPopup = gameObject.Find("InfoPopup");
        title = infoPopup.Find<TMP_Text>("Title");
        itemImage = infoPopup.Find<Image>("ItemSlot/ItemImage");
        itemCount = infoPopup.Find<TMP_Text>("ItemSlot/Count");
        desc = infoPopup.Find<TMP_Text>("Desc");
        
        useButton = infoPopup.Find<Button>("UseButton");

        dimButton.onClick.AddListener(() => Hide());
    }

    public void Set(string itemName, string itemDesc, Sprite itemSprite, long count, Action clickUse)
    {
        title.text = itemName;
        desc.text = itemDesc;
        itemImage.sprite = itemSprite;
        itemCount.text = count.ToString();

        useButton.onClick.RemoveAllListeners();
        useButton.gameObject.SetActive(clickUse != null);
        if (clickUse != null)
            useButton.onClick.AddListener(() => clickUse());
    }
}
