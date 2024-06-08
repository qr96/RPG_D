using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineShop : UIPopup
{
    Button dim;
    GameObject shopPopup;
    Button sellAll;
    SlotView shopInventory;

    long fullPrice;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dim.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.Space))
            sellAll.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dim = gameObject.Find<Button>("Dim");
        shopPopup = gameObject.Find("ShopPopup");
        sellAll = gameObject.Find<Button>("SellAllButton");
        shopInventory = shopPopup.GetComponent<SlotView>();

        sellAll.onClick.AddListener(() => OnClickSellAll());
        dim.onClick.AddListener(() => Hide());

        var slotPrefab = gameObject.Find("ShopPopup/Scroll View/Viewport/Content/ItemSlot");
        shopInventory.SetPrefab(slotPrefab, slotPrefab.transform.parent);
        slotPrefab.SetActive(false);
    }

    public void SetInventory(List<Item> items)
    {
        sellAll.enabled = false;
        foreach (var item in items)
        {
            if (item.count > 0)
            {
                sellAll.enabled = true;
                break;
            }
        }

        shopInventory.SetInventory(items,
            (item, slot) =>
            {
                if (item.count > 0)
                {
                    var countText = slot.Find<TMP_Text>("ItemCount");
                    countText.text = item.count.ToString();
                    var itemImage = slot.Find<Image>("ItemImage");
                    itemImage.sprite = Resources.Load<Sprite>(DataTable.GetItemSpritePath(item.itemType));
                    slot.SetActive(true);
                }
            }, null);
        fullPrice = DataTable.GetItemPrice(items);
    }

    void OnClickSellAll()
    {
        var message = $"�� �Ǹűݾ��� {fullPrice}��� �Դϴ�. ������ �Ľðڽ��ϱ�?";
        Managers.ui.ShowPopup<UILayoutNotice>().SetPopup(message,
            () => LocalPacketSender.C_SellItem(true, 0, 0));
    }
}
