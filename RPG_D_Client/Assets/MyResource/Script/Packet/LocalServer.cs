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

    List<LodeInfo> lodeInfoList = new List<LodeInfo>();

    private void Start()
    {
        lodeInfoList.Add(new LodeInfo() { id = 0, lodeType = 10001, position = new Vector3() });
    }

    public void Recv()
    {

    }

    public void C_LodeAttackStart(int lodeId)
    {
        LocalPacketHandler.S_LodeAttackStart(0, 1000);
    }
}
