using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredPrisoner : MonoBehaviour
{
    public string prisonerName;
    public int hp;
    public int proficiency;
    public int strength;
    public string crime;
    public int erosion;

    // UI 요소에 대한 참조 변수
    public Text nameText;
    public Text hpText;
    public Text proficiencyText;
    public Text strengthText;
    public Text crimeText;
    public Image prisonerImage; // 이미지를 사용할 경우

    // 프리팹이 활성화될 때 호출되는 메소드
    void Start()
    {
        // UI 요소에 정보를 설정
        if (nameText != null)
            nameText.text = prisonerName;

        if (hpText != null)
            hpText.text = "체력: " + hp;

        if (proficiencyText != null)
            proficiencyText.text = "숙련도: " + proficiency;

        if (strengthText != null)
            strengthText.text = "힘: " + strength;

        if (crimeText != null)
            crimeText.text = "범죄: " + crime;

        // 이미지가 있다면 설정
        if (prisonerImage != null)
        {
            // 이미지 설정 코드 (예: prisonerImage.sprite = ...)
        }
    }
}

