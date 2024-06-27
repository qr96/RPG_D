
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        Managers.ui.GetPopup<UIPopupInventory>().SetInventory(userData.consumableDic.Values.ToList());
        Managers.ui.GetPopup<UIPopupEquipShop>().UpdatePopup();
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(userData.money);
        Managers.ui.GetPopup<UIPopupSkill>().SetSkills(userData.skillDic.Values.ToList());

        Managers.obj.myPlayer.speed = userData.normalStat.speed + userData.equipStat.speed;
        Managers.obj.myPlayer.SetNameTag(userData.nickName);
    }

    public static void S_MoveMapTown(int mapId)
    {
        Managers.obj.DestroyAllLode();
        Managers.map.SetMap(mapId);
        Managers.ui.GetLayout<UILayoutMiniMap>().SetMiniMap(mapId);
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
            lode.SetSprite(DataTable.GetLodeSpritePath(lodeInfo.lodeType));
            lode.transform.localScale = DataTable.GetLodeScale(lodeInfo.lodeType);
            lode.gameObject.SetActive(true);
        }

        Managers.ui.ShowPopup<UIPopupStartGame>();
    }

    public static void S_GameStart(long hpReducePerSec)
    {
        Managers.ui.GetLayout<UILayoutMineHUD>().StartReduceHP(hpReducePerSec, () => LocalPacketSender.C_MineGameResult());
        Managers.ui.GetLayout<UILayoutMineHUD>().StartTimer();
        Managers.ui.HidePopup<UIPopupStartGame>();
    }

    public static void S_LodeAttackStart(int lodeId, long lodeMaxHp, long userMaxMp)
    {
        Managers.ui.ShowPopup<UIPopupMineGame>().SetMinePopup(lodeMaxHp, userMaxMp);
    }

    public static void S_LodeAttack(long lodeHp, List<long> damages, long userMaxMp, long userNowMp)
    {
        Managers.ui.GetPopup<UIPopupMineGame>().ChangeMinePopupHp(lodeHp);
        Managers.ui.GetPopup<UIPopupMineGame>().SetMpBar(userMaxMp, userNowMp);
        Managers.ui.GetPopup<UIPopupMineGame>().ShowDamageBar(damages);

        if (lodeHp <= 0)
            Managers.ui.GetPopup<UIPopupMineGame>().ResultMineGamePopup();
        else if (userNowMp <= 0)
        {
            Managers.ui.GetPopup<UIPopupMineGame>().ResultMineGamePopup();
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"기력이 부족합니다.");
        }
        else
            Managers.ui.GetPopup<UIPopupMineGame>().ChangeMinePopupTargetZone();
    }

    public static void S_LodeAttackResult(int lodeId, List<Item> minerals, long maxWeight, long nowWeight)
    {
        Managers.obj.DestroyLode(lodeId);

        if (nowWeight >= maxWeight)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"가방이 가득 차 획득할 수 없습니다.");

        foreach (var mineral in minerals)
            Managers.ui.GetLayout<UILayoutMineHUD>().AddItemNotiQue($"{DataTable.GetMinrealName(mineral.itemType)} {mineral.count}개 획득");

        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }

    public static void S_MineGameResult(bool success, List<Item> minerals)
    {
        Managers.ui.GetLayout<UILayoutMineHUD>().StopReduceHp();
        Managers.ui.GetLayout<UILayoutMineHUD>().StopTimer();
        Managers.ui.ShowPopup<UIPopupGameResult>().SetResultPopup(success, minerals);
    }

    public static void S_InventoryInfo(List<Item> minerals, long money, long maxWeight, long nowWeight)
    {
        Managers.ui.GetPopup<UIPopupInventory>().SetMoney(money);
        Managers.ui.GetPopup<UIPopupMineShop>().SetInventory(minerals);
        Managers.ui.GetPopup<UIPopupEquipShop>().SetPopup(money);
        Managers.ui.GetLayout<UILayoutMineHUD>().SetBagIndicator(maxWeight, nowWeight);
    }

    public static void S_LearnNewSkill(Skill newSkill)
    {
        Managers.ui.ShowPopup<UIPopupSkillInfo>().Set(
            DataTable.GetSkillName(newSkill.type) + " 획득!",
            DataTable.GetSkillInfo(newSkill.type, newSkill.level),
            Resources.Load<Sprite>(DataTable.GetSkillSpritePath(newSkill.type)),
            newSkill.level,
            DataTable.GetSkillMaxExp(newSkill.level),
            newSkill.exp);
    }
}
