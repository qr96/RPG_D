using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public long money;
    public long nowWeight;
    public int lastMapId;

    public Stat normalStat;
    public Stat equipStat;

    public Dictionary<int, Item> mineralDic = new Dictionary<int, Item>();
    public Dictionary<int, Item> mineTicketDic = new Dictionary<int, Item>();

    public Dictionary<int, Equipment> weaponDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> shirtDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> bagDic = new Dictionary<int, Equipment>();
    public Dictionary<int, Equipment> shoeDic = new Dictionary<int, Equipment>();

    public Dictionary<int, QuestState> questDic = new Dictionary<int, QuestState>();
}

[Serializable]
public class Stat
{
    public long attack;
    public long maxHp;
    public long maxMp;
    public long maxWeight;
    public float speed;

    public void AddStat(Stat stat)
    {
        attack += stat.attack;
        maxHp += stat.maxHp;
        maxMp += stat.maxMp;
        maxWeight += stat.maxWeight;
        speed += stat.speed;
    }

    public string ToStringInfo()
    {
        var info = "";
        var newLine = "\n";
        var nothing = "";

        if (attack > 0)
            info += $"ATK +{attack}";
        if (maxHp > 0)
            info += $"{(info.Length > 0 ? newLine : nothing)}HP +{maxHp}";
        if (maxWeight > 0)
            info += $"{(info.Length > 0 ? newLine : nothing)}WGHT +{maxWeight}";
        if (speed > 0)
            info += $"{(info.Length > 0 ? newLine : nothing)}SPEED +{speed}";

        return info;
    }
}

public enum QuestState
{
    NotStarted = 0,
    Progress = 1,
    Done = 2
}

#endregion

# region InServer
public class UserGameInfo
{
    public int id;
    public long nowWeight;
    public long nowHp;
    public float speed;
    public Stat gameStat;

    public DateTime gameStartTime;
    public Dictionary<int, Item> acquired = new Dictionary<int, Item>();
}
#endregion
