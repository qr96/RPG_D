using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalServer : MonoBehaviour
{
    public static LocalServer Instance;

    private void Awake()
    {
        Instance = this;
    }

    Dictionary<int, LodeObject> lodeObjectDic = new Dictionary<int, LodeObject>();
    Dictionary<int, Tuple<long>> lodeInfoDic = new Dictionary<int, Tuple<long>>(); // <lodeType, maxHp> : TODO

    // TODO : Make class
    // Now Attacked lode Info
    int lodeId = 0;
    long lodeHp = 0;

    private void Start()
    {
        lodeInfoDic.Add(10001, new Tuple<long>(100));
        lodeObjectDic.Add(0, new LodeObject() { id = 0, lodeType = 10001, position = new Vector3() });
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
        var damage = 10;
        lodeHp -= damage;

        LocalPacketHandler.S_LodeAttack(lodeHp, damage, false);
    }
}
