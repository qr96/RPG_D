using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketSender
{
    public static void C_ReqGameInfo(int mapId)
    {
        LocalServer.Instance.C_ReqGameInfo(mapId);
    }

    public static void C_GameStart()
    {
        LocalServer.Instance.C_GameStart();
    }

    public static void C_LodeAttackStart(int lodeId)
    {
        LocalServer.Instance.C_LodeAttackStart(lodeId);
    }

    public static void C_LodeAttack(int attackLevel)
    {
        LocalServer.Instance.C_LodeAttack(attackLevel);
    }

    public static void C_MineGameResult()
    {
        LocalServer.Instance.C_MineGameResult();
    }
}
