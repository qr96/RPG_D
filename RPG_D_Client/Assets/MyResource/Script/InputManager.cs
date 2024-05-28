using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Dictionary<KeyCode, Action> keyDownEventDic = new Dictionary<KeyCode, Action>();

    private void Update()
    {
        foreach (var keyDownEvent in keyDownEventDic.ToArray())
        {
            if (Input.GetKeyDown(keyDownEvent.Key))
            {
                keyDownEventDic[keyDownEvent.Key]?.Invoke();
            }
        }
    }

    public void AddKeyDownEvent(KeyCode key, Action onKeyDown)
    {
        if (keyDownEventDic.ContainsKey(key))
            keyDownEventDic[key] += onKeyDown;
        else
            keyDownEventDic.Add(key, onKeyDown);
    }

    public void RemoveKeyDownEvent(KeyCode key, Action onKeyDown)
    {
        if (keyDownEventDic.ContainsKey(key))
            keyDownEventDic[key] -= onKeyDown;
    }
}
