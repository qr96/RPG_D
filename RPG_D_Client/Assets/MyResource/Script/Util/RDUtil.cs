using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RDUtil
{
    public static string MoneyComma(long money)
    {
        if (money == 0)
            return "0";
        return money.ToString("#,##0");
    }
}
