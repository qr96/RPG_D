using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> mapList = new List<GameObject>();

    public void SetMap(int mapId)
    {
        foreach (var map in mapList)
            map.SetActive(false);

        if (mapId == 1001)
        {
            mapList[0].SetActive(true);

            Managers.obj.myPlayer.transform.position = Vector3.zero;

            var portal = mapList[0].Find<MovePortal>("MinePortal");
            portal.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().ShowButton("광산입장",
                    () =>
                    {
                        LocalPacketSender.C_MoveMap(1002);
                    });
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().HideButton();
            });

            var shop = mapList[0].Find<MovePortal>("Shop");
            shop.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().ShowButton("상점이용",
                    () =>
                    {
                        Managers.ui.GetLayout<UILayoutMineShop>().ShowShopPopup();
                    });
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().HideButton();
            });
        }
        else if (mapId == 1002)
        {
            mapList[1].SetActive(true);

            Managers.obj.myPlayer.transform.position = Vector3.zero;

            var portal = mapList[1].Find<MovePortal>("TownPortal");
            portal.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().ShowButton("탐험종료",
                    () =>
                    {
                        LocalPacketSender.C_MineGameResult();
                    });
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.GetLayout<UILayoutTriggerButton>().HideButton();
            });
        }
    }
}
