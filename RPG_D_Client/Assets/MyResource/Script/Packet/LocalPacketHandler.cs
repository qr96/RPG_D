using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketHandler
{
    public static void S_GameStart(List<LodeObject> lodeInfoList, long hpReducePerSec, UserGameInfo userInfo)
    {
        foreach (var lodeInfo in lodeInfoList)
        {
            var lode = Managers.Instance.obj.InstantiateLode();
            lode.id = lodeInfo.id;
            lode.transform.position = lodeInfo.position;
            lode.gameObject.SetActive(true);
        }

        Managers.Instance.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userInfo.maxHp, userInfo.maxHp);
        Managers.Instance.ui.GetLayout<UILayoutMineHUD>().StartReduceHP(hpReducePerSec);
    }

    public static void S_LodeAttackStart(int lodeId, long lodeMaxHp)
    {
        Managers.Instance.ui.GetLayout<UILayoutMineGame>().SetMinePopup(lodeMaxHp);
        Managers.Instance.ui.GetLayout<UILayoutMineGame>().ShowMinePopup(true);
    }

    public static void S_LodeAttack(long lodeHp, long damage, bool critical)
    {
        Managers.Instance.ui.GetLayout<UILayoutMineGame>().ChangeMinePopupHp(lodeHp);

        if (lodeHp <= 0)
            Managers.Instance.ui.GetLayout<UILayoutMineGame>().ResultMineGamePopup();
        else
            Managers.Instance.ui.GetLayout<UILayoutMineGame>().ChangeMinePopupTargetZone();
    }

    public static void S_LodeAttackResult(int lodeId, List<int> minerals, float nowWeight)
    {
        Managers.Instance.obj.DestroyLode(lodeId);
        foreach (var mineral in minerals)
            Managers.Instance.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue("»×»× 99.9K°³ È¹µæ");
    }
}
