using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    // 추후에 json 혹은 csv 등의 파일로 서버, 클라 공용으로 읽어서 처리할 예정

    #region Minerals
    public static string GetItemSpritePath(int itemType)
    {
        var spriteName = "";
        var prefix = "Sprites/";
        if (itemType == 10001)
            spriteName = "Mineral0";
        else if (itemType == 10002)
            spriteName = "Mineral1";
        else if (itemType == 10003)
            spriteName = "Mineral2";

        return prefix + spriteName;
    }

    public static long GetItemPrice(List<Item> itemList)
    {
        var price = 0L;

        foreach (var item in itemList)
            price += GetItemPrice(item.itemType) * item.count;

        return price;
    }

    public static long GetItemPrice(int itemType)
    {
        var price = 0L;

        if (itemType == 10001)
            price = 100;
        else if (itemType == 10002)
            price = 150;
        else if (itemType == 10003)
            price = 200;

        return price;
    }

    public static long GetMineralWeight(List<Item> itemList)
    {
        var weight = 0L;
        foreach (var item in itemList)
            weight += GetMineralWeight(item.itemType) * item.count;

        return weight;
    }

    public static long GetMineralWeight(int itemType)
    {
        var weight = 0L;

        if (itemType == 10001)
            weight = 1;
        else if (itemType == 10002)
            weight = 2;
        else if (itemType == 10003)
            weight = 3;

        return weight;
    }

    public static string GetMinrealName(int itemType)
    {
        var name = "NONE";

        if (itemType == 10001)
            name = "사암";
        else if (itemType == 10002)
            name = "철광석";
        else if (itemType == 10003)
            name = "흑요석";

        return name;
    }
    #endregion

    #region Lode

    public static string GetLodeSpritePath(int lodeType)
    {
        var spriteName = "";
        var prefix = "Sprites/Lode/";

        spriteName = lodeType.ToString();

        return prefix + spriteName;
    }

    public static Vector2 GetLodeScale(int lodeType)
    {
        Vector2 scale = Vector2.one;

        if (lodeType == 10004)
            scale *= 2;

        return scale;
    }

    public static long GetLodeHp(int lodeType)
    {
        var lodeHp = -1L;

        if (lodeType == 10001)
            lodeHp = 100;
        else if (lodeType == 10002)
            lodeHp = 500;
        else if (lodeType == 10003)
            lodeHp = 1100;
        else if (lodeType == 10004)
            lodeHp = 3000;

        return lodeHp;
    }

    public static int GetRewardMineTicket(int lodeType)
    {
        var ticketType = 0;

        if (lodeType == 10004)
            ticketType = 1002;

        return ticketType;
    }

    #endregion

    #region Equipment
    public static string GetEquipmentSpritePath(int equipType)
    {
        var path = "";
        var prefix = "Sprites/";

        if (equipType >= 3001 && equipType <= 3999)
            prefix += "Weapon/";
        else if (equipType >= 4001 && equipType <= 4999)
            prefix += "Shirt/";
        else if (equipType >= 5001 && equipType <= 5999)
            prefix += "Bag/";
        else if (equipType >= 6001 && equipType <= 6999)
            prefix += "Shoes/";

        path = equipType.ToString();

        return prefix + path;
    }

    public static string GetEquipmentSpriteName(int equipType)
    {
        var name = "-";

        if (equipType == 3001)
            name = "흙수저";
        else if (equipType == 3002)
            name = "은수저";
        else if (equipType == 3003)
            name = "금수저";
        else if (equipType == 3004)
            name = "플래티넘 수저";
        else if (equipType == 3005)
            name = "흙곡괭이";
        else if (equipType == 3006)
            name = "은곡괭이";
        else if (equipType == 3007)
            name = "금곡괭이";
        else if (equipType == 3008)
            name = "플래티넘곡괭이";
        else if (equipType == 4001)
            name = "비닐봉지옷";
        else if (equipType == 4002)
            name = "신문지옷";
        else if (equipType == 4003)
            name = "허름한 메리야스";
        else if (equipType == 4004)
            name = "메리야스";
        else if (equipType == 5001)
            name = "비닐봉투";
        else if (equipType == 5002)
            name = "신문지보따리";
        else if (equipType == 5003)
            name = "대형마트 쇼핑백";
        else if (equipType == 5004)
            name = "초등학생 책가방";
        else if (equipType == 5005)
            name = "메이커 가방";
        else if (equipType == 5006)
            name = "원가 8만원 명품백";

        else if (equipType == 6001)
            name = "비닐봉지 신발";
        else if (equipType == 6002)
            name = "헤진 신발";
        else if (equipType == 6003)
            name = "중고 독일군 스니커즈";
        else if (equipType == 6004)
            name = "검은 캔버스화";
        else if (equipType == 6005)
            name = "메이커 신발";

        return name;
    }

    public static Stat GetEquipmentStat(int equipType, int level)
    {
        Stat stat = GetEquipmentIncreaseStat(equipType);
        stat.attack *= level;
        stat.maxHp *= level;
        stat.maxWeight *= level;
        stat.speed *= level;
        return stat;
    }

    public static Stat GetEquipmentIncreaseStat(int equipType)
    {
        Stat stat = new Stat();

        if (equipType >= 3001 && equipType <= 3999)
            stat.attack += (long)Mathf.Pow(2, equipType - 3001);
        else if (equipType >= 4001 && equipType <= 4999)
            stat.maxHp += (long)Mathf.Pow(2, equipType - 4001) * 5;
        else if (equipType >= 5001 && equipType <= 5999)
            stat.maxWeight += (long)Mathf.Pow(2, equipType - 5001) * 5;
        else if (equipType >= 6001 && equipType <= 6999)
            stat.speed += equipType - 6000;

        return stat;
    }

    public static long GetEquipmentEnhancePrice(int equipType, int level)
    {
        long price = 0;

        if (equipType >= 3001 && equipType <= 3999)
            price = (long)(Mathf.Pow(3, equipType - 3001) * 1000);
        else if (equipType >= 4001 && equipType <= 4999)
            price = (long)(Mathf.Pow(3, equipType - 4001) * 1000);
        else if (equipType >= 5001 && equipType <= 5999)
            price = (long)(Mathf.Pow(3, equipType - 5001) * 1000);
        else if (equipType >= 6001 && equipType <= 6999)
            price = (long)(Mathf.Pow(10, equipType - 6001) * 1000);
        return price;
    }
    #endregion

    #region Quest

    public static bool IsQuestComplete(int questId, UserData userData)
    {
        bool complted = false;

        if (questId == 0)
        {

        }

        return complted;
    }

    #endregion
}
