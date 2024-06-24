using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGuage : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform zone;
    public RectTransform orangeZone;
    public RectTransform yellowZone;
    public RectTransform greenZone;

    // Setting Value
    float moveTime;
    float targetTime;
    float orangePer;
    float yellowPer;
    float greenPer;
    float minPos;
    float maxPos;
    float moveRange;

    // Caching Value
    bool movingPointer = false;
    bool moveRight;
    float timer;

    private void Update()
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

    public void SetGuage(float moveTime, float targetTime, float orangePer, float yellowPer, float greenPer)
    {
        this.moveTime = moveTime;
        this.orangePer = orangePer;
        this.yellowPer = yellowPer;
        this.greenPer = greenPer;

        moveRange = zone.rect.width;
        maxPos = moveRange / 2f;
        minPos = -moveRange / 2f;
        timer = 0f;

        orangeZone.sizeDelta = new Vector2(moveRange * orangePer, orangeZone.sizeDelta.y);
        yellowZone.sizeDelta = new Vector2(moveRange * yellowPer, yellowZone.sizeDelta.y);
        greenZone.sizeDelta = new Vector2(moveRange * greenPer, greenZone.sizeDelta.y);

        SetTargetZone(targetTime);
    }

    public void StartMove()
    {
        pointer.anchoredPosition = new Vector2(minPos, pointer.transform.localPosition.y);
        movingPointer = true;
    }

    public void ChanageTargetZone(float targetTime)
    {
        SetTargetZone(targetTime);
    }

    public int GetPointResult()
    {
        var greenBoundary = moveTime * greenPer / 2f;
        var yellowBoundary = moveTime * yellowPer / 2f;
        var orangeBoundary = moveTime * orangePer / 2f;

        if (moveRight)
        {
            if (timer < targetTime + greenBoundary && timer > targetTime - greenBoundary)
                return 0;
            else if (timer < targetTime + yellowBoundary && timer > targetTime - yellowBoundary)
                return 1;
            else if (timer < targetTime + orangeBoundary && timer > targetTime - orangeBoundary)
                return 2;
        }
        else
        {
            if (moveTime - timer < targetTime + greenBoundary && timer > targetTime - greenBoundary)
                return 0;
            else if (moveTime - timer < targetTime + yellowBoundary && timer > targetTime - yellowBoundary)
                return 1;
            else if (moveTime - timer < targetTime + orangeBoundary && timer > targetTime - orangeBoundary)
                return 2;
        }

        return -1;
    }

    public void StopMove()
    {
        movingPointer = false;
    }

    void SetTargetZone(float targetTime)
    {
        this.targetTime = targetTime;

        orangeZone.anchoredPosition = new Vector2(minPos + moveRange * targetTime / moveTime, orangeZone.anchoredPosition.y);
        yellowZone.anchoredPosition = new Vector2(minPos + moveRange * targetTime / moveTime, yellowZone.anchoredPosition.y);
        greenZone.anchoredPosition = new Vector2(minPos + moveRange * targetTime / moveTime, greenZone.anchoredPosition.y);
    }
}
