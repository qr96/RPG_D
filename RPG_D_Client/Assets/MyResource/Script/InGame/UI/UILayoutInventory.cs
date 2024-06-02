using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutInventory : UILayout
{
    public Button dimButton;
    public GameObject inventoryPopup;
    public GameObject itemSlotParent;
    public GameObject itemSlotPrefab;

    TMP_Text moneyText;

    public List<GameObject> itemSlotPool = new List<GameObject>();

    private void Awake()
    {
        dimButton.onClick.AddListener(() =>
        {
            ShowPopup(false);
        });

        moneyText = inventoryPopup.Find<TMP_Text>("Property/MoneyText");
    }

    private void Start()
    {
        itemSlotPrefab.SetActive(false);
    }

    public void SetInventory(List<Item> minerals, long money)
    {
        var needSlot = minerals.Count - itemSlotPool.Count;

        for (int i = 0; i < needSlot; i++)
        {
            var itemSlot = Instantiate(itemSlotPrefab, itemSlotParent.transform);
            itemSlotPool.Add(itemSlot);
        }

        foreach (var itemSlot in itemSlotPool)
            itemSlot.SetActive(false);

        for (int i = 0; i < minerals.Count; i++)
        {
            if (minerals[i].count > 0)
            {
                var itemSlot = itemSlotPool[i];
                itemSlot.Find<TMP_Text>("ItemCount").text = minerals[i].count.ToString();
                itemSlot.SetActive(true);
            }
        }

        moneyText.text = money.ToString();
    }

    public void ShowPopup(bool show)
    {
        dimButton.gameObject.SetActive(show);
        inventoryPopup.SetActive(show);
    }
}
