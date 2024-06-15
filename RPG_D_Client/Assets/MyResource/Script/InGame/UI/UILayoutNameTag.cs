using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UILayoutNameTag : UILayout
{
    GameObject nameTagPrefab;
    Stack<GameObject> nameTagPool = new Stack<GameObject>();
    Dictionary<GameObject, Transform> nameTagAttachedDic = new Dictionary<GameObject, Transform>();

    private void Awake()
    {
        nameTagPrefab = gameObject.Find("NameTag");
    }

    public Transform AcquireNameTag(GameObject target, string name)
    {
        GameObject nameTag;

        if (nameTagPool.Count > 0)
            nameTag = nameTagPool.Pop();
        else
            nameTag = Instantiate(nameTagPrefab, nameTagPrefab.transform.parent);

        if (nameTagAttachedDic.ContainsKey(target))
            nameTagAttachedDic.Remove(target);

        nameTagAttachedDic.Add(target, nameTag.transform);
        nameTag.Find<TMP_Text>("NameText").text = name;
        nameTag.SetActive(true);

        return nameTag.transform;
    }

    public void RemoveNameTag(GameObject target)
    {
        if (nameTagAttachedDic.ContainsKey(target))
        {
            var nameTag = nameTagAttachedDic[target];
            nameTag.gameObject.SetActive(false);
            nameTagPool.Push(nameTag.gameObject);
            nameTagAttachedDic.Remove(target);
        }
    }
}
