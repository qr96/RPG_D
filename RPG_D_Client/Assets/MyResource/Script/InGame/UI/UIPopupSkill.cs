using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkill : UIPopup
{
    Button dimButton;
    GameObject inventoryPopup;
    GameObject itemSlotParent;
    GameObject itemSlotPrefab;
    SlotView inventoryView;

    public List<GameObject> itemSlotPool = new List<GameObject>();

    public override void InputEvent()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            dimButton.onClick.Invoke();
    }

    public override void OnCreate()
    {
        dimButton = gameObject.Find<Button>("Dim");
        inventoryPopup = gameObject.Find("InventoryPopup");
        itemSlotParent = inventoryPopup.Find("Scroll View/Viewport/Content");
        itemSlotPrefab = itemSlotParent.Find("ItemSlot");

        inventoryView = inventoryPopup.GetComponent<SlotView>();

        dimButton.onClick.AddListener(() => Hide());

        itemSlotPrefab.SetActive(false);
        inventoryView.SetPrefab(itemSlotPrefab, itemSlotParent.transform);
    }

    public void SetSkills(List<Skill> skills)
    {
        inventoryView.SetInventory(skills,
            (skill, slot) =>
            {
                var itemImage = slot.Find<Image>("ItemImage");
                itemImage.sprite = Resources.Load<Sprite>(DataTable.GetSkillSpritePath(skill.type));

                var levelText = slot.Find<TMP_Text>("Level");
                levelText.text = skill.level <= 0 ? "미획득" : $"Lv.{skill.level}";

                var disable = slot.Find("Disable");
                disable.SetActive(skill.level <= 0);

                var expGuage = slot.Find<GuageBar>("EXPGuage");
                expGuage.SetGuage(DataTable.GetSkillMaxExp(skill.level), skill.exp);

                var slotButton = slot.GetComponent<Button>();
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() =>
                {
                    Managers.ui.ShowPopup<UIPopupSkillInfo>().Set
                    (
                        DataTable.GetSkillName(skill.type),
                        DataTable.GetSkillInfo(skill.type, skill.level),
                        itemImage.sprite,
                        skill.level,
                        DataTable.GetSkillMaxExp(skill.level),
                        skill.exp
                        );
                });

                slot.SetActive(true);
            });
    }
}
