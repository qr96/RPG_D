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
            Managers.ui.GetLayout<UILayoutMineHUD>().SetDepthIndicator(false);

            var portal = mapList[0].Find<MovePortal>("MinePortal");
            portal.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UILayoutTriggerButton>().SetButton("±§ªÍ¿‘¿Â",
                    () => LocalPacketSender.C_MoveMap(1002));
                Managers.ui.ShowPopup<UILayoutTriggerButton>();
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UILayoutTriggerButton>();
            });

            var shop = mapList[0].Find<MovePortal>("Shop");
            shop.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UILayoutTriggerButton>().SetButton("ªÛ¡°¿ÃøÎ",
                    () => Managers.ui.ShowPopup<UILayoutMineShop>());
                Managers.ui.ShowPopup<UILayoutTriggerButton>();
            });
            shop.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UILayoutTriggerButton>();
            });
            shop.SetNameTag(Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(shop.transform, "±§π∞ªÛ¿Œ"));
        }
        else if (mapId == 1002)
        {
            mapList[1].SetActive(true);

            Managers.obj.myPlayer.transform.position = Vector3.zero;
            Managers.ui.GetLayout<UILayoutMineHUD>().SetDepthIndicator(true);

            var portal = mapList[1].Find<MovePortal>("TownPortal");
            portal.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UILayoutTriggerButton>().SetButton("≈Ω«Ë¡æ∑·",
                    () => LocalPacketSender.C_MineGameResult());
                Managers.ui.ShowPopup<UILayoutTriggerButton>();
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UILayoutTriggerButton>();
            });
        }
    }
}
