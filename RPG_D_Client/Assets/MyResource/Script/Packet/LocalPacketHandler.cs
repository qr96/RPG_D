using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalPacketHandler
{
    public static void S_UserInfo(UserData userData)
    {
        Managers.data.SetMyUserData(userData);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userData.maxHp, userData.maxHp);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(userData.maxWeight, userData.nowWeight);
        Managers.ui.GetPopup<UILayoutEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetMoney(userData.money);
        Managers.ui.GetPopup<UILayoutInventory>().SetMoney(userData.money);
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
        Managers.ui.GetLayout<UILayoutMiniMap>().SetMiniMap(mapId);

        foreach (var lodeInfo in lodeInfoList)
        {
            var lode = Managers.obj.InstantiateLode();
            lode.id = lodeInfo.id;
            lode.transform.position = lodeInfo.position;
            lode.gameObject.SetActive(true);
        }

        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userInfo.maxHp, userInfo.maxHp);
        if (showStartGame)
        {
            Managers.ui.ShowPopup<UILayoutStartGame>();
            Managers.obj.myPlayer.SetPlayerMoveLock(true);
        }
        else
            Managers.obj.myPlayer.SetPlayerMoveLock(false);
    }

    public static void S_GameStart(bool success, long hpReducePerSec)
    {
        if (success)
        {
            Managers.ui.GetLayout<UILayoutMineHUD>().StartReduceHP(hpReducePerSec, () => LocalPacketSender.C_MineGameResult());
            Managers.ui.HidePopup<UILayoutStartGame>();
            Managers.obj.myPlayer.SetPlayerMoveLock(false);
        }
        else
        {
            // TODO
            // Start game failed
        }
    }

    public static void S_LodeAttackStart(int lodeId, long lodeMaxHp)
    {
        Managers.obj.myPlayer.SetPlayerMoveLock(true);
        Managers.ui.ShowPopup<UILayoutMineGame>().SetMinePopup(lodeMaxHp);
    }

    public static void S_LodeAttack(long lodeHp, long damage, bool critical)
    {
        Managers.ui.GetPopup<UILayoutMineGame>().ChangeMinePopupHp(lodeHp);

        if (lodeHp <= 0)
            Managers.ui.GetPopup<UILayoutMineGame>().ResultMineGamePopup();
        else
            Managers.ui.GetPopup<UILayoutMineGame>().ChangeMinePopupTargetZone();
    }

    public static void S_LodeAttackResult(int lodeId, List<Item> minerals, long maxWeight, long nowWeight)
    {
        Managers.obj.myPlayer.SetPlayerMoveLock(false);
        Managers.obj.DestroyLode(lodeId);
        foreach (var mineral in minerals)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{mineral.itemType} {mineral.count}개 획득");
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }

    public static void S_MineGameResult(bool success, List<Item> minerals)
    {
        Managers.obj.myPlayer.SetPlayerMoveLock(true);
        Managers.ui.GetLayout<UILayoutMineHUD>().StopReduceHp();
        Managers.ui.ShowPopup<UILayoutGameResult>().SetResultPopup(success, minerals);
    }

    public static void S_EnforceEquip(UserData userData, int result)
    {
        if (result == 0)
        {
            Managers.ui.ShowPopup<UILayoutNotice>().SetPopup("강화 성공", null);
        }
        else if (result == 1)
        {
            Managers.ui.ShowPopup<UILayoutNotice>().SetPopup("강화 실패", null);
        }
        else if (result == 2)
        {
            Managers.ui.ShowPopup<UILayoutNotice>().SetPopup("돈이 부족합니다.", null);
        }

        Managers.ui.GetPopup<UILayoutInventory>().SetMoney(userData.money);
        Managers.ui.GetPopup<UILayoutEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.ui.GetPopup<UILayoutEquipment>().SetMoney(userData.money);
        Managers.obj.myPlayer.speed = userData.speed;
    }

    public static void S_InventoryInfo(List<Item> minerals, long money)
    {
        Managers.ui.GetPopup<UILayoutInventory>().SetInventory(minerals);
        Managers.ui.GetPopup<UILayoutInventory>().SetMoney(money);
        Managers.ui.GetPopup<UILayoutMineShop>().SetInventory(minerals);
        Managers.ui.GetPopup<UILayoutEquipment>().SetMoney(money);
    }
}
