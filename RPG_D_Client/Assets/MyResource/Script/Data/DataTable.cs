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
    public static string GetEquipmentSpritePath(int equipType)
    {
        var path = "";
        var prefix = "Sprites/";

        if (equipType >= 3001 && equipType <= 3999)
            prefix += "Weapon/";
        else if (equipType >= 4001 && equipType <= 4999)
            prefix += "Shirt/";

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
        else if (equipType == 4001)
            name = "��Һ�����";
        else if (equipType == 4002)
            name = "�Ź�����";
        else if (equipType == 4003)
            name = "�㸧�� �޸��߽�";
        else if (equipType == 4004)
            name = "�޸��߽�";

        return name;
    }

    public static long GetEquipmentStat(int equipType, int level)
    {
        long stat = 0;
        stat = GetEquipmentAddedStat(equipType, level) * level;
        return stat;
    }

    public static long GetEquipmentAddedStat(int equipType, int nowLevel)
    {
        long weight = 0;
        weight = (long)Mathf.Pow(2, equipType - 3001);
        return weight;
    }

    public static long GetEquipmentEnhancePrice(int equipType, int level)
    {
        long price = 0;
        price = (long)(Mathf.Pow(3, equipType - 3001) * 1000);
        return price;
    }
    #endregion
}
