using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalServer : MonoBehaviour
{
    public static LocalServer Instance;

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
    }

    Dictionary<int, LodeObject> lodeObjectDic = new Dictionary<int, LodeObject>();
    Dictionary<int, Tuple<long>> lodeInfoDic = new Dictionary<int, Tuple<long>>(); // <lodeType, maxHp> : TODO

    // TODO : Make class
    // Now Attacked lode Info
    int lodeId = 0;
    long lodeHp = 0;
    Dictionary<int, Item> acquiredItem = new Dictionary<int, Item>();

    public void C_ReqGameInfo(int mapId)
    {
        LocalPacketHandler.S_ReqGameInfo(lodeObjectDic.Values.ToList(), new UserGameInfo() { maxHp = 100 });
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
        LocalPacketHandler.S_MineGameResult(acquiredItem.Values.ToList());
    }
}
