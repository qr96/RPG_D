using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePortal : MonoBehaviour
{
    public int id;

    Action onTiggerEnter;
    Action onTiggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (onTiggerEnter != null)
                onTiggerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (onTiggerExit != null)
                onTiggerExit();
        }
    }

    public void SetTriggerEnterEvent(Action onTriggerEnterEvent)
    {
        onTiggerEnter = onTriggerEnterEvent;
    }

    public void SetTriggerExitEvent(Action onTriggerExitEvnet)
    {
        onTiggerExit = onTriggerExitEvnet;
    }
}
