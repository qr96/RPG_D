using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineShop : UILayout
{
    Button dim;
    GameObject shopPopup;
    Button sellAll;
    SlotView shopInventory;

    long fullPrice;

    private void Awake()
    {
        dim = gameObject.Find<Button>("Dim");
        shopPopup = gameObject.Find("ShopPopup");
        sellAll = gameObject.Find<Button>("SellAllButton");
        shopInventory = shopPopup.GetComponent<SlotView>();

        sellAll.onClick.AddListener(() => OnClickSellAll());
        dim.onClick.AddListener(() => HideShopPopup());

        var slotPrefab = gameObject.Find("ShopPopup/Scroll View/Viewport/Content/ItemSlot");
        shopInventory.SetPrefab(slotPrefab, slotPrefab.transform.parent);
        slotPrefab.SetActive(false);
    }

    private void Start()
    {
        HideShopPopup();
    }

    public void ShowShopPopup()
    {
        dim.gameObject.SetActive(true);
        shopPopup.SetActive(true);
        sellAll.gameObject.SetActive(true);

        Managers.input.AddKeyDownEvent(KeyCode.Escape, () => HideShopPopup());
    }

    public void HideShopPopup()
    {
        dim.gameObject.SetActive(false);
        shopPopup.SetActive(false);
        sellAll.gameObject.SetActive(false);
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
        var message = $"총 판매금액은 {fullPrice}골드 입니다. 정말로 파시겠습니까?";
        Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup(message,
            () => LocalPacketSender.C_SellItem(true, 0, 0), null);
    }
}
