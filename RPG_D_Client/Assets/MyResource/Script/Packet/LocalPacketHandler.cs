using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketHandler
{
    public static void S_GameStart(List<LodeObject> lodeInfoList, float hpReducePerSec, UserInfo userInfo)
    {
        
    }

    public static void S_LodeAttackStart(int lodeId, long lodeMaxHp)
    {
        Managers.Instance.ui.SetMinePopup(lodeMaxHp);
        Managers.Instance.ui.ShowMinePopup(true);
    }

    public static void S_LodeAttack(long lodeHp, long damage, bool critical)
    {
        Managers.Instance.ui.ChangeMinePopupHp(lodeHp);

        if (lodeHp <= 0)
            Managers.Instance.ui.StopMinePopup();
    }
}
