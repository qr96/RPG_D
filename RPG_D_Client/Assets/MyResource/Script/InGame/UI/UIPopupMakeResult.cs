using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupMakeResult : UIPopup
{
    GameObject resultPopup;
    GameObject equip01;
    GameObject equip02;

    Button equipButton;
    Button sellButton;

    public override void OnCreate()
    {
        resultPopup = gameObject.Find("ResultPopup");
        equip01 = resultPopup.Find("Equipment01");
        equip02 = resultPopup.Find("Equipment02");

    }

    public override void InputEvent()
    {

    }
}
