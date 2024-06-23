using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIPopupMineTicket : UIPopup
{
    Button dimButton;
    GameObject inventoryPopup;
    GameObject itemSlotParent;
    GameObject itemSlotPrefab;
    SlotView inventoryView;

    List<GameObject> itemSlotPool = new List<GameObject>();
    
    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dimButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dimButton = gameObject.Find<Button>("Dim");
        inventoryPopup = gameObject.Find("InventoryPopup");
        itemSlotParent = inventoryPopup.Find("ScrollView/Viewport/Content");
        itemSlotPrefab = itemSlotParent.Find("ItemSlot");

        inventoryView = inventoryPopup.GetComponent<SlotView>();

        dimButton.onClick.AddListener(() => Hide());

        var slotPrefab = gameObject.Find("InventoryPopup/Scroll View/Viewport/Content/ItemSlot");
        slotPrefab.SetActive(false);
        inventoryView.SetPrefab(slotPrefab, slotPrefab.transform.parent);
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
                    itemImage.sprite = Resources.Load<Sprite>(DataTable.GetTicketSpritePath(item.itemType));
                    slot.SetActive(true);
                }
            });
    }
}
