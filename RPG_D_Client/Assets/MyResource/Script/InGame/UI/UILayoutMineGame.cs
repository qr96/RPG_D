using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineGame : UIPopup
{
    TimingGuage timingGuage;
    GuageBar hpBar;
    Button stopButton;

    long lodeMaxHp = 0;

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

        stopButton.onClick.AddListener(() => OnClickMinePopupAttack());
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
        LocalPacketSender.C_LodeAttack(result);
    }
}
