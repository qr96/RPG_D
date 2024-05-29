using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketSender
{
    public static void C_GameStart()
    {
        LocalServer.Instance.Recv();
    }

    public static void C_LodeAttackStart(int lodeId)
    {
        LocalServer.Instance.C_LodeAttackStart(lodeId);
    }

    public static void C_LodeAttack(int attackLevel)
    {

    }
}
