using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupMineShop : UIPopup
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
        fullPrice = DataTable.GetItemPrice(items);

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
    }

    void OnClickSellAll()
    {
        if (fullPrice == 0)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("팔 수 있는 아이템이 없습니다.");
        }
        else
        {
            var message = $"총 판매금액은 {fullPrice}골드 입니다. 정말로 파시겠습니까?";
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup(message,
                () => LocalPacketSender.C_SellItem(true, 0, 0));
        }
        
    }
}
