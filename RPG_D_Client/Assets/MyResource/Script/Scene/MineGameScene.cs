using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGameScene : MonoBehaviour
{
    void Start()
    {
        LocalPacketSender.C_GameStart();
    }
}
