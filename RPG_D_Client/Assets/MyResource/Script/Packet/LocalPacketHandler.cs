using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPacketHandler
{
    public static void S_UserInfo(UserData userData)
    {
        Managers.data.SetMyUserData(userData);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userData.maxHp, userData.maxHp);
        Managers.ui.GetLayout<UILayoutEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.obj.myPlayer.speed = userData.speed;
    }

    public static void S_MoveMap(bool moveSuccess, int mapId, List<LodeObject> lodeInfoList, UserGameInfo userInfo, bool showStartGame)
    {
        if (!moveSuccess)
        {
            // TODO
            // Move Room failed procedure;
            return;
        }

        Managers.obj.DestroyAllLode();
        Managers.map.SetMap(mapId);

        foreach (var lodeInfo in lodeInfoList)
        {
            var lode = Managers.obj.InstantiateLode();
            lode.id = lodeInfo.id;
            lode.transform.position = lodeInfo.position;
            lode.gameObject.SetActive(true);
        }

        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userInfo.maxHp, userInfo.maxHp);
        if (showStartGame)
            Managers.ui.GetLayout<UILayoutStartGame>().ShowStartGame(true);
        else
            Managers.obj.myPlayer.SetPlayerMoveLock(false);
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
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{mineral.itemType} {mineral.count}개 획득");
    }

    public static void S_MineGameResult(List<Item> minerals)
    {
        Managers.ui.GetLayout<UILayoutMineHUD>().StopReduceHp();
        Managers.ui.GetLayout<UILayoutGameResult>().SetInventory(minerals);
        Managers.ui.GetLayout<UILayoutGameResult>().ShowGameResult(true);
    }

    public static void S_EnforceEquip(UserData userData, int result)
    {
        if (result == 0)
        {
            Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup("강화 성공", null, null);
        }
        else if (result == 1)
        {
            Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup("강화 실패", null, null);
        }
        else if (result == 2)
        {
            Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup("돈이 부족합니다.", null, null);
        }

        Managers.ui.GetLayout<UILayoutInventory>().SetMoney(userData.money);
        Managers.ui.GetLayout<UILayoutEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetLayout<UILayoutEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.obj.myPlayer.speed = userData.speed;
    }

    public static void S_InventoryInfo(List<Item> minerals, long money)
    {
        Managers.ui.GetLayout<UILayoutInventory>().SetInventory(minerals);
        Managers.ui.GetLayout<UILayoutInventory>().SetMoney(money);
        Managers.ui.GetLayout<UILayoutMineShop>().SetInventory(minerals);
        Managers.ui.GetLayout<UILayoutEquipment>().SetMoney(money);
    }
}
