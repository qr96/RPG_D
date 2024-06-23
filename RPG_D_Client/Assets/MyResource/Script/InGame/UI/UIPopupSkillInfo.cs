using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillInfo : UIPopup
{
    Button dimButton;
    GameObject infoPopup;
    TMP_Text title;
    Image skillImage;
    TMP_Text skillLevel;
    GuageBar skillExp;
    TMP_Text desc;


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
        skillImage = infoPopup.Find<Image>("ItemSlot/ItemImage");
        skillLevel = infoPopup.Find<TMP_Text>("ItemSlot/Level");
        skillExp = infoPopup.Find<GuageBar>("EXPGuage");
        desc = infoPopup.Find<TMP_Text>("Desc");

        dimButton.onClick.AddListener(() => Hide());
    }

    public void Set(string skillName, string skillDesc, Sprite skillSprite, int level, int maxExp, int nowExp)
    {
        title.text = skillName;
        desc.text = skillDesc;
        skillImage.sprite = skillSprite;
        skillLevel.text = level <= 0 ? "미획득" : $"Lv.{level}";
        skillExp.SetGuage(maxExp, nowExp);
    }
}
