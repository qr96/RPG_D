using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPool
{
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
            price += GetItemPrice(item);

        return price;
    }

    public static long GetItemPrice(Item item)
    {
        var price = 0L;

        if (item.itemType == 10001)
            price = 1000;
        else if (item.itemType == 10002)
            price = 1500;
        else if (item.itemType == 10003)
            price = 2000;

        return price;
    }
}