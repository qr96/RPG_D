using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button mineButton;
    public GameObject minePopup;

    private void Awake()
    {
        mineButton.onClick.AddListener(() =>
        {
            ShowMinePopup();
            HideMinButton();
        });
    }

    public void ShowMineButton()
    {
        mineButton.gameObject.SetActive(true);
    }

    public void HideMinButton()
    {
        mineButton.gameObject.SetActive(false);
    }

    public void ShowMinePopup()
    {
        minePopup.gameObject.SetActive(true);
    }
}
