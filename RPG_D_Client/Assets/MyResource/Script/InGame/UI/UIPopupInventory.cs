using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupInventory : UIPopup
{
    Button dimButton;
    GameObject inventoryPopup;
    GameObject itemSlotParent;
    GameObject itemSlotPrefab;
    SlotView inventoryView;

    TMP_Text moneyText;

    public List<GameObject> itemSlotPool = new List<GameObject>();

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dimButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dimButton = gameObject.Find<Button>("Dim");
        inventoryPopup = gameObject.Find("InventoryPopup");
        itemSlotParent = inventoryPopup.Find("Scroll View/Viewport/Content");
        itemSlotPrefab = itemSlotParent.Find("ItemSlot");
        inventoryView = inventoryPopup.GetComponent<SlotView>();

        dimButton.onClick.AddListener(() => Hide());

        itemSlotPrefab.SetActive(false);
        inventoryView.SetPrefab(itemSlotPrefab, itemSlotParent.transform);
        
        moneyText = inventoryPopup.Find<TMP_Text>("Property/MoneyText");
    }

    private void Start()
    {
        itemSlotPrefab.SetActive(false);
    }

    public void SetInventory(List<Item> items)
    {
        inventoryView.SetInventory(items,
            (item, slot) =>
            {
                if (item.count > 0)
                {
                    var countText = slot.Find<TMP_Text>("ItemCount");
                    countText.text = item.count.ToString();

                    var itemImage = slot.Find<Image>("ItemImage");
                    itemImage.sprite = Resources.Load<Sprite>(DataTable.GetConsumableSpritePath(item.itemType));
                    slot.SetActive(true);

                    var slotButton = slot.GetComponent<Button>();
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() =>
                    {
                        Managers.ui.ShowPopup<UIPopupItemInfo>().Set(
                            "비급서",
                            "사용 시 무공을 획득합니다.",
                            itemImage.sprite,
                            item.count,
                            () => OnClickUseItem(item.itemType)
                            );
                    });
                }
            });
    }

    public void SetMoney(long money)
    {
        moneyText.text = $"보유 골드 : {RDUtil.MoneyComma(money)}";
    }

    void OnClickUseItem(int itemType)
    {
        Debug.Log($"Use Item {itemType}");
        LocalPacketSender.C_UseItem(itemType);
    }
}
