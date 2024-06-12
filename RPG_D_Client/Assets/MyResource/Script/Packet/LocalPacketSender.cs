using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketSender
{
    public static void C_Login(string nickname)
    {
        LocalServer.Instance.C_Login(nickname);
    }

    public static void C_UserInfo(int uid)
    {
        LocalServer.Instance.C_UserInfo(uid);
    }

    public static void C_MoveMap(int mapId)
    {
        LocalServer.Instance.C_MoveMap(mapId);
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

    public static void C_SellItem(bool sellAll, int itemType, long count)
    {
        LocalServer.Instance.C_SellItem(sellAll, itemType, count);
    }

    public static void C_EnforceEquip(int equipType)
    {
        LocalServer.Instance.C_EnforceEquip(equipType);
    }
}
