using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIPopupEquipShop : UIPopup
{
    GameObject shopPopup;
    GameObject itemSlotPrefab;
    SlotView slotView;

    public override void InputEvent()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCreate()
    {
        shopPopup = gameObject.Find("ShopPopup");

        itemSlotPrefab = shopPopup.gameObject.Find("Scroll View/Viewport/Content/ItemSlot");
        itemSlotPrefab.SetActive(false);

        slotView = shopPopup.GetComponent<SlotView>();
        slotView.SetPrefab(itemSlotPrefab, itemSlotPrefab.transform.parent);
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

                slot.SetActive(true);
            }, null);
    }
}
