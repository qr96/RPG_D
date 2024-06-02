using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class LocalServer : MonoBehaviour
{
    public static LocalServer Instance;

    Dictionary<int, LodeObject> lodeObjectDic = new Dictionary<int, LodeObject>();
    Dictionary<int, Tuple<long>> lodeInfoDic = new Dictionary<int, Tuple<long>>(); // <lodeType, maxHp> : TODO

    // TODO : Make class
    // Now Attacked lode Info
    int lodeId = 0;
    long lodeHp = 0;
    Dictionary<int, Item> acquiredItem = new Dictionary<int, Item>();

    UserData userData = new UserData();
    UserGameInfo userGameInfo = new UserGameInfo();

    private void Awake()
    {
        Instance = this;

        lodeInfoDic.Add(10001, new Tuple<long>(100));
        lodeObjectDic.Add(1, new LodeObject() { id = 1, lodeType = 10001, position = new Vector2(0.3f, -3.5f) });
        lodeObjectDic.Add(2, new LodeObject() { id = 2, lodeType = 10001, position = new Vector2(4.2f, -8.7f) });
        lodeObjectDic.Add(3, new LodeObject() { id = 3, lodeType = 10001, position = new Vector2(0.6f, -16.5f) });
        lodeObjectDic.Add(4, new LodeObject() { id = 4, lodeType = 10001, position = new Vector2(8f, -18f) });
        lodeObjectDic.Add(5, new LodeObject() { id = 5, lodeType = 10001, position = new Vector2(18f, -22.5f) });
        lodeObjectDic.Add(6, new LodeObject() { id = 6, lodeType = 10001, position = new Vector2(24f, -31.5f) });

        userData.speed = 200f;
        userData.maxHp = 100;
        userData.nickName = "테스트플레이어";
        userData.maxWeight = 100;
        userData.maxHp = 100;
        userData.attack = 10;
    }

    public void C_MoveMap(int mapId)
    {
        userGameInfo.speed = userData.speed;
        userGameInfo.maxHp = userData.maxHp;
        userGameInfo.nowHp = userData.maxHp;
        userGameInfo.maxWeight = userData.maxWeight;
        userGameInfo.attack = userData.attack;

        if (mapId == 1001)
            LocalPacketHandler.S_MoveMap(true, mapId, new List<LodeObject>(), userGameInfo, false);
        else if (mapId == 1002)
            LocalPacketHandler.S_MoveMap(true, mapId, lodeObjectDic.Values.ToList(), userGameInfo, true);
    }

    public void C_GameStart()
    {
        LocalPacketHandler.S_GameStart(true, 1);
    }

    public void C_LodeAttackStart(int lodeId)
    {
        if (!lodeObjectDic.ContainsKey(lodeId))
            return;

        if (!lodeInfoDic.ContainsKey(lodeObjectDic[lodeId].lodeType))
            return;

        this.lodeId = lodeId;
        lodeHp = lodeInfoDic[lodeObjectDic[lodeId].lodeType].Item1;

        LocalPacketHandler.S_LodeAttackStart(0, lodeHp);
    }

    public void C_LodeAttack(int attackLevel)
    {
        if (lodeHp <= 0)
            return;

        var damage = 10;
        lodeHp -= damage;

        LocalPacketHandler.S_LodeAttack(lodeHp, damage, false);

        if (lodeHp <= 0)
        {
            var minerals = new List<Item>() {
                new Item() { itemType = 10001, count = 5 },
                new Item() { itemType = 10002, count = 3 },
                new Item() { itemType = 10003, count = 6 }
            };
            foreach (var item in minerals) {
                if (acquiredItem.ContainsKey(item.itemType))
                    acquiredItem[item.itemType].count += item.count;
                else
                    acquiredItem.Add(item.itemType, item);
            }

            LocalPacketHandler.S_LodeAttackResult(lodeId, minerals, 10);
        }
    }

    public void C_MineGameResult()
    {
        foreach (var acquired in acquiredItem)
        {
            if (userData.mineralDic.ContainsKey(acquired.Key))
                userData.mineralDic[acquired.Key].count += acquired.Value.count;
            else
                userData.mineralDic[acquired.Key] = acquired.Value;

        }
        LocalPacketHandler.S_MineGameResult(acquiredItem.Values.ToList());
        SendInventoryInfo();
    }

    public void C_SellItem(bool sellAll, int itemType, long count)
    {
        long sellPrice = 0;

        if (sellAll)
        {
            foreach (var item in userData.mineralDic)
            {
                if (item.Value.itemType == 10001)
                    sellPrice += item.Value.count * 1000;
                else if (item.Value.itemType == 10002)
                    sellPrice += item.Value.count * 1500;
                else if (item.Value.itemType == 10003)
                    sellPrice += item.Value.count * 2000;
                item.Value.count = 0;
            }
        }
        else
        {
            if (userData.mineralDic.ContainsKey(itemType))
            {
                var item = userData.mineralDic[itemType];
                if (item.itemType == 10001)
                    sellPrice += item.count * 1000;
                else if (item.itemType == 10002)
                    sellPrice += item.count * 1500;
                else if (item.itemType == 10003)
                    sellPrice += item.count * 2000;
                item.count = 0;
            }
        }

        userData.money += sellPrice;
        SendInventoryInfo();
    }

    public void SendInventoryInfo()
    {
        LocalPacketHandler.S_InventoryInfo(userData.mineralDic.Values.ToList(), userData.money);
    }
}
