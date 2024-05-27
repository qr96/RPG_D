using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lode : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.Instance.ui.ShowMineButton();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.Instance.ui.HideMinButton();
        }
    }
}
