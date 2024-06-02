using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMineShop : UILayout
{
    Button dim;
    GameObject shopPopup;
    Button sellAll;

    private void Awake()
    {
        dim = gameObject.Find<Button>("Dim");
        shopPopup = gameObject.Find("ShopPopup");
        sellAll = gameObject.Find<Button>("SellAllButton");

        sellAll.onClick.AddListener(() => OnClickSellAll());
        dim.onClick.AddListener(() => HideShopPopup());
    }

    private void Start()
    {
        HideShopPopup();
    }

    public void ShowShopPopup()
    {
        dim.gameObject.SetActive(true);
        shopPopup.SetActive(true);
        sellAll.gameObject.SetActive(true);
    }

    public void HideShopPopup()
    {
        dim.gameObject.SetActive(false);
        shopPopup.SetActive(false);
        sellAll.gameObject.SetActive(false);
    }

    void OnClickSellAll()
    {
        var price = 1000;
        var message = $"�� �Ǹűݾ��� {price}��� �Դϴ�. ������ �Ľðڽ��ϱ�?";
        Managers.ui.GetLayout<UILayoutNotice>().ShowNoticePopup(message,
            () =>
            {
                LocalPacketSender.C_SellItem(true, 0, 0);
                Managers.ui.GetLayout<UILayoutNotice>().HideNoticePopup();
            },
            () =>
            {
                Managers.ui.GetLayout<UILayoutNotice>().HideNoticePopup();
            });
    }
}
