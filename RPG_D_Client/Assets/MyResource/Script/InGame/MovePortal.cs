using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePortal : MonoBehaviour
{
    public int id;

    Action onTiggerEnter;
    Action onTiggerExit;

    Transform nameTag;

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

    private void OnDisable()
    {
        if (nameTag != null)
        {
            Managers.ui.GetLayout<UILayoutNameTag>().RemoveNameTag(transform);
            nameTag = null;
        }
    }

    private void LateUpdate()
    {
        if (nameTag != null)
            nameTag.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 1f, 0f));
    }

    public void SetTriggerEnterEvent(Action onTriggerEnterEvent)
    {
        onTiggerEnter = onTriggerEnterEvent;
    }

    public void SetTriggerExitEvent(Action onTriggerExitEvnet)
    {
        onTiggerExit = onTriggerExitEvnet;
    }

    public void SetNameTag(Transform nameTag)
    {
        this.nameTag = nameTag;
    }
}
