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
            Managers.ui.GetLayout<UILayoutMineGame>().ShowMineStartButton(true, id);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.ui.GetLayout<UILayoutMineGame>().ShowMineStartButton(false);
        }
    }
}
