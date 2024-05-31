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
    public List<Item> minerals;
}

public class UserGameInfo
{
    public int id;
    public long maxWeight;
    public long nowWeight;
    public long maxHp;
    public long nowHp;
    public long attack;

    public DateTime gameStartTime;
    public List<int> mineralList = new List<int>();
}
#endregion
