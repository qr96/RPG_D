using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UILayoutNameTag : UILayout
{
    GameObject nameTagPrefab;
    Stack<GameObject> nameTagPool = new Stack<GameObject>();
    Dictionary<GameObject, GameObject> nameTagAttachedDic = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        nameTagPrefab = gameObject.Find("NameTag");
    }

    private void LateUpdate()
    {
        foreach (var iter in nameTagAttachedDic)
        {
            //iter.Value.transform.position = Camera.main.WorldToScreenPoint(iter.Key.transform.position + new Vector3(0f, 1f, 0f));
        }
    }

    public GameObject AcquireNameTag(string name)
    {
        GameObject nameTag;

        if (nameTagPool.Count > 0)
            nameTag = nameTagPool.Pop();
        else
            nameTag = Instantiate(nameTagPrefab, nameTagPrefab.transform.parent);

        nameTag.Find<TMP_Text>("NameText").text = name;
        nameTag.SetActive(true);
        return nameTag;
    }

    public void RemoveNameTag(GameObject target)
    {
        if (nameTagAttachedDic.ContainsKey(target))
        {
            var nameTag = nameTagAttachedDic[target];
            nameTag.SetActive(false);
            nameTagPool.Push(nameTag);
            nameTagAttachedDic.Remove(target);
        }
    }
}
