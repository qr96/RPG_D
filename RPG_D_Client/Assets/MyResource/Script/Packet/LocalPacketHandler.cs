using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketHandler
{
    public static void S_ReqGameInfo(List<LodeObject> lodeInfoList, UserGameInfo userInfo)
    {
        foreach (var lodeInfo in lodeInfoList)
        {
            var lode = Managers.obj.InstantiateLode();
            lode.id = lodeInfo.id;
            lode.transform.position = lodeInfo.position;
            lode.gameObject.SetActive(true);
        }

        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userInfo.maxHp, userInfo.maxHp);
        Managers.ui.GetLayout<UILayoutStartGame>().ShowStartGame(true);
    }

    public static void S_GameStart(bool success, long hpReducePerSec)
    {
        if (success)
        {
            Managers.ui.GetLayout<UILayoutMineHUD>().StartReduceHP(hpReducePerSec);
            Managers.ui.GetLayout<UILayoutStartGame>().ShowStartGame(false);
        }
        else
        {
            // TODO
            // Start game failed
        }
    }

    public static void S_LodeAttackStart(int lodeId, long lodeMaxHp)
    {
        Managers.ui.GetLayout<UILayoutMineGame>().SetMinePopup(lodeMaxHp);
        Managers.ui.GetLayout<UILayoutMineGame>().ShowMinePopup(true);
    }

    public static void S_LodeAttack(long lodeHp, long damage, bool critical)
    {
        Managers.ui.GetLayout<UILayoutMineGame>().ChangeMinePopupHp(lodeHp);

        if (lodeHp <= 0)
            Managers.ui.GetLayout<UILayoutMineGame>().ResultMineGamePopup();
        else
            Managers.ui.GetLayout<UILayoutMineGame>().ChangeMinePopupTargetZone();
    }

    public static void S_LodeAttackResult(int lodeId, List<Item> minerals, float nowWeight)
    {
        Managers.obj.DestroyLode(lodeId);
        foreach (var mineral in minerals)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{mineral.itemType} {mineral.count}�� ȹ��");
    }

    public static void S_MineGameResult(List<Item> minerals)
    {
        Managers.ui.GetLayout<UILayoutGameResult>().SetInventory(minerals);
        Managers.ui.GetLayout<UILayoutGameResult>().ShowGameResult(true);
    }
}
