using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public MyPlayer myPlayer;
    
    public Lode lodePrefab;

    List<Lode> lodeList = new List<Lode>();
    Stack<Lode> lodePool = new Stack<Lode>();

    public Lode InstantiateLode()
    {
        if (lodePool.Count > 0)
            return lodePool.Pop();

        var lode = Instantiate(lodePrefab);
        lodeList.Add(lode);
        return lode;
    }

    public void DestroyLode(int id)
    {
        foreach (var lode in lodeList)
        {
            if (lode.id == id)
            {
                lode.gameObject.SetActive(false);
                lodePool.Push(lode);
                break;
            }
        }
    }

    public void DestroyAllLode()
    {
        foreach (var lode in lodeList)
        {
            if (!lodePool.Contains(lode))
            {
                lode.gameObject.SetActive(false);
                lodePool.Push(lode);
            }
        }
    }
}
