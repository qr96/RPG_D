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
    Button skillButton;

    GameObject bagIndicator;
    TMP_Text bagIndicatorTmp;
    GameObject depthIndicator;
    TMP_Text depthIndicatorTmp;
    GameObject timeIndicator;
    TMP_Text timeIndicatorTmp;

    Coroutine reduceHpCo;
    Coroutine timerCo;
    Queue<string> itemNotiMessageQue = new Queue<string>();
    List<TMP_Text> itemNotiPool = new List<TMP_Text>();

    long maxHp;
    long nowHP;

    const int MAX_ITEM_NOTI = 5;

    private void Awake()
    {
        skillButton = gameObject.Find<Button>("SkillButton");

        bagIndicator = gameObject.Find("BagIndicator");
        bagIndicatorTmp = bagIndicator.Find<TMP_Text>("Text");
        depthIndicator = gameObject.Find("DepthIndicator");
        depthIndicatorTmp = depthIndicator.Find<TMP_Text>("Text");
        timeIndicator = gameObject.Find("TimeIndicator");
        timeIndicatorTmp = timeIndicator.Find<TMP_Text>("Text");

        inventoryButton.onClick.AddListener(() => Managers.ui.ShowPopup<UIPopupInventory>());
        skillButton.onClick.AddListener(() => Managers.ui.ShowPopup<UIPopupSkill>());
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

        StopTimer();
    }

    private void Update()
    {
        depthIndicatorTmp.text = $"{(-Managers.obj.myPlayer.transform.position.y).ToString("F1")}m";
    }

    public void SetHpBar(long maxHp, long nowHp)
    {
        this.maxHp = maxHp;
        this.nowHP = nowHp;

        hpBar.SetGuage(maxHp, nowHp);
    }

    public void StartReduceHP(long reduceHpPerSec, Action onEndHpCo)
    {
        if (reduceHpPerSec <= 0)
            return;

        if (reduceHpCo != null)
            StopCoroutine(reduceHpCo);

        reduceHpCo = StartCoroutine(ReduceHpCo(reduceHpPerSec, onEndHpCo));
    }

    public void StopReduceHp()
    {
        if (reduceHpCo != null)
            StopCoroutine(reduceHpCo);
    }

    public void StartTimer()
    {
        StopTimer();
        timeIndicator.SetActive(true);
        timerCo = StartCoroutine(TimerCo());
    }

    public void StopTimer()
    {
        timeIndicator.SetActive(false);
        if (timerCo != null)
            StopCoroutine(timerCo);
    }

    public void AddItemNotiQue(string message)
    {
        itemNotiMessageQue.Enqueue(message);
        FlushItemMessageQue();
    }

    public void SetDepthIndicator(bool show)
    {
        depthIndicator.SetActive(show);
    }

    public void SetBagIndicator(long maxWeight, long nowWeight)
    {
        bagIndicatorTmp.text = $"{nowWeight}/{maxWeight}g";
    }

    IEnumerator ReduceHpCo(long reduceHpPerSec, Action onEndReduceHp)
    {
        while (nowHP > 0)
        {
            yield return new WaitForSeconds(1f);
            nowHP -= reduceHpPerSec;
            SetHpBar(maxHp, nowHP);
        }

        onEndReduceHp?.Invoke();
    }

    IEnumerator TimerCo()
    {
        DateTime startTime = DateTime.Now;

        while (true)
        {
            timeIndicatorTmp.text = (DateTime.Now - startTime).ToString(@"mm\:ss");
            yield return new WaitForSeconds(1f);
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
