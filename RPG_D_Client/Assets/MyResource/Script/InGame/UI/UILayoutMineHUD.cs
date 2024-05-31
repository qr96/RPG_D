using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILayoutMineHUD : UILayout
{
    public GuageBar hpBar;
    public List<TMP_Text> itemNotiList;

    Coroutine reduceHpCo;
    Queue<string> itemNotiMessageQue = new Queue<string>();
    Queue<TMP_Text> itemNotiPool = new Queue<TMP_Text>();

    long maxHp;
    long nowHP;

    private void Start()
    {
        foreach (var item in itemNotiList)
            itemNotiPool.Enqueue(item);
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
        if (itemNotiPool.Count > 0)
        {
            var itemNoti = itemNotiPool.Dequeue();
            itemNoti.text = itemNotiMessageQue.Dequeue();
            itemNoti.color = Color.white;
            itemNoti.gameObject.SetActive(true);
            itemNoti.DOFade(0f, 0.5f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    itemNoti.gameObject.SetActive(false);
                    itemNotiPool.Enqueue(itemNoti);
                    if (itemNotiMessageQue.Count > 0)
                        FlushItemMessageQue();
                });
        }
    }
}
