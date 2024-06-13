using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupBlackSmith : UIPopup
{
    Button dim;

    GameObject smithPopup;
    TMP_Text anvilLevel;
    Button percentTable;
    Button levelUp;

    Button makeEquipment;
    
    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dim.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Space))
            makeEquipment.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dim = gameObject.Find<Button>("Dim");

        smithPopup = gameObject.Find("SmithPopup");
        anvilLevel = smithPopup.Find<TMP_Text>("AnvilImage/Level");
        levelUp = smithPopup.Find<Button>("LevelUpButton");
        percentTable = smithPopup.Find<Button>("PercentTableButton");

        makeEquipment = gameObject.Find<Button>("MakeButton");
        makeEquipment.onClick.AddListener(() => OnClickMakeButton());
    }

    public void SetPopup(int anvilLevel, long makeEquipPrice)
    {
        this.anvilLevel.text = $"LV : {anvilLevel}";
        
        var equipPriceText = $"제작비용 : {RDUtil.MoneyComma(makeEquipPrice)}";
        makeEquipment.gameObject.Find<TMP_Text>("NeedMoney").text = equipPriceText;
    }

    void OnClickMakeButton()
    {

    }
}
