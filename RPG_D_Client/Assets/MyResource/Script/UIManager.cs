using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button mineButton;
    public TimingPopup minePopup;

    private void Awake()
    {
        mineButton.onClick.AddListener(OnClickMineButton);
        minePopup.SetButtonEvent(OnClickMinePopupAttack);
    }

    private void Start()
    {
        ShowMineButton(false);
        ShowMinePopup(false);
    }

    public void ShowMineButton(bool show)
    {
        mineButton.gameObject.SetActive(show);

        if (show)
            Managers.Instance.input.AddKeyDownEvent(KeyCode.Space, OnClickMineButton);
        else
            Managers.Instance.input.RemoveKeyDownEvent(KeyCode.Space, OnClickMineButton);
    }

    public void SetMinePopup(long lodeMaxHp)
    {
        minePopup.SetHpBar(lodeMaxHp, lodeMaxHp);
    }

    public void ChangeMinePopupHp(long lodeNowHp)
    {
        var targetTime = Random.Range(0.375f, 0.625f);

        minePopup.ChanageTargetZone(targetTime);
        minePopup.ChangeHpBar(lodeNowHp);
    }

    public void StopMinePopup()
    {
        minePopup.StopMove();
    }

    public void ChangeTargetTime(float targetTime)
    {
        minePopup.ChanageTargetZone(targetTime);
    }

    public void ShowMinePopup(bool show)
    {
        var orangeTime = 0.75f;
        var targetTime = Random.Range(orangeTime / 2f, 1 - orangeTime / 2f);

        minePopup.gameObject.SetActive(show);
        minePopup.StartMove(1f, targetTime, 0.75f, 0.4f, 0.1f, OnStopMinePopup);

        if (show)
            Managers.Instance.input.AddKeyDownEvent(KeyCode.Space, OnClickMinePopupAttack);
        else
            Managers.Instance.input.RemoveKeyDownEvent(KeyCode.Space, OnClickMinePopupAttack);
    }

    void OnClickMineButton()
    {
        LocalPacketSender.C_LodeAttackStart(0);
        ShowMineButton(false);
    }

    void OnClickMinePopupAttack()
    {
        minePopup.GetPointResult();
    }

    void OnStopMinePopup(int result)
    {
        if (result == 0)
            Debug.Log("OnGreenZone");
        else if (result == 1)
            Debug.Log("OnYellowZone");
        else if (result == 2)
            Debug.Log("OnOrangeZone");
        else
            Debug.Log("OnRedZone");

        LocalPacketSender.C_LodeAttack(result);
    }
}
