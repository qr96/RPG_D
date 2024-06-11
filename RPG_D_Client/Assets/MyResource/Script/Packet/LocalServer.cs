using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

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

    UserData userData = new UserData(); // user's normal info
    UserGameInfo userGameInfo = new UserGameInfo(); // user's now playing game info
    long hpReducePerSec = 1;

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
        lodeObjectDic.Add(7, new LodeObject() { id = 7, lodeType = 10001, position = new Vector2(20f, -42f) });
        lodeObjectDic.Add(8, new LodeObject() { id = 8, lodeType = 10001, position = new Vector2(12f, -51f) });
        lodeObjectDic.Add(9, new LodeObject() { id = 9, lodeType = 10001, position = new Vector2(5f, -63f) });
        lodeObjectDic.Add(10, new LodeObject() { id = 10, lodeType = 10001, position = new Vector2(10f, -75f) });
        lodeObjectDic.Add(11, new LodeObject() { id = 11, lodeType = 10001, position = new Vector2(20f, -80f) });
        lodeObjectDic.Add(12, new LodeObject() { id = 12, lodeType = 10001, position = new Vector2(32f, -80f) });
        lodeObjectDic.Add(13, new LodeObject() { id = 13, lodeType = 10001, position = new Vector2(46f, -77f) });
        lodeObjectDic.Add(14, new LodeObject() { id = 14, lodeType = 10001, position = new Vector2(50f, -85f) });
        lodeObjectDic.Add(15, new LodeObject() { id = 15, lodeType = 10001, position = new Vector2(44f, -98f) });
        lodeObjectDic.Add(16, new LodeObject() { id = 16, lodeType = 10001, position = new Vector2(37f, -104f) });
        lodeObjectDic.Add(17, new LodeObject() { id = 17, lodeType = 10001, position = new Vector2(31f, -107f) });
        lodeObjectDic.Add(18, new LodeObject() { id = 18, lodeType = 10001, position = new Vector2(20f, -110f) });
        lodeObjectDic.Add(19, new LodeObject() { id = 19, lodeType = 10001, position = new Vector2(12f, -116f) });

        userData.speed = 200f;
        userData.maxHp = 100;
        userData.nickName = "테스트플레이어";
        userData.maxWeight = 100;
        userData.maxHp = 100;
        userData.attack = 10;
        userData.money = 10000000;
    }

    public void C_UserInfo(int uid)
    {
        LocalPacketHandler.S_UserInfo(userData);
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
        userGameInfo.gameStartTime = DateTime.Now;
        LocalPacketHandler.S_GameStart(true, hpReducePerSec);
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

        var damage = 0L;
        if (attackLevel == 0)
            damage = userData.attack * 12 / 10;
        else if (attackLevel == 1)
            damage = userData.attack;
        else if (attackLevel == 2)
            damage = userData.attack * 8 / 10;

            lodeHp -= damage;
        if (lodeHp < 0)
            lodeHp = 0;

        LocalPacketHandler.S_LodeAttack(lodeHp, damage, false);

        if (lodeHp <= 0)
        {
            var nowMinerals = new List<Item>();

            // can get more items
            if (userGameInfo.nowWeight < userGameInfo.maxWeight)
            {
                nowMinerals.Add(new Item() { itemType = 10001, count = 3 });
                nowMinerals.Add(new Item() { itemType = 10002, count = 4 });
                nowMinerals.Add(new Item() { itemType = 10003, count = 5 });
            }

            foreach (var item in nowMinerals)
            {
                if (acquiredItem.ContainsKey(item.itemType))
                    acquiredItem[item.itemType].count += item.count;
                else
                    acquiredItem.Add(item.itemType, item);
            }

            userGameInfo.nowWeight = DataTable.GetMineralWeight(acquiredItem.Values.ToList());

            LocalPacketHandler.S_LodeAttackResult(lodeId, nowMinerals, userData.maxWeight, userGameInfo.nowWeight);
        }
    }

    public void C_MineGameResult()
    {
        bool gameSuccess = false;
        gameSuccess = (DateTime.Now - userGameInfo.gameStartTime).TotalSeconds * hpReducePerSec < userData.maxHp;

        if (gameSuccess)
        {
            foreach (var acquired in acquiredItem)
            {
                // Put items to user's normal info
                if (userData.mineralDic.ContainsKey(acquired.Key))
                    userData.mineralDic[acquired.Key].count += acquired.Value.count;
                else
                    userData.mineralDic[acquired.Key] = acquired.Value;
            }
        }

        LocalPacketHandler.S_MineGameResult(gameSuccess, acquiredItem.Values.ToList());
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
                    sellPrice += Math.Min(count, item.count) * 1000;
                else if (item.itemType == 10002)
                    sellPrice += Math.Min(count, item.count) * 1500;
                else if (item.itemType == 10003)
                    sellPrice += Math.Min(count, item.count) * 2000;
                item.count -= Math.Min(count, item.count);
            }
        }

        userData.money += sellPrice;
        SendInventoryInfo();
    }

    public void C_EnforceEquip(int equipType)
    {
        var nowLevel = 0;
        var result = 0;

        if (equipType == 0)
            nowLevel = userData.weaponLevel;
        else if (equipType == 1)
            nowLevel = userData.armorLevel;
        else if (equipType == 2)
            nowLevel = userData.shoesLevel;

        var enhancePrice = DataTable.GetEquipEnhancePrice(nowLevel);
        var successPer = DataTable.GetEquipEnhanceSuccessPercent(nowLevel);
        var rand = UnityEngine.Random.Range(0f, 100f);

        if (userData.money > enhancePrice)
        {
            userData.money -= enhancePrice;
            if (rand < successPer)
            {
                result = 0;
                if (equipType == 0)
                {
                    userData.weaponLevel++;
                    userData.attack += 5;
                }
                else if (equipType == 1)
                {
                    userData.armorLevel++;
                    userData.maxHp += 10;
                }
                else if (equipType == 2)
                {
                    userData.shoesLevel++;
                    userData.speed += 5;
                }
            }
            else
            {
                result = 1;
            }
        }
        else
        {
            result = 2;
        }

        LocalPacketHandler.S_EnforceEquip(userData, result);
    }

    public void SendInventoryInfo()
    {
        LocalPacketHandler.S_InventoryInfo(userData.mineralDic.Values.ToList(), userData.money, userData.maxWeight, userData.nowWeight);
    }
}
