using UnityEngine;
using UnityEngine.UI;

public class PrisonerDataUI : MonoBehaviour
{
    public GameObject prisonerUIImage; // 이미지 객체를 연결할 변수
    public Text[] nameTexts;
    public Text[] hpTexts;
    public Text[] proficiencyTexts;
    public Text[] strengthTexts;
    public Text[] crimeTexts;
    public Text[] erosionTexts;

    void Start()
    {
        // 이미지 객체를 초반 비공개
        if (prisonerUIImage != null)
        {
            prisonerUIImage.SetActive(false);
        }
    }

    public void showPrisonerUI()
    {
        // 이미지 객체의 활성화 상태를 설정
        prisonerUIImage.SetActive(true);
    }

    public void dasfdshowPrisonerUI()
    {
        // 이미지 객체의 활성화 상태를 설정
        prisonerUIImage.SetActive(false);
    }

    public void DisplayPrisonerData(
        string[] names,
        int[] hps,
        int[] proficiencies,
        int[] strengths,
        string[] crimes,
        int[] erosions
    )
    {
        // 최대 수감자 수는 UI 요소 배열의 길이와 맞춰야 합니다.
        int maxCount = Mathf.Min(names.Length, nameTexts.Length);
        for (int i = 0; i < maxCount; i++)
        {
            nameTexts[i].text = names[i];
            hpTexts[i].text = "체력: " + hps[i];
            proficiencyTexts[i].text = "숙련도: " + proficiencies[i];
            strengthTexts[i].text = "힘: " + strengths[i];
            crimeTexts[i].text = "범죄: " + crimes[i];
            erosionTexts[i].text = "침식도: " + erosions[i];
        }

        // 배열의 길이보다 많은 UI 요소는 숨깁니다.
        for (int i = maxCount; i < nameTexts.Length; i++)
        {
            nameTexts[i].text = "";
            hpTexts[i].text = "";
            proficiencyTexts[i].text = "";
            strengthTexts[i].text = "";
            crimeTexts[i].text = "";
            erosionTexts[i].text = "";
        }
    }
}