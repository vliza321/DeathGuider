using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoredPrisonerstat : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI proficiencyText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI crimeText;
    public Image head;
    public Image body;

    // Prisoner 구조체의 필드
    public PrototypeUnitData prisoner; // Prisoner 구조체 타입으로 변경
    public Sprite[] headSprites; // 머리 이미지 배열
    public Sprite[] bodySprites; // 몸 이미지 배열

    public void SetPrisonerData(int id, string name, int maxHealthPoint, int strength, int defense, int handicraft, int crime, int headID, int bodyID)
    {
        prisoner = new PrototypeUnitData
        {
            ID = id,
            Name = name,
            MaxHealthPoint = maxHealthPoint,
            Strength = strength,
            Defense = defense,
            Handicraft = handicraft,
            Crime = crime,
            HeadID = headID,
            BodyID = bodyID
        };
        UpdateUI(); // 데이터 설정 후 UI 업데이트
    }

    public void UpdateUI()
    {
        if (nameText != null) nameText.text = prisoner.Name;
        if (hpText != null) hpText.text = "HP: " + prisoner.MaxHealthPoint.ToString();
        if (proficiencyText != null) proficiencyText.text = "숙련도: " + prisoner.Handicraft.ToString();
        if (strengthText != null) strengthText.text = "근력: " + prisoner.Strength.ToString();
        if (crimeText != null) crimeText.text = "범죄: " + prisoner.Crime.ToString();
        if (head != null && prisoner.HeadID >= 0) head.sprite = headSprites[prisoner.HeadID];
        if (body != null && prisoner.BodyID >= 0) body.sprite = bodySprites[prisoner.BodyID];
    }
}
