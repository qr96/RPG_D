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

    private void Update()
    {
        if (movingPointer)
        {
            if (moveTime == 0) return;

            var pointerPos = pointer.transform.localPosition;
            var moveRange = maxPos - minPos;
            var speed = moveRange / moveTime;

            if (moveRight)
            {
                if (pointerPos.x < maxPos)
                    pointer.transform.localPosition = new Vector2(pointerPos.x + speed * Time.deltaTime, pointerPos.y);
                else
                    moveRight = false;
            }
            else
            {
                if (pointerPos.x > minPos)
                    pointer.transform.localPosition = new Vector2(pointerPos.x - speed * Time.deltaTime, pointerPos.y);
                else
                    moveRight = true;
            }
        }
    }


}
