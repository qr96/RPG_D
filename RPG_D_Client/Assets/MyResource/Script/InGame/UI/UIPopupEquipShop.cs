using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipShop : UIPopup
{
    Button dim;
    GameObject shopPopup;
    GameObject itemSlotPrefab;
    SlotView slotView;
    ToggleGroup toggleGroup;

    TMP_Text moneyText;
    TMP_Text needMoneyText;
    TMP_Text addStatText;

    Button weaponTabButton;
    Button shirtTabButton;
    Button bagTabButton;
    Button shoeTabButton;

    Button buyButton;

    public int selectedEquipType;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dim.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Space))
            buyButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dim = gameObject.Find<Button>("Dim");
        dim.onClick.AddListener(() => Hide());

        shopPopup = gameObject.Find("ShopPopup");
        toggleGroup = shopPopup.GetComponent<ToggleGroup>();

        itemSlotPrefab = shopPopup.gameObject.Find("Scroll View/Viewport/Content/ItemSlot");
        itemSlotPrefab.SetActive(false);

        slotView = shopPopup.GetComponent<SlotView>();
        slotView.SetPrefab(itemSlotPrefab, itemSlotPrefab.transform.parent);

        moneyText = shopPopup.Find<TMP_Text>("Info/Money");
        needMoneyText = shopPopup.Find<TMP_Text>("Info/NeedMoney");
        addStatText = shopPopup.Find<TMP_Text>("Info/AddStat");

        weaponTabButton = shopPopup.Find<Button>("Navigation/Weapon");
        shirtTabButton = shopPopup.Find<Button>("Navigation/Shirt");
        bagTabButton = shopPopup.Find<Button>("Navigation/Bag");
        shoeTabButton = shopPopup.Find<Button>("Navigation/Shoes");
        weaponTabButton.onClick.AddListener(() => SetPopup(Managers.data.GetMyUserData().weaponDic.Values.ToList()));
        shirtTabButton.onClick.AddListener(() => SetPopup(Managers.data.GetMyUserData().shirtDic.Values.ToList()));
        bagTabButton.onClick.AddListener(() => SetPopup(Managers.data.GetMyUserData().bagDic.Values.ToList()));
        shoeTabButton.onClick.AddListener(() => SetPopup(Managers.data.GetMyUserData().shoeDic.Values.ToList()));

        buyButton = gameObject.Find<Button>("BuyButton");
        buyButton.onClick.AddListener(() => OnClickBuyEquip());
    }

    public void SetPopup(long money)
    {
        moneyText.text = $"보유 골드 : {RDUtil.MoneyComma(money)}";
    }

    public void SetPopup(List<Equipment> equipList)
    {
        slotView.SetInventory(equipList,
            (item, slot) =>
            {
                var itemImage = slot.Find<Image>("Item/ItemImage");
                itemImage.sprite = Resources.Load<Sprite>(DataTable.GetEquipmentSpritePath(item.type));

                var levelText = slot.Find<TMP_Text>("Item/Level");
                levelText.text = $"+{item.level}";

                var itemNameText = slot.Find<TMP_Text>("Name");
                itemNameText.text = DataTable.GetEquipmentSpriteName(item.type);

                var itemInfoText = slot.Find<TMP_Text>("Info");
                itemInfoText.text = $"ATK+{DataTable.GetEquipmentStat(item.type, item.level)}";

                var unPurchased = slot.Find("Unpurchased");
                unPurchased.SetActive(item.level <= 0);

                var toggle = slot.GetComponent<Toggle>();
                toggle.graphic = slot.Find<Image>("Selected");
                toggle.group = toggleGroup;
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((isOn) => OnChangeToggle(isOn, item));

                slot.SetActive(true);
            });
    }

    void OnChangeToggle(bool isOn, Equipment equipment)
    {
        if (isOn)
        {
            selectedEquipType = equipment.type;
            if (equipment.level > 0)
            {
                var buyButtonText = buyButton.gameObject.Find<TMP_Text>("Text");
                buyButtonText.text = "강화하기";
            }
            else
            {
                var buyButtonText = buyButton.gameObject.Find<TMP_Text>("Text");
                buyButtonText.text = "구매하기";
            }

            needMoneyText.text = $"필요 골드 : {RDUtil.MoneyComma(DataTable.GetEquipmentEnhancePrice(equipment.type, equipment.level))}";
            addStatText.text = $"증가 스탯 : ATK +{DataTable.GetEquipmentAddedStat(equipment.type, equipment.level)}";
        }
    }

    void OnClickBuyEquip()
    {
        LocalPacketSender.C_BuyEquip(selectedEquipType);
    }
}
