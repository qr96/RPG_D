using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# region Packet
public class LodeObject
{
    public int id;
    public int lodeType;
    public Vector2 position;
}

public class UserInfo
{
    public int id;
    public string name;
    public float speed;
    public Vector2 position;
}

public class Item
{
    public int itemType;
    public long count;
}
#endregion

# region InServer
public class UserData
{
    public int userId;
    public string nickName;
    public long maxWeight;
    public long maxHp;
    public long attack;
    public float speed;
    public long money;
    public int anvilLevel;

    public Dictionary<int, Item> mineralDic = new Dictionary<int, Item>();
    public long nowWeight;
    public int weaponLevel;
    public int armorLevel;
    public int shoesLevel;

    public int lastMapId;
}

public class UserGameInfo
{
    public int id;
    public long maxWeight;
    public long nowWeight;
    public long maxHp;
    public long nowHp;
    public long attack;
    public float speed;

    public DateTime gameStartTime;
    public List<Item> mineralList = new List<Item>();
}
#endregion
