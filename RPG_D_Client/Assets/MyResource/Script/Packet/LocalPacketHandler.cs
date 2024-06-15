using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalPacketHandler
{
    public static void S_Login(bool success)
    {
        if (success)
        {
            Managers.ui.HidePopup<UIPopupLogin>();
            LocalPacketSender.C_UserInfo(0);
        }
        else
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("에러가 발생했습니다.\n재시도 부탁드립니다.");
        }
    }

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
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(userData.equipmentDic.Values.ToList());
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(userData.money);

        Managers.obj.myPlayer.speed = userData.speed;
        Managers.obj.myPlayer.SetNameTag(userData.nickName);
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

        if (minerals.Count <= 0)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"가방이 가득 차 획득할 수 없습니다.");
        foreach (var mineral in minerals)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{DataTable.GetMinrealName(mineral.itemType)} {mineral.count}개 획득");
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
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("강화 성공");
        }
        else if (result == 1)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("강화 실패");
        }
        else if (result == 2)
        {
            Managers.ui.ShowPopup<UIPopupNotice>().SetPopup("돈이 부족합니다.");
        }

        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(userData.money);
        Managers.ui.GetPopup<UIPopupEquipment>().SetStat(userData.attack.ToString(), userData.maxHp.ToString(), userData.speed.ToString());
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(0, userData.weaponLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(1, userData.armorLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetEquipLevel(2, userData.shoesLevel);
        Managers.ui.GetPopup<UIPopupEquipment>().SetMoney(userData.money);
        Managers.obj.myPlayer.speed = userData.speed;
    }

    public static void S_EquipmentList(Dictionary<int, Equipment> equipmentList)
    {
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(equipmentList.Values.ToList());
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
