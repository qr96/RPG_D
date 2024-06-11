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
            price = 1000;
        else if (itemType == 10002)
            price = 1500;
        else if (itemType == 10003)
            price = 2000;

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

    #region Equipment
    public static long GetEquipEnhancePrice(int nowLevel)
    {
        return 1000 + nowLevel * 1000;
    }

    static float[] successPercentage = new float[10] { 90f, 85f, 80f, 75f, 60f, 50f, 40f, 30f, 20f, 20f };
    public static float GetEquipEnhanceSuccessPercent(int nowLevel)
    {
        if (nowLevel < 10)
            return successPercentage[nowLevel];
        else
            return 20f;
    }
    #endregion
}
