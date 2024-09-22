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
    public Prisoner prisoner; // Prisoner 구조체 타입으로 변경

    public void SetPrisonerData(string name, int hp, int proficiency, int strength, string crime, int erosion, Sprite headSprite, Sprite bodySprite)
    {
        prisoner = new Prisoner(name, hp, proficiency, strength, crime, erosion, headSprite, bodySprite);
        UpdateUI(); // 데이터 설정 후 UI 업데이트
    }

    public void UpdateUI()
    {
        if (nameText != null) nameText.text = prisoner.name;
        if (hpText != null) hpText.text = "HP: " + prisoner.hp.ToString();
        if (proficiencyText != null) proficiencyText.text = "숙련도: " + prisoner.proficiency.ToString();
        if (strengthText != null) strengthText.text = "근력: " + prisoner.strength.ToString();
        if (crimeText != null) crimeText.text = "범죄: " + prisoner.crime;
        if (head != null) head.sprite = prisoner.head; // 머리 이미지 업데이트
        if (body != null) body.sprite = prisoner.body; // 몸 이미지 업데이트
    }
}
