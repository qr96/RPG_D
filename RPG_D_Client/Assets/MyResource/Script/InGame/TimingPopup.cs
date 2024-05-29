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

    // Setting
    Action<int> onStop;

    // Caching
    Coroutine stopMoveCo;

    public void SetButtonEvent(Action onClick)
    {
        stopButton.onClick.RemoveAllListeners();
        stopButton.onClick.AddListener(() => onClick());
    }

    public void SetHpBar(long maxHp, long nowHp)
    {
        hpBar.SetGuage(maxHp, nowHp);
    }

    public void StartMove(float moveTime, float targetTime, float orangePer, float yellowPer, float greenPer, Action<int> onStop)
    {
        this.onStop = onStop;

        if (stopMoveCo != null)
            StopCoroutine(stopMoveCo);

        timingGuage.StartMove(moveTime, targetTime, orangePer, yellowPer, greenPer);
    }

    public void StopMove()
    {
        var result = timingGuage.StopMove();

        if (stopMoveCo != null)
            StopCoroutine(stopMoveCo);
        stopMoveCo = StartCoroutine(StopMoveCo(result));
    }

    IEnumerator StopMoveCo(int result)
    {
        yield return new WaitForSeconds(0.5f);

        if (onStop != null)
            onStop.Invoke(result);
    }
}
