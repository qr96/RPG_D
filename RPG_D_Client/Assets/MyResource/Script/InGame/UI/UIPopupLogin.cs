using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupLogin : UIPopup
{
    TMP_InputField nicknameField;
    Button loginButton;

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loginButton.onClick.Invoke();
        }
    }

    public override void OnCreate()
    {
        nicknameField = gameObject.Find<TMP_InputField>("NicknamePopup/NicknameField");
        loginButton = gameObject.Find<Button>("LoginButton");

        loginButton.onClick.AddListener(() => OnClickLoginButton());
    }

    void OnClickLoginButton()
    {
        if (!string.IsNullOrEmpty(nicknameField.text))
        {
            LocalPacketSender.C_Login(nicknameField.text);
            Managers.Network.Init();
            Managers.Network.Send(new C_LoginGame()
            {
                Name = nicknameField.text
            });
        }
    }
}
