using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public MyPlayer myPlayer;
    
    public Lode lodePrefab;

    List<Lode> lodePool = new List<Lode>();

    public Lode InstantiateLode()
    {
        var lode = Instantiate(lodePrefab);
        lodePool.Add(lode);
        return lode;
    }

    public void DestroyLode(int id)
    {
        foreach (var lode in lodePool)
        {
            if (lode.id == id)
            {
                lode.gameObject.SetActive(false);
                break;
            }
        }
    }
}
