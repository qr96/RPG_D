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
        Managers.ui.GetLayout<UILayoutMineHUD>().SetHpBar(userData.normalStat.maxHp + userData.equipStat.maxHp, userData.normalStat.maxHp + userData.equipStat.maxHp);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(userData.normalStat.maxWeight + userData.equipStat.maxWeight, userData.nowWeight);
        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(userData.money);
        Managers.ui.GetPopup<UIPopupEquipShop>().UpdatePopup();
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(userData.money);

        Managers.obj.myPlayer.speed = userData.normalStat.speed + userData.equipStat.speed;
        Managers.obj.myPlayer.SetNameTag(userData.nickName);
    }

    public static void S_MoveMapTown(int mapId)
    {
        Managers.obj.DestroyAllLode();
        Managers.map.SetMap(mapId);
        Managers.ui.GetLayout<UILayoutMiniMap>().SetMiniMap(mapId);
        Managers.obj.myPlayer.SetPlayerMoveLock(false);
    }

    public static void S_MoveMapMineGame(int mapId, List<LodeObject> lodeInfoList)
    {
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

        Managers.ui.ShowPopup<UIPopupStartGame>();
        Managers.obj.myPlayer.SetPlayerMoveLock(true);
    }

    public static void S_GameStart(long hpReducePerSec)
    {
        Managers.ui.GetLayout<UILayoutMineHUD>().StartReduceHP(hpReducePerSec, () => LocalPacketSender.C_MineGameResult());
        Managers.ui.HidePopup<UIPopupStartGame>();
        Managers.obj.myPlayer.SetPlayerMoveLock(false);
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

    public static void S_InventoryInfo(List<Item> minerals, long money, long maxWeight, long nowWeight)
    {
        Managers.ui.GetPopup<UIPopupInventory>().SetInventory(minerals);
        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(money);
        Managers.ui.GetPopup<UIPopupMineShop>().SetInventory(minerals);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }
}
