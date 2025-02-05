using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaboratorySystem : MonoBehaviour
{
    public string[] textContents = {
        "날짜의 리셋 시간 기준, 이감 대기 유닛의 최대 수치 증가",
        "수용소 증축 비용 감소",
        "체력 회복 비율 증가1",
        "체력 회복 비율 증가2",
        "죽음 침식도 회복 비율 증가",
        "최대 수용 인원 증가",
        "스탯 증가량 비율 증가",
        "최대 수용 인원 증가",
        "무기 재련 / 내구도 수리 / 무기 진화 비용 감소",
        "내구도 회복 비율 증가",
        "무기 구매 대상 무기 등급 증가",
        "무기 구매 대상 무기 가격 감소 및 무기 판매 대상 무기 가격 증가",
        "전투를 통해 획득하는 재화(골드 및 어둠 정수)의 증가",
        "전투 종료로 얻는 탐사 진척도 증가"
    };

    public int[] currency1Values = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    public int[] currency2Values = { 500, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000 };

    public GameObject LaboratoryPrefab;
    public Transform parentTransform;

    private List<GameObject> createdLabs = new List<GameObject>();
    private int currentIndex = -1;

    void Start()
    {
        for (int i = 0; i < textContents.Length; i++)
        {
            GameObject instance = Instantiate(LaboratoryPrefab, parentTransform);
            createdLabs.Add(instance);

            TextMeshProUGUI laboratoryNameText = instance.transform.Find("Laboratory Name Text")?.GetComponent<TextMeshProUGUI>();
            if (laboratoryNameText != null)
            {
                laboratoryNameText.text = textContents[i];
            }

            TextMeshProUGUI laboratoryNeedCoin1Text = instance.transform.Find("Laboratory Need Coin1 Text")?.GetComponent<TextMeshProUGUI>();
            if (laboratoryNeedCoin1Text != null && i < currency1Values.Length)
            {
                laboratoryNeedCoin1Text.text = $"{currency1Values[i]:N0} G";
            }

            TextMeshProUGUI laboratoryNeedCoin2Text = instance.transform.Find("Laboratory Need Coin2 Text")?.GetComponent<TextMeshProUGUI>();
            if (laboratoryNeedCoin2Text != null && i < currency2Values.Length)
            {
                laboratoryNeedCoin2Text.text = $"{currency2Values[i]:N0} K";
            }

            Button button = instance.GetComponentInChildren<Button>();
            if (button != null)
            {
                int index = i;
                button.onClick.AddListener(() => OnLabButtonClick(index, textContents.Length));

                if (i > 0)
                {
                    button.interactable = false;
                }
            }
        }
    }

    void OnLabButtonClick(int index, int total)
    {
        if (index == currentIndex + 1)
        {
            if (index < createdLabs.Count)
            {
                GameObject currentLab = createdLabs[index];

                Button currentButton = currentLab.GetComponentInChildren<Button>();
                if (currentButton != null)
                {
                    currentButton.interactable = false;
                }

                if (currentIndex >= 0 && currentIndex < createdLabs.Count)
                {
                    GameObject prevLab = createdLabs[currentIndex];
                    Button prevButton = prevLab.GetComponentInChildren<Button>();
                    if (prevButton != null)
                    {
                        prevButton.interactable = false;
                    }
                }
                currentIndex = index;

                EnableNextButton(index, total);
            }
        }
    }

    void EnableNextButton(int startIndex, int total)
    {
        if (startIndex + 1 < total)
        {
            Button button = createdLabs[startIndex + 1].GetComponentInChildren<Button>();
            if (button != null)
            {
                button.interactable = true;
            }
        }
    }

}
