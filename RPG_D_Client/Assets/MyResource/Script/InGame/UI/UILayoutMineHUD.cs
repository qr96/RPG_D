using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineHUD : UILayout
{
    public GuageBar hpBar;
    public GameObject ItemNotiBox;
    public TMP_Text itemNotiPrefab;
    public Button inventoryButton;
    Button equipmentButton;

    Coroutine reduceHpCo;
    Queue<string> itemNotiMessageQue = new Queue<string>();
    List<TMP_Text> itemNotiPool = new List<TMP_Text>();

    long maxHp;
    long nowHP;

    const int MAX_ITEM_NOTI = 5;

    private void Awake()
    {
        equipmentButton = gameObject.Find<Button>("EquipButton");

        inventoryButton.onClick.AddListener(() => Managers.ui.GetLayout<UILayoutInventory>().ShowPopup(true));
        equipmentButton.onClick.AddListener(() => Managers.ui.GetLayout<UILayoutEquipment>().ShowPopup(true));
    }

    private void Start()
    {
        itemNotiPrefab.gameObject.SetActive(false);
        var itemNotiPivotPos = itemNotiPrefab.GetComponent<RectTransform>().anchoredPosition;
        var itemNotiHeight = itemNotiPrefab.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < MAX_ITEM_NOTI; i++)
        {
            var itemNoti = Instantiate(itemNotiPrefab, itemNotiPrefab.transform.parent);
            itemNotiPool.Add(itemNoti);
            itemNoti.GetComponent<RectTransform>().anchoredPosition = new Vector2(itemNotiPivotPos.x, itemNotiPivotPos.y + i * itemNotiHeight);
        }
    }

    public void SetHpBar(long maxHp, long nowHp)
    {
        this.maxHp = maxHp;
        this.nowHP = nowHp;

        hpBar.SetGuage(maxHp, nowHp);
    }

    public void StartReduceHP(long reduceHpPerSec)
    {
        if (reduceHpPerSec <= 0)
            return;

        if (reduceHpCo != null)
            StopCoroutine(reduceHpCo);

        reduceHpCo = StartCoroutine(ReduceHpCo(reduceHpPerSec));
    }

    public void StopReduceHp()
    {
        if (reduceHpCo != null)
            StopCoroutine(reduceHpCo);
    }

    public void AddItemNotiQue(string message)
    {
        itemNotiMessageQue.Enqueue(message);
        FlushItemMessageQue();
    }

    IEnumerator ReduceHpCo(long reduceHpPerSec)
    {
        while (nowHP > 0)
        {
            yield return new WaitForSeconds(1f);
            nowHP -= reduceHpPerSec;
            SetHpBar(maxHp, nowHP);
        }
    }

    void FlushItemMessageQue()
    {
        for (int i = 0; i < itemNotiPool.Count; i++)
        {
            if (itemNotiMessageQue.Count <= 0)
                break;

            if (itemNotiPool[i].isActiveAndEnabled == false)
            {
                var itemNoti = itemNotiPool[i];
                itemNoti.text = itemNotiMessageQue.Dequeue();
                itemNoti.color = Color.white;
                itemNoti.gameObject.SetActive(true);
                itemNoti.DOFade(0f, 0.3f)
                    .SetDelay(2f)
                    .OnComplete(() =>
                    {
                        itemNoti.gameObject.SetActive(false);
                        FlushItemMessageQue();
                    });
                break;
            }
        }
    }
}
