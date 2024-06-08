using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lode : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.ui.ShowPopup<UILayoutTriggerButton>().SetButton("Ã¤±¤½ÃÀÛ", () => LocalPacketSender.C_LodeAttackStart(id));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.ui.HidePopup<UILayoutTriggerButton>();
            //Managers.ui.GetLayout<UILayoutMineGame>().ShowMineStartButton(false);
        }
    }
}
