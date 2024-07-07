using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RDUtil
{
    const float COS45 = 0.70710678f;

    public static string MoneyComma(long money)
    {
        if (money == 0)
            return "0";
        return money.ToString("#,##0");
    }

    public static Vector2 InputToOctaVector(Vector2 vec)
    {
        // (0,1)이 0도에서 시계방향으로 증가, [-180~180]
        var angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

        if (angle >= -202.5f && angle <= -157.5f)
            return new Vector2(0f, -1f);
        if (angle < -112.5f)
            return new Vector2(-COS45, -COS45);
        else if (angle <= -67.5f)
            return new Vector2(-1f, 0f);
        else if (angle < -22.5f)
            return new Vector2(-COS45, COS45);
        else if (angle <= 22.5f)
            return new Vector2(0f, 1f);
        else if (angle < 67.5f)
            return new Vector2(COS45, COS45);
        else if (angle <= 112.5f)
            return new Vector2(1f, 0f);
        else if (angle < 157.5f)
            return new Vector2(COS45, -COS45);
        else if (angle <= 202.5f)
            return new Vector2(0f, -1f);

        return Vector2.zero;
    }
}
