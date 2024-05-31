using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingPopup : MonoBehaviour
{
    public TimingGuage timingGuage;
    public GuageBar hpBar;
    public Button stopButton;

    // Caching
    long maxHp;

    public void SetButtonEvent(Action onClick)
    {
        stopButton.onClick.RemoveAllListeners();
        stopButton.onClick.AddListener(() => onClick());
    }

    public void RemoveButtonEvent()
    {
        stopButton.onClick.RemoveAllListeners();
    }

    public void SetHpBar(long maxHp, long nowHp)
    {
        this.maxHp = maxHp;
        hpBar.SetGuage(maxHp, nowHp);
    }

    public void ChangeHpBar(long nowHp)
    {
        hpBar.SetGuage(maxHp, nowHp);
    }

    public void StartMove(float moveTime, float targetTime, float orangePer, float yellowPer, float greenPer)
    {
        timingGuage.SetGuage(moveTime, targetTime, orangePer, yellowPer, greenPer);
        timingGuage.StartMove();
    }

    public void StopMove()
    {
        timingGuage.StopMove();
    }

    public void ChanageTargetZone(float targetTime)
    {
        timingGuage.ChanageTargetZone(targetTime);
    }

    public int GetPointResult()
    {
        var result = timingGuage.GetPointResult();
        return result;
    }
}
