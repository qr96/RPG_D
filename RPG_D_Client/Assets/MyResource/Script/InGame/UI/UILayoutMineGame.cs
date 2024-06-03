using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineGame : UILayout
{
    public Button mineStartButton;
    public TimingPopup minePopup;

    int targetLodeId;

    private void Awake()
    {
        mineStartButton.onClick.AddListener(OnClickMineStartButton);
    }

    private void Start()
    {
        ShowMineStartButton(false);
        ShowMinePopup(false);
    }

    public void ShowMineStartButton(bool show, int targetLodeId = 0)
    {
        mineStartButton.gameObject.SetActive(show);

        if (show)
        {
            Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickMineStartButton);
            this.targetLodeId = targetLodeId;
        }
        else
            Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickMineStartButton);
    }

    public void SetMinePopup(long lodeMaxHp)
    {
        minePopup.SetHpBar(lodeMaxHp, lodeMaxHp);
    }

    public void ShowMinePopup(bool show)
    {
        minePopup.gameObject.SetActive(show);
        Managers.obj.myPlayer.SetPlayerMoveLock(show);

        if (show)
        {
            var orangeTime = 0.75f;
            var targetTime = Random.Range(orangeTime / 2f, 1 - orangeTime / 2f);

            minePopup.StartMove(1f, targetTime, 0.75f, 0.4f, 0.1f);
            Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickMinePopupAttack);
            minePopup.SetButtonEvent(OnClickMinePopupAttack);
        }
        else
        {
            Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickMinePopupAttack);
            minePopup.RemoveButtonEvent();
        }
    }

    public void ResultMineGamePopup()
    {
        minePopup.StopMove();
        Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickMinePopupAttack);
        minePopup.RemoveButtonEvent();
        StartCoroutine(ShowResult());

        IEnumerator ShowResult()
        {
            yield return new WaitForSeconds(0.3f);
            ShowMinePopup(false);
        }
    }

    public void ChangeMinePopupHp(long lodeNowHp)
    {
        minePopup.ChangeHpBar(lodeNowHp);
    }

    public void ChangeMinePopupTargetZone()
    {
        var targetTime = Random.Range(0.375f, 0.625f);
        minePopup.ChanageTargetZone(targetTime);
    }

    void OnClickMineStartButton()
    {
        LocalPacketSender.C_LodeAttackStart(targetLodeId);
        ShowMineStartButton(false);
    }

    void OnClickMinePopupAttack()
    {
        var result = minePopup.GetPointResult();
        Debug.Log($"[{GetType().Name}] OnClickMinePopupAttack(), result:{result}");
        LocalPacketSender.C_LodeAttack(result);
    }
}
