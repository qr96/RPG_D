using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupGameResult : UIPopup
{
    GameObject dim;
    GameObject inventoryPopup;
    GameObject itemSlotParent;
    GameObject itemSlotPrefab;
    TMP_Text inventoryPopupTitle;
    TMP_Text result;

    Button goHomeButton;

    List<GameObject> itemSlotPool = new List<GameObject>();

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            goHomeButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dim = gameObject.Find("Dim");

        inventoryPopup = gameObject.Find("InventoryPopup");
        itemSlotParent = inventoryPopup.Find("Scroll View/Viewport/Content");
        itemSlotPrefab = itemSlotParent.Find("ItemSlot");
        inventoryPopupTitle = inventoryPopup.Find<TMP_Text>("Title");
        result = gameObject.Find<TMP_Text>("Result");

        goHomeButton = gameObject.Find<Button>("GoHomeButton");

        goHomeButton.onClick.AddListener(() => OnClickGoTown());
    }

    private void Start()
    {
        itemSlotPrefab.SetActive(false);
    }

    public void SetResultPopup(bool success, List<Item> itemList)
    {
        result.text = success ? "≈ΩªÁº∫∞¯" : "≈ΩªÁΩ«∆–";
        inventoryPopupTitle.text = success ? "»πµÊ æ∆¿Ã≈€" : "¿“¿∫ æ∆¿Ã≈€";

        var needSlot = itemList.Count - itemSlotPool.Count;

        for (int i = 0; i < needSlot; i++)
        {
            var itemSlot = Instantiate(itemSlotPrefab, itemSlotParent.transform);
            itemSlotPool.Add(itemSlot);
        }

        foreach (var itemSlot in itemSlotPool)
            itemSlot.SetActive(false);

        for (int i = 0; i < itemList.Count; i++)
        {
            var itemSlot = itemSlotPool[i];
            itemSlot.Find<TMP_Text>("ItemCount").text = itemList[i].count.ToString();
            itemSlot.Find<Image>("ItemImage").sprite = Resources.Load<Sprite>(DataTable.GetItemSpritePath(itemList[i].itemType));
            itemSlot.SetActive(true);
        }
    }

    void OnClickGoTown()
    {
        Hide();
        LocalPacketSender.C_MoveMap(1001);
    }
}
