using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGameScene : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;

        // Recv my town 1001
        //LocalPacketSender.C_UserInfo(123456);
        //LocalPacketSender.C_MoveMap(1001);
        //Managers.map.SetMap(1001);

        Managers.ui.ShowPopup<UIPopupLogin>();
    }
}
