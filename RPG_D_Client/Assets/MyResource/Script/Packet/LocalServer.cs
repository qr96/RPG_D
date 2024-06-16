using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        lodeInfoDic.Add(10002, new Tuple<long>(200));
        lodeInfoDic.Add(10003, new Tuple<long>(500));

        lodeObjectDic.Add(1, new LodeObject() { id = 1, lodeType = 10001, position = new Vector2(0.3f, -3.5f) });
        lodeObjectDic.Add(2, new LodeObject() { id = 2, lodeType = 10001, position = new Vector2(4.2f, -8.7f) });
        lodeObjectDic.Add(3, new LodeObject() { id = 3, lodeType = 10001, position = new Vector2(0.6f, -16.5f) });
        lodeObjectDic.Add(4, new LodeObject() { id = 4, lodeType = 10001, position = new Vector2(8f, -18f) });
        lodeObjectDic.Add(5, new LodeObject() { id = 5, lodeType = 10001, position = new Vector2(18f, -22.5f) });
        lodeObjectDic.Add(6, new LodeObject() { id = 6, lodeType = 10001, position = new Vector2(24f, -31.5f) });
        lodeObjectDic.Add(7, new LodeObject() { id = 7, lodeType = 10001, position = new Vector2(20f, -42f) });
        lodeObjectDic.Add(8, new LodeObject() { id = 8, lodeType = 10001, position = new Vector2(12f, -51f) });
        lodeObjectDic.Add(9, new LodeObject() { id = 9, lodeType = 10001, position = new Vector2(5f, -63f) });
        lodeObjectDic.Add(10, new LodeObject() { id = 10, lodeType = 10002, position = new Vector2(10f, -75f) });
        lodeObjectDic.Add(11, new LodeObject() { id = 11, lodeType = 10002, position = new Vector2(20f, -80f) });
        lodeObjectDic.Add(12, new LodeObject() { id = 12, lodeType = 10002, position = new Vector2(32f, -80f) });
        lodeObjectDic.Add(13, new LodeObject() { id = 13, lodeType = 10002, position = new Vector2(46f, -77f) });
        lodeObjectDic.Add(14, new LodeObject() { id = 14, lodeType = 10002, position = new Vector2(50f, -85f) });
        lodeObjectDic.Add(15, new LodeObject() { id = 15, lodeType = 10002, position = new Vector2(44f, -98f) });
        lodeObjectDic.Add(16, new LodeObject() { id = 16, lodeType = 10002, position = new Vector2(37f, -104f) });
        lodeObjectDic.Add(17, new LodeObject() { id = 17, lodeType = 10002, position = new Vector2(31f, -107f) });
        lodeObjectDic.Add(18, new LodeObject() { id = 18, lodeType = 10002, position = new Vector2(20f, -110f) });
        lodeObjectDic.Add(19, new LodeObject() { id = 19, lodeType = 10003, position = new Vector2(12f, -116f) });   
    }

    void MakeUserData(string nickname)
    {
        userData.nickName = nickname;
        userData.money = 10000;
        userData.lastMapId = 1001;
        userData.normalStat = new Stat() { attack = 10, maxHp = 100, maxWeight = 100, speed = 200f };
        userData.equipStat = new Stat();

        for (int i = 3001; i < 3009; i++)
            userData.weaponDic.Add(i, new Equipment() { type = i, level = i == 3001 ? 1 : 0 });

        for (int i = 4001; i < 4005; i++)
            userData.shirtDic.Add(i, new Equipment() { type = i, level = i == 4001 ? 1 : 0 });

        for (int i = 5001; i < 5007; i++)
            userData.bagDic.Add(i, new Equipment() { type = i, level = i == 5001 ? 1 : 0 });

        for (int i = 6001; i < 6006; i++)
            userData.shoeDic.Add(i, new Equipment() { type = i, level = i == 6001 ? 1 : 0 });
    }

    void SaveData()
    {
        var directoryPath = $"{Application.persistentDataPath}/{userData.nickName}/";
        Directory.CreateDirectory(directoryPath);

        var filePath = $"{directoryPath}/userData";
        var userDataJson = JsonConvert.SerializeObject(userData);
        File.WriteAllText(filePath, userDataJson);
    }

    void LoadData(string nickname)
    {
        var filePath = $"{Application.persistentDataPath}/{nickname}/userData";
        if (File.Exists(filePath))
        {
            var fileData = File.ReadAllText(filePath);
            userData = JsonConvert.DeserializeObject<UserData>(fileData);
        }
        else
        {
            MakeUserData(nickname);
            SaveData();
        }
    }

    public void C_Login(string nickname)
    {
        LoadData(nickname);
        LocalPacketHandler.S_Login(true);
    }

    public void C_UserInfo(int uid)
    {
        LocalPacketHandler.S_UserInfo(userData);
        C_MoveMap(userData.lastMapId);
    }

    public void C_MoveMap(int mapId)
    {
        if (mapId == 1001)
        {
            LocalPacketHandler.S_MoveMapTown(mapId);
            LocalPacketHandler.S_UserInfo(userData);
        }
        else if (mapId == 1002)
            LocalPacketHandler.S_MoveMapMineGame(mapId, lodeObjectDic.Values.ToList());
    }

    public void C_GameStart()
    {
        userGameInfo.gameStat = new Stat();
        userGameInfo.gameStat.AddStat(userData.normalStat);
        userGameInfo.gameStat.AddStat(userData.equipStat);

        userGameInfo.gameStartTime = DateTime.Now;
        userGameInfo.nowWeight = userData.nowWeight;
        userGameInfo.nowHp = userGameInfo.gameStat.maxHp;

        LocalPacketHandler.S_GameStart(hpReducePerSec);
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
            damage = userGameInfo.gameStat.attack * 12 / 10;
        else if (attackLevel == 1)
            damage = userGameInfo.gameStat.attack;
        else if (attackLevel == 2)
            damage = userGameInfo.gameStat.attack * 8 / 10;

            lodeHp -= damage;
        if (lodeHp < 0)
            lodeHp = 0;

        LocalPacketHandler.S_LodeAttack(lodeHp, damage, false);

        if (lodeHp <= 0)
        {
            var nowMinerals = new List<Item>();

            // can get more items
            if (userGameInfo.nowWeight < userGameInfo.gameStat.maxWeight)
            {
                if (lodeObjectDic[lodeId].lodeType == 10001)
                {
                    nowMinerals.Add(new Item() { itemType = 10001, count = 3 });
                    nowMinerals.Add(new Item() { itemType = 10002, count = 4 });
                    nowMinerals.Add(new Item() { itemType = 10003, count = 5 });
                }
                else if (lodeObjectDic[lodeId].lodeType == 10002)
                {
                    nowMinerals.Add(new Item() { itemType = 10001, count = 7 });
                    nowMinerals.Add(new Item() { itemType = 10002, count = 9 });
                    nowMinerals.Add(new Item() { itemType = 10003, count = 11 });
                }
                else if (lodeObjectDic[lodeId].lodeType == 10003)
                {
                    nowMinerals.Add(new Item() { itemType = 10001, count = 16 });
                    nowMinerals.Add(new Item() { itemType = 10002, count = 21 });
                    nowMinerals.Add(new Item() { itemType = 10003, count = 26 });
                }
            }

            foreach (var item in nowMinerals)
            {
                if (acquiredItem.ContainsKey(item.itemType))
                    acquiredItem[item.itemType].count += item.count;
                else
                    acquiredItem.Add(item.itemType, item);
            }

            userGameInfo.nowWeight = DataTable.GetMineralWeight(acquiredItem.Values.ToList());

            LocalPacketHandler.S_LodeAttackResult(lodeId, nowMinerals, userGameInfo.gameStat.maxWeight, userGameInfo.nowWeight);
        }
    }

    public void C_MineGameResult()
    {
        bool gameSuccess = false;
        gameSuccess = (DateTime.Now - userGameInfo.gameStartTime).TotalSeconds * hpReducePerSec < userGameInfo.gameStat.maxWeight;

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

            userData.nowWeight = userGameInfo.nowWeight;
        }

        LocalPacketHandler.S_MineGameResult(gameSuccess, acquiredItem.Values.ToList());
        SendInventoryInfo();
        SaveData();
    }

    public void C_SellItem(bool sellAll, int itemType, long count)
    {
        long sellPrice = 0;

        if (sellAll)
        {
            foreach (var item in userData.mineralDic)
            {
                sellPrice += item.Value.count * DataTable.GetItemPrice(item.Value.itemType);
                item.Value.count = 0;
            }

            userData.nowWeight = 0;
        }
        else
        {
            if (userData.mineralDic.ContainsKey(itemType))
            {
                var item = userData.mineralDic[itemType];
                sellPrice += Math.Min(count, item.count) * DataTable.GetItemPrice(item.itemType);
                item.count -= Math.Min(count, item.count);
            }
        }

        userData.money += sellPrice;
        SendInventoryInfo();
        SaveData();
    }

    public void C_BuyEquip(int equipType)
    {
        if (userData.weaponDic.ContainsKey(equipType))
        {
            var nowLevel = userData.weaponDic[equipType].level;
            var price = DataTable.GetEquipmentEnhancePrice(equipType, nowLevel);

            if (userData.money >= price)
            {
                userData.money -= price;
                userData.equipStat.AddStat(DataTable.GetEquipmentIncreaseStat(equipType));
                userData.weaponDic[equipType].level++;

                LocalPacketHandler.S_UserInfo(userData);
            }
        }
        else if (userData.shirtDic.ContainsKey(equipType))
        {
            var nowLevel = userData.shirtDic[equipType].level;
            var price = DataTable.GetEquipmentEnhancePrice(equipType, nowLevel);

            if (userData.money >= price)
            {
                userData.money -= price;
                userData.equipStat.AddStat(DataTable.GetEquipmentIncreaseStat(equipType));
                userData.shirtDic[equipType].level++;

                LocalPacketHandler.S_UserInfo(userData);
            }
        }
        else if (userData.bagDic.ContainsKey(equipType))
        {
            var nowLevel = userData.bagDic[equipType].level;
            var price = DataTable.GetEquipmentEnhancePrice(equipType, nowLevel);

            if (userData.money >= price)
            {
                userData.money -= price;
                userData.equipStat.AddStat(DataTable.GetEquipmentIncreaseStat(equipType));
                userData.bagDic[equipType].level++;

                LocalPacketHandler.S_UserInfo(userData);
            }
        }
        else if (userData.shoeDic.ContainsKey(equipType))
        {
            var nowLevel = userData.shoeDic[equipType].level;
            var price = DataTable.GetEquipmentEnhancePrice(equipType, nowLevel);

            if (userData.money >= price)
            {
                userData.money -= price;
                userData.equipStat.AddStat(DataTable.GetEquipmentIncreaseStat(equipType));
                userData.shoeDic[equipType].level++;

                LocalPacketHandler.S_UserInfo(userData);
            }
        }

        SaveData();
    }

    public void SendInventoryInfo()
    {
        LocalPacketHandler.S_InventoryInfo(userData.mineralDic.Values.ToList(), userData.money, userData.normalStat.maxWeight + userData.equipStat.maxWeight, userData.nowWeight);
    }
}
