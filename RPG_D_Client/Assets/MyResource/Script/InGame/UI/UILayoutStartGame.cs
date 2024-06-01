using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutStartGame : UILayout
{
    GameObject dim;
    Button startButton;

    private void Awake()
    {
        dim = gameObject.Find("Dim");
        startButton = gameObject.Find<Button>("StartMineButton");

        startButton.onClick.AddListener(() => OnClickStartGame());
    }

    public void ShowStartGame(bool show)
    {
        dim.SetActive(show);
        startButton.gameObject.SetActive(show);
        startButton.enabled = show;

        if (show)
            Managers.input.AddKeyDownEvent(KeyCode.Space, OnClickStartGame);
        else
            Managers.input.RemoveKeyDownEvent(KeyCode.Space, OnClickStartGame);
    }

    void OnClickStartGame()
    {
        LocalPacketSender.C_GameStart();
        startButton.enabled = false;
    }
}
