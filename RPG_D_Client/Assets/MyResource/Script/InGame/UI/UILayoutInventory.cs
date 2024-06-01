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

    public List<GameObject> itemSlotPool = new List<GameObject>();

    private void Awake()
    {
        dimButton.onClick.AddListener(() =>
        {
            ShowPopup(false);
        });
    }

    private void Start()
    {
        itemSlotPrefab.SetActive(false);
    }

    public void SetInventory(List<int> itemTypes, List<int> itemCounts)
    {
        var needSlot = itemCounts.Count - itemSlotPool.Count;

        for (int i = 0; i < needSlot; i++)
        {
            var itemSlot = Instantiate(itemSlotPrefab, itemSlotParent.transform);
            itemSlotPool.Add(itemSlot);
        }

        foreach (var itemSlot in itemSlotPool)
            itemSlot.SetActive(false);

        for (int i = 0; i < itemCounts.Count; i++)
        {
            var itemSlot = itemSlotPool[i];
            itemSlot.Find<TMP_Text>("ItemCount").text = itemCounts[i].ToString();
            itemSlot.SetActive(true);
        }
    }

    public void ShowPopup(bool show)
    {
        dimButton.gameObject.SetActive(show);
        inventoryPopup.SetActive(show);
    }
}
