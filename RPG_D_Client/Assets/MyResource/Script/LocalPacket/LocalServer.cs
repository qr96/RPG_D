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

    // TODO : Make class
    // Now Attacked lode Info
    int lodeId = 0;
    long lodeHp = 0;
    long userNowMp = 0;

    UserData userData = new UserData(); // user's normal info
    UserGameInfo userGameInfo = new UserGameInfo(); // user's now playing game info
    long hpReducePerSec = 1;

    System.Random rand = new System.Random();

    private void Awake()
    {
        Instance = this;

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
        lodeObjectDic.Add(20, new LodeObject() { id = 20, lodeType = 10003, position = new Vector2(5f, -120f) });
        lodeObjectDic.Add(21, new LodeObject() { id = 21, lodeType = 10003, position = new Vector2(-9f, -127f) });
        lodeObjectDic.Add(22, new LodeObject() { id = 22, lodeType = 10003, position = new Vector2(1.4f, -134f) });
        lodeObjectDic.Add(23, new LodeObject() { id = 23, lodeType = 10003, position = new Vector2(9.3f, -137f) });
        lodeObjectDic.Add(24, new LodeObject() { id = 24, lodeType = 10003, position = new Vector2(19f, -140f) });
        lodeObjectDic.Add(25, new LodeObject() { id = 25, lodeType = 10003, position = new Vector2(33.5f, -146f) });
        lodeObjectDic.Add(26, new LodeObject() { id = 26, lodeType = 10003, position = new Vector2(42f, -152f) });
        lodeObjectDic.Add(27, new LodeObject() { id = 27, lodeType = 10003, position = new Vector2(31.4f, -159f) });
        lodeObjectDic.Add(28, new LodeObject() { id = 28, lodeType = 10004, position = new Vector2(34f, -226f) });
    }

    void MakeUserData(string nickname)
    {
        userData.nickName = nickname;
        userData.money = 10000;
        userData.lastMapId = 1001;
        userData.normalStat = new Stat() { attack = 10, maxHp = 30, maxMp = 5, maxWeight = 100, speed = 200f };
        userData.equipStat = new Stat();

        userData.mineTicketDic.Add(1001, new Item() { itemType = 1001, count = 999 });
        userData.consumableDic.Add(1001, new Item() { itemType = 1001, count = 10 });

        for (int i = 8001; i < 8009; i++)
            userData.skillDic.Add(i, new Skill() { type = i });

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
        {
            if (userData.mineTicketDic.ContainsKey(1001) && userData.mineTicketDic[1001].count > 0)
            {
                userData.mineTicketDic[1001].count--;
                LocalPacketHandler.S_MoveMapMineGame(mapId, lodeObjectDic.Values.ToList());
            }
        }
    }

    public void C_GameStart()
    {
        userGameInfo.gameStat = new Stat();
        userGameInfo.gameStat.AddStat(userData.normalStat);
        userGameInfo.gameStat.AddStat(userData.equipStat);

        userGameInfo.gameStartTime = DateTime.Now;
        userGameInfo.nowWeight = userData.nowWeight;
        userGameInfo.nowHp = userGameInfo.gameStat.maxHp;
        userGameInfo.acquired.Clear();

        LocalPacketHandler.S_GameStart(hpReducePerSec);
    }

    public void C_LodeAttackStart(int lodeId)
    {
        if (!lodeObjectDic.ContainsKey(lodeId))
            return;

        var getLodeHp = DataTable.GetLodeHp(lodeObjectDic[lodeId].lodeType);
        if (getLodeHp <= 0)
            return;

        this.lodeId = lodeId;
        lodeHp = getLodeHp;
        userNowMp = userGameInfo.gameStat.maxMp;

        LocalPacketHandler.S_LodeAttackStart(0, lodeHp, userNowMp);
    }

    public void C_LodeAttack(int attackLevel)
    {
        if (lodeHp <= 0)
            return;

        if (userNowMp <= 0)
            return;

        var damage = 0L;
        if (attackLevel == 0)
            damage = userGameInfo.gameStat.attack * 12 / 10;
        else if (attackLevel == 1)
            damage = userGameInfo.gameStat.attack;
        else if (attackLevel == 2)
            damage = userGameInfo.gameStat.attack * 8 / 10;

        lodeHp -= damage;
        lodeHp = Math.Max(lodeHp, 0);
        userNowMp -= 1;
        userNowMp = Math.Max(userNowMp, 0);

        LocalPacketHandler.S_LodeAttack(lodeHp, damage, userGameInfo.gameStat.maxMp, userNowMp);

        if (userNowMp <= 0 && lodeHp > 0)
        {
            lodeHp = 0;
        }
        else if (lodeHp <= 0)
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
                else if (lodeObjectDic[lodeId].lodeType == 10004)
                {
                    var ticketType = DataTable.GetRewardMineTicket(lodeObjectDic[lodeId].lodeType);
                    if (userData.mineTicketDic.ContainsKey(ticketType))
                        userData.mineTicketDic[ticketType].count++;
                    else
                        userData.mineTicketDic.Add(ticketType, new Item() { itemType = ticketType, count = 1 });

                    if (userGameInfo.consumableDic.ContainsKey(ticketType))
                        userGameInfo.consumableDic[1001].count++;
                    else
                        userGameInfo.consumableDic.Add(1001, new Item() { itemType = 1001, count = 1 });

                    nowMinerals.Add(new Item() { itemType = 10003, count = 50 });
                }
            }

            foreach (var item in nowMinerals)
            {
                if (userGameInfo.acquired.ContainsKey(item.itemType))
                    userGameInfo.acquired[item.itemType].count += item.count;
                else
                    userGameInfo.acquired.Add(item.itemType, item);
            }

            userGameInfo.nowWeight = DataTable.GetMineralWeight(userGameInfo.acquired.Values.ToList());

            LocalPacketHandler.S_LodeAttackResult(lodeId, nowMinerals, userGameInfo.gameStat.maxWeight, userGameInfo.nowWeight);
        }
    }

    public void C_MineGameResult()
    {
        bool gameSuccess = false;
        gameSuccess = (DateTime.Now - userGameInfo.gameStartTime).TotalSeconds * hpReducePerSec <= userGameInfo.gameStat.maxHp;

        if (gameSuccess)
        {
            foreach (var acquired in userGameInfo.acquired)
            {
                // Put items to user's normal info
                if (userData.mineralDic.ContainsKey(acquired.Key))
                    userData.mineralDic[acquired.Key].count += acquired.Value.count;
                else
                    userData.mineralDic[acquired.Key] = acquired.Value;
            }

            foreach (var item in userGameInfo.consumableDic)
            {
                if (userData.consumableDic.ContainsKey(item.Key))
                    userData.consumableDic[item.Key].count += item.Value.count;
                else
                    userData.consumableDic[item.Key] = item.Value;
            }

            userData.nowWeight = userGameInfo.nowWeight;
        }

        LocalPacketHandler.S_MineGameResult(gameSuccess, userGameInfo.acquired.Values.ToList());
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

            if (userData.money >= price && userData.weaponDic[equipType].level < 20)
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

            if (userData.money >= price && userData.shirtDic[equipType].level < 20)
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

            if (userData.money >= price && userData.bagDic[equipType].level < 20)
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

            if (userData.money >= price && userData.shoeDic[equipType].level < 20)
            {
                userData.money -= price;
                userData.equipStat.AddStat(DataTable.GetEquipmentIncreaseStat(equipType));
                userData.shoeDic[equipType].level++;

                LocalPacketHandler.S_UserInfo(userData);
            }
        }

        SaveData();
    }

    public void C_UseItem(int itemType)
    {
        if (userData.consumableDic.ContainsKey(itemType))
        {
            if (userData.consumableDic[itemType] != null && userData.consumableDic[(itemType)].count > 0)
            {
                userData.consumableDic[itemType].count--;
                if (itemType == 1001)
                {
                    var skillType = rand.Next(8001, 8008);
                    if (userData.skillDic[skillType].level == 0)
                        userData.skillDic[skillType].level = 1;
                    else
                        userData.skillDic[skillType].exp++;

                    LocalPacketHandler.S_LearnNewSkill(userData.skillDic[skillType]);
                }
                LocalPacketHandler.S_UserInfo(userData);
                SaveData();
            }
        }
    }

    public void SendInventoryInfo()
    {
        LocalPacketHandler.S_InventoryInfo(userData.mineralDic.Values.ToList(), userData.money, userData.normalStat.maxWeight + userData.equipStat.maxWeight, userData.nowWeight);
    }

    public void C_StartQuest(int questId)
    {
        if (userData.questDic.ContainsKey(questId))
        {
            if (userData.questDic[questId] == QuestState.NotStarted)
            {
                userData.questDic[questId] = QuestState.Progress;

            }   
        }
    }

    public void C_CompleteQuest(int questId)
    {
        if (userData.questDic.ContainsKey(questId))
        {
            if (userData.questDic[questId] == QuestState.Progress)
            {
                if (DataTable.IsQuestComplete(questId, userData))
                {

                }
            }
        }
    }
}