using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILayoutMiniMap : UILayout
{
    GameObject miniMap;
    RectTransform mapSprite;

    Vector2 startPos;
    Transform target;

    private void Awake()
    {
        miniMap = gameObject.Find("MiniMap");
        mapSprite = gameObject.Find<RectTransform>("MiniMap/Mask/Map");
    }

    private void Start()
    {
        startPos = mapSprite.anchoredPosition;
        target = Managers.obj.myPlayer.transform;
    }

    private void LateUpdate()
    {
        mapSprite.anchoredPosition = startPos - new Vector2(target.position.x / 230f * 760f, target.position.y / 230f * 760f);
    }

    public void SetMiniMap(int mapId)
    {
        if (mapId == 1002)
        {
            miniMap.SetActive(true);
            mapSprite.anchoredPosition = startPos;
        }
        else
            miniMap.SetActive(false);
    }
}
