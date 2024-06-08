using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    Dictionary<KeyCode, Action> keyDownEventDic = new Dictionary<KeyCode, Action>();
    Stack<Action> inputEvents = new Stack<Action>();

    private void Update()
    {
        foreach (var keyDownEvent in keyDownEventDic.ToArray())
        {
            if (Input.GetKeyDown(keyDownEvent.Key))
            {
                keyDownEventDic[keyDownEvent.Key]?.Invoke();
            }
        }

        if (inputEvents.Count > 0)
            inputEvents.Peek().Invoke();
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

    public void PushInputEvent(Action inputEvent)
    {
        inputEvents.Push(inputEvent);
    }

    public void PopInputEvent()
    {
        inputEvents.Pop();
    }
}
