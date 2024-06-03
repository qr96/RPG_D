using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutGameResult : UILayout
{
    GameObject dim;
    GameObject inventoryPopup;
    GameObject itemSlotParent;
    GameObject itemSlotPrefab;
    
    Button goHomeButton;
    Button endGameButton;

    List<GameObject> itemSlotPool = new List<GameObject>();

    private void Awake()
    {
        dim = gameObject.Find("Dim");

        inventoryPopup = gameObject.Find("InventoryPopup");
        itemSlotParent = inventoryPopup.Find("Scroll View/Viewport/Content");
        itemSlotPrefab = itemSlotParent.Find("ItemSlot");

        goHomeButton = gameObject.Find<Button>("GoHomeButton");
        endGameButton = gameObject.Find<Button>("EndGameButton");
    }

    private void Start()
    {
        itemSlotPrefab.SetActive(false);
        ShowGameResult(false);
        ShowEndGameButton(false);
    }

    public void ShowGameResult(bool show)
    {
        dim.SetActive(show);
        inventoryPopup.SetActive(show);
        goHomeButton.gameObject.SetActive(show);
        Managers.obj.myPlayer.SetPlayerMoveLock(show);

        if (show)
            Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickGoTown);
        else
            Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickGoTown);
    }

    public void SetInventory(List<Item> itemList)
    {
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
            itemSlot.SetActive(true);
        }
    }

    public void ShowEndGameButton(bool show)
    {
        endGameButton.gameObject.SetActive(show);

        if (show)
            Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickEndGame);
        else
            Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickEndGame);
    }

    void OnClickGoTown()
    {
        ShowGameResult(false);
        LocalPacketSender.C_MoveMap(1001);
    }

    void OnClickEndGame()
    {
        LocalPacketSender.C_MineGameResult();
        ShowEndGameButton(false);
    }
}
