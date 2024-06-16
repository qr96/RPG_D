using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

#region Packet

public class UserInfo
{
    public int id;
    public string name;
    public float speed;
    public Vector2 position;
}
#endregion

# region Common
public class LodeObject
{
    public int id;
    public int lodeType;
    public Vector2 position;
}

public class Item
{
    public int itemType;
    public long count;
}

public class Equipment
{
    public int type;
    public int level;
}

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
    public Dictionary<int, Equipment> weaponDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> shirtDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> bagDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> shoeDic = new Dictionary<int, Equipment>();

    public long nowWeight;
    public int weaponLevel;
    public int armorLevel;
    public int shoesLevel;

    public int lastMapId;
}
public class Stat
{
    public long attack;
    public long maxHp;
    public long maxWeight;
    public float speed;

    public void AddStat(Stat stat)
    {
        attack += stat.attack;
        maxHp += stat.maxHp;
        maxWeight += stat.maxWeight;
        speed += stat.speed;
    }
}
#endregion

# region InServer
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
