using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutStartGame : UIPopup
{
    GameObject dim;
    Button startButton;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            startButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dim = gameObject.Find("Dim");
        startButton = gameObject.Find<Button>("StartMineButton");

        startButton.onClick.AddListener(() => OnClickStartGame());
    }

    void OnClickStartGame()
    {
        Debug.Log("start");
        LocalPacketSender.C_GameStart();
        startButton.enabled = false;
    }
}
