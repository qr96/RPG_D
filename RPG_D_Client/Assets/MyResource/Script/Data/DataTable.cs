using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    // ���Ŀ� json Ȥ�� csv ���� ���Ϸ� ����, Ŭ�� �������� �о ó���� ����

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
            name = "���";
        else if (itemType == 10002)
            name = "ö����";
        else if (itemType == 10003)
            name = "��伮";

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

    public static long GetMakeEquipPrice(int anvilLevel)
    {
        return (anvilLevel * 1000);
    }

    public static string GetEquipmentSpritePath(int equipType)
    {
        var path = "";
        var prefix = "Sprites/Weapon/";

        path = equipType.ToString();

        Debug.Log(path);

        return prefix + path;
    }

    public static string GetEquipmentSpriteName(int equipType)
    {
        var name = "-";

        if (equipType == 3001)
            name = "�����";
        else if (equipType == 3002)
            name = "������";
        else if (equipType == 3003)
            name = "�ݼ���";
        else if (equipType == 3004)
            name = "�÷�Ƽ�� ����";
        else if (equipType == 3005)
            name = "����";
        else if (equipType == 3006)
            name = "�����";
        else if (equipType == 3007)
            name = "�ݰ��";
        else if (equipType == 3008)
            name = "�÷�Ƽ�Ѱ��";

        return name;
    }

    public static long GetEquipmentStat(int equipType)
    {
        long stat = 0;

        if (equipType == 3001)
            stat = 10;

        return stat;
    }
    #endregion
}
