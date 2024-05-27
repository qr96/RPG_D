using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGuage : MonoBehaviour
{
    public GameObject pointer;
    public float moveTime;
    public float minPos;
    public float maxPos;

    bool movingPointer = true;
    bool moveRight;
    float moveRange;
    float timer;

    private void Start()
    {
        StartMove(1f);
    }

    private void FixedUpdate()
    {
        if (movingPointer)
        {
            if (moveTime == 0) return;

            var pointerPos = pointer.transform.localPosition;
            var speed = moveRange / moveTime;
            timer += Time.fixedDeltaTime;

            if (moveRight)
            {
                if (timer < moveTime)
                    pointer.transform.localPosition = new Vector2(minPos + speed * timer, pointerPos.y);
                else
                {
                    moveRight = false;
                    timer -= moveTime;
                }
            }
            else
            {
                if (timer < moveTime)
                    pointer.transform.localPosition = new Vector2(maxPos - speed * timer, pointerPos.y);
                else
                {
                    moveRight = true;
                    timer -= moveTime;
                }
            }
        }
    }

    public void StartMove(float moveTime)
    {
        this.moveTime = moveTime;
        pointer.transform.localPosition = new Vector2(minPos, pointer.transform.localPosition.y);
        moveRange = maxPos - minPos;
        timer = 0f;
    }
}
