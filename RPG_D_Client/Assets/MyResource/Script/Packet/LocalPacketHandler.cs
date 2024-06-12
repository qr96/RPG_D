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
        Managers.ui.GetPopup<UIPopupEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetMoney(userData.money);
        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(userData.money);
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
            Managers.ui.ShowPopup<UIPopupStartGame>();
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
            Managers.ui.HidePopup<UIPopupStartGame>();
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
        Managers.ui.ShowPopup<UIPopupMineGame>().SetMinePopup(lodeMaxHp);
    }

    public static void S_LodeAttack(long lodeHp, long damage, bool critical)
    {
        Managers.ui.GetPopup<UIPopupMineGame>().ChangeMinePopupHp(lodeHp);
        Managers.ui.GetPopup<UIPopupMineGame>().ShowDamageBar(damage);

        if (lodeHp <= 0)
            Managers.ui.GetPopup<UIPopupMineGame>().ResultMineGamePopup();
        else
            Managers.ui.GetPopup<UIPopupMineGame>().ChangeMinePopupTargetZone();
    }

    public static void S_LodeAttackResult(int lodeId, List<Item> minerals, long maxWeight, long nowWeight)
    {
        Managers.obj.myPlayer.SetPlayerMoveLock(false);
        Managers.obj.DestroyLode(lodeId);

        foreach (var mineral in minerals)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{DataTable.GetMinrealName(mineral.itemType)} {mineral.count}�� ȹ��");
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }

    public static void S_MineGameResult(bool success, List<Item> minerals)
    {
        Managers.obj.myPlayer.SetPlayerMoveLock(true);
        Managers.ui.GetLayout<UILayoutMineHUD>().StopReduceHp();
        Managers.ui.ShowPopup<UIPopupGameResult>().SetResultPopup(success, minerals);
    }

    public static void S_EnforceEquip(UserData userData, int result)
    {
        if (result == 0)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("��ȭ ����");
        }
        else if (result == 1)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("��ȭ ����");
        }
        else if (result == 2)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("���� �����մϴ�.");
        }

        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(userData.money);
        Managers.ui.GetPopup<UIPopupEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetMoney(userData.money);
        Managers.obj.myPlayer.speed = userData.speed;
    }

    public static void S_InventoryInfo(List<Item> minerals, long money, long maxWeight, long nowWeight)
    {
        Managers.ui.GetPopup<UIPopupInventory>().SetInventory(minerals);
        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(money);
        Managers.ui.GetPopup<UIPopupMineShop>().SetInventory(minerals);
        Managers.ui.GetPopup<UIPopupEquipment>().SetMoney(money);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }
}
