using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuageBar : MonoBehaviour
{
    public Image fill;
    public TMP_Text amountText;

    public void SetGuage(long fullAmount, long nowAmount)
    {
        if (fullAmount == 0)
            return;

        fill.fillAmount = (float)nowAmount / fullAmount;
        amountText.text = $"{nowAmount}/{fullAmount}";
    }
}
