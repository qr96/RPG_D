using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupInventory : UIPopup
{
    public Button dimButton;
    public GameObject inventoryPopup;
    public GameObject itemSlotParent;
    public GameObject itemSlotPrefab;
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
        inventoryView = inventoryPopup.GetComponent<SlotView>();

        dimButton.onClick.AddListener(() => Hide());

        var slotPrefab = gameObject.Find("InventoryPopup/Scroll View/Viewport/Content/ItemSlot");
        slotPrefab.SetActive(false);
        inventoryView.SetPrefab(slotPrefab, slotPrefab.transform.parent);
        
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
                    itemImage.sprite = Resources.Load<Sprite>(DataTable.GetItemSpritePath(item.itemType));
                    slot.SetActive(true);
                }
            });
    }

    public void SetMoney(long money)
    {
        moneyText.text = money.ToString();
    }
}
