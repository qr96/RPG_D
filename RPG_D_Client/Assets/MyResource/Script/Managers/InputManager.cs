using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    Stack<Action> inputEvents = new Stack<Action>();
    Stack<bool> playerLock = new Stack<bool>();
    Vector2 playerInput;

    private void Update()
    {
        if (inputEvents.Count > 0)
            inputEvents.Peek().Invoke();

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        if (playerLock.Count > 0 && playerLock.Peek())
            playerInput = Vector2.zero;
    }

    public void PushInputEvent(Action inputEvent, bool playerMoveLock)
    {
        inputEvents.Push(inputEvent);
        playerLock.Push(playerMoveLock);
    }

    public void PopInputEvent()
    {
        inputEvents.Pop();
        playerLock.Pop();
    }

    public Vector2 GetPlayerInputAxis()
    {
        return playerInput;
    }
}
