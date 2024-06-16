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
                Managers.ui.GetPopup<UIPopupTriggerButton>().SetButton("광산입장",
                    () => LocalPacketSender.C_MoveMap(1002));
                Managers.ui.ShowPopup<UIPopupTriggerButton>();
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UIPopupTriggerButton>();
            });

            var mineralShop = mapList[0].Find<MovePortal>("MineralShop");
            mineralShop.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UIPopupTriggerButton>().SetButton("상점이용",
                    () => Managers.ui.ShowPopup<UIPopupMineShop>());
                Managers.ui.ShowPopup<UIPopupTriggerButton>();
            });
            mineralShop.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UIPopupTriggerButton>();
            });
            mineralShop.SetNameTag(Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(mineralShop.gameObject, "광물상인"));

            var equipShop = mapList[0].Find<MovePortal>("EquipShop");
            equipShop.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UIPopupTriggerButton>().SetButton("상점이용",
                    () => Managers.ui.ShowPopup<UIPopupEquipShop>());
                Managers.ui.ShowPopup<UIPopupTriggerButton>();
            });
            equipShop.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UIPopupTriggerButton>();
            });
            equipShop.SetNameTag(Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(equipShop.gameObject, "대장장이"));
        }
        else if (mapId == 1002)
        {
            mapList[1].SetActive(true);

            Managers.obj.myPlayer.transform.position = Vector3.zero;
            Managers.ui.GetLayout<UILayoutMineHUD>().SetDepthIndicator(true);

            var portal = mapList[1].Find<MovePortal>("TownPortal");
            portal.SetTriggerEnterEvent(() =>
            {
                Managers.ui.GetPopup<UIPopupTriggerButton>().SetButton("탐험종료",
                    () => LocalPacketSender.C_MineGameResult());
                Managers.ui.ShowPopup<UIPopupTriggerButton>();
            });
            portal.SetTriggerExitEvent(() =>
            {
                Managers.ui.HidePopup<UIPopupTriggerButton>();
            });
        }
    }
}
