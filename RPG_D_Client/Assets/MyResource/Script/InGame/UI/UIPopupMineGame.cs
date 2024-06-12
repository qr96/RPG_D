using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupMineGame : UIPopup
{
    TimingGuage timingGuage;
    GuageBar hpBar;
    Button stopButton;
    Image attackEffet;
    TMP_Text resultEffect;
    GameObject damages;
    TMP_Text damagePrefab;

    long lodeMaxHp = 0;

    Queue<TMP_Text> damagePool = new Queue<TMP_Text>();

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            stopButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        timingGuage = gameObject.Find<TimingGuage>("TimingGuage");
        hpBar = gameObject.Find<GuageBar>("HPGuage");
        stopButton = gameObject.Find<Button>("StopButton");
        attackEffet = gameObject.Find<Image>("Effect");
        resultEffect = gameObject.Find<TMP_Text>("Result");
        damages = gameObject.Find("Damages");
        damagePrefab = damages.Find<TMP_Text>("Damage");

        stopButton.onClick.AddListener(() => OnClickMinePopupAttack());

        damagePool.Enqueue(damagePrefab);
        damagePrefab.gameObject.SetActive(false);
    }

    public void SetMinePopup(long lodeMaxHp)
    {
        var moveTime = 0.5f;
        var orangePer = 0.75f;
        var yellowPer = 0.4f;
        var greenPer = 0.1f;
        var orangeTime = moveTime * orangePer;
        var targetTime = Random.Range(orangeTime / 2f, moveTime - orangeTime / 2f);

        timingGuage.SetGuage(moveTime, targetTime, orangePer, yellowPer, greenPer);
        timingGuage.StartMove();

        hpBar.SetGuage(lodeMaxHp, lodeMaxHp);
        this.lodeMaxHp = lodeMaxHp;

        attackEffet.color = new Color(0f, 0f, 0f, 0f);
        resultEffect.text = "";
    }

    public void ResultMineGamePopup()
    {
        timingGuage.StopMove();
        StartCoroutine(ShowResult());

        IEnumerator ShowResult()
        {
            yield return new WaitForSeconds(0.3f);
            Hide();
        }
    }

    public void ChangeMinePopupHp(long lodeNowHp)
    {
        hpBar.SetGuage(lodeMaxHp, lodeNowHp);
    }

    public void ShowDamageBar(long damage)
    {
        TMP_Text damageTmp;

        if (damagePool.Count <= 0)
            damageTmp = Instantiate(damagePrefab, damages.transform);
        else
            damageTmp = damagePool.Dequeue();

        var damageColor = damageTmp.color;
        damageColor.a = 1f;
        damageTmp.color = damageColor;
        damageTmp.rectTransform.anchoredPosition = new Vector2(0f, -70f);
        damageTmp.gameObject.SetActive(true);
        damageTmp.text = damage.ToString();

        damageTmp.rectTransform.DOAnchorPosY(82f, 2f);
        damageTmp.DOFade(0f, 2f)
            .OnComplete(() =>
            {
                damageTmp.gameObject.SetActive(false);
                damagePool.Enqueue(damageTmp);
            });
    }

    public void ChangeMinePopupTargetZone()
    {
        var moveTime = 0.5f;
        var orangePer = 0.75f;
        var orangeTime = moveTime * orangePer;
        var targetTime = Random.Range(orangeTime / 2f, moveTime - orangeTime / 2f);

        timingGuage.ChanageTargetZone(targetTime);
    }

    void OnClickMinePopupAttack()
    {
        var result = timingGuage.GetPointResult();
        Debug.Log($"[{GetType().Name}] OnClickMinePopupAttack(), result:{result}");

        var pointer = timingGuage.gameObject.Find("Pointer");
        attackEffet.transform.position = pointer.transform.position;
        attackEffet.color = Color.white;
        attackEffet.DOFade(0f, 0.2f);

        if (result == 0)
            resultEffect.text = "PERFECT";
        else if (result == 1)
            resultEffect.text = "GOOD";
        else if (result == 2)
            resultEffect.text = "SOSO";
        else
            resultEffect.text = "BAD";

        LocalPacketSender.C_LodeAttack(result);
    }
}
