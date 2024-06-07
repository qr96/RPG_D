using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutEquipment : UILayout
{
    GameObject dim;
    GameObject equipmentPopup;

    Button sellAllButton;

    TMP_Text attackStat;
    TMP_Text hpStat;
    TMP_Text speedStat;

    TMP_Text money;

    Button weaponButton;
    Button armorButton;
    Button shoesButton;

    private void Awake()
    {
        dim = gameObject.Find("Dim");
        equipmentPopup = gameObject.Find("EquipmentPopup");

        sellAllButton = equipmentPopup.Find<Button>("SellAllButton");

        attackStat = equipmentPopup.Find<TMP_Text>("Stats/Attack/Value");
        hpStat = equipmentPopup.Find<TMP_Text>("Stats/HP/Value");
        speedStat = equipmentPopup.Find<TMP_Text>("Stats/Speed/Value");

        money = equipmentPopup.Find<TMP_Text>("Money");

        weaponButton = equipmentPopup.Find<Button>("Equipments/Weapon");
        armorButton = equipmentPopup.Find<Button>("Equipments/Armor");
        shoesButton = equipmentPopup.Find<Button>("Equipments/Shoes");

        sellAllButton.onClick.AddListener(() => ShowPopup(false));

        weaponButton.onClick.AddListener(() => OnClickEquipButton(0));
        armorButton.onClick.AddListener(() => OnClickEquipButton(1));
        shoesButton.onClick.AddListener(() => OnClickEquipButton(2));
    }

    private void Start()
    {
        ShowPopup(false);
    }

    public void ShowPopup(bool show)
    {
        dim.SetActive(show);
        equipmentPopup.SetActive(show);
    }

    public void SetStat(string attack, string hp, string speed)
    {
        attackStat.text = attack;
        hpStat.text = hp;
        speedStat.text = speed;
    }

    public void SetEquipLevel(int equipType, int level)
    {
        var levelString = $"+{level}";
        if (equipType == 0)
        {
            weaponButton.gameObject.Find<TMP_Text>("Level").text = levelString;
        }
        else if (equipType == 1)
        {
            armorButton.gameObject.Find<TMP_Text>("Level").text = levelString;
        }
        else if (equipType == 2)
        {
            shoesButton.gameObject.Find<TMP_Text>("Level").text = levelString;
        }
    }

    public void SetMoney(long money)
    {
        this.money.text = RDUtil.MoneyComma(money);
    }

    void OnClickEquipButton(int equipType)
    {
        var nowLevel = Managers.data.GetEquipLevel(equipType);
        var price = DataTable.GetEquipEnhancePrice(nowLevel);
        var successPer = DataTable.GetEquipEnhanceSuccessPercent(nowLevel);
        var message = $"정말로 강화하시겠습니까?\n강화비용:{price}\n성공확률:{successPer}%";
        Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup(message,
            () => LocalPacketSender.C_EnforceEquip(equipType), null);
    }
}
