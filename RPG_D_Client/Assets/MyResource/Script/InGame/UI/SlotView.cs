using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    GameObject slotPrefab;
    Transform slotParent;

    List<GameObject> slotPool = new List<GameObject>();

    public void SetPrefab(GameObject slotPrefab, Transform slotParent)
    {
        this.slotPrefab = slotPrefab;
        this.slotParent = slotParent;

        foreach (var slot in slotPool)
            Destroy(slot);
    }

    public void SetInventory<T>(List<T> items, Action<T, GameObject> setSlotFunc0, Action<T, GameObject> setSlotFunc1, Action onClickSlot)
    {
        var needSlot = items.Count - slotPool.Count;

        for (int i = 0; i < needSlot; i++)
        {
            var itemSlot = Instantiate(slotPrefab, slotParent);
            slotPool.Add(itemSlot);
        }

        foreach (var itemSlot in slotPool)
            itemSlot.SetActive(false);

        for (int i = 0; i < items.Count; i++)
        {
            if (setSlotFunc0 != null)
                setSlotFunc0(items[i], slotPool[i]);

            if (setSlotFunc1 != null)
                setSlotFunc1(items[i], slotPool[i]);

            if (onClickSlot != null)
            {
                slotPool[i].GetOrAddComponent<Button>().onClick.RemoveAllListeners();
                slotPool[i].GetOrAddComponent<Button>().onClick.AddListener(() => onClickSlot());
            }
        }
    }
}
