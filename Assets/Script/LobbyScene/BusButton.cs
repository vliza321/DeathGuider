using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusButton : MonoBehaviour
{
    public Button busButton;
    public Button changeDateButton;
    public GameObject floorCanvas;
    public GameObject mainCanvas;
    public GameObject busPrisoner;
    public DateSystem dateSystem;

    // 원하는 위치와 크기
    public Vector2 newPosition = new Vector2(100, 100);
    public Vector2 newSize = new Vector2(200, 100);

    public Vector2 originPosition = new Vector2(-681, -277);
    public Vector2 originSize = new Vector2(120, 100);

    public Text[] nameTexts;
    private List<string> prisonerNames = new List<string> { "강병찬", "김민성", "박광희", "이상준", "이주원", "이영상", "장영규", "정재민", "진주형", "한정도" };
    private List<string> currentPrisonerNames = new List<string>();

    void Start()
    {
        // 버튼에 리스너 추가
        busButton.onClick.AddListener(OnButtonClick);

        // 날짜 변경 버튼에 리스너 추가
        if (changeDateButton != null)
        {
            changeDateButton.onClick.AddListener(OnChangeDateButtonClick);
        }

        // 날짜 변경 이벤트 구독
        if (dateSystem != null)
        {
            dateSystem.OnDateChanged += UpdatePrisonerNames;
            // 초기화 시 첫 날짜에 맞는 이름 설정
            UpdatePrisonerNames();
        }
    }

    void OnDestroy()
    {
        // 날짜 변경 이벤트 구독 해제
        if (dateSystem != null)
        {
            dateSystem.OnDateChanged -= UpdatePrisonerNames;
        }
    }

    void OnButtonClick()
    {
        RectTransform rectTransform = busButton.GetComponent<RectTransform>();

        // 위치 변경
        rectTransform.anchoredPosition = newPosition;

        // 크기 변경
        rectTransform.sizeDelta = newSize;

        // Floor Canvas 비활성화
        if (floorCanvas != null)
        {
            floorCanvas.SetActive(false);
        }

        // Main Canvas 비활성화
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }

        // Bus Canvas 활성화
        if (busPrisoner != null)
        {
            busPrisoner.SetActive(true);
        }

        // 현재 선택된 죄수 이름들로 텍스트 설정
        SetPrisonerNames();
    }

    void OnChangeDateButtonClick()
    {
        if (dateSystem != null)
        {
            dateSystem.ChangeDate();
        }
    }

    public void CloserButtonClick()
    {
        RectTransform closerrectTransform = busButton.GetComponent<RectTransform>();

        // 위치 변경
        closerrectTransform.anchoredPosition = originPosition;

        // 크기 변경
        closerrectTransform.sizeDelta = originSize;

        // Floor Canvas 활성화
        if (floorCanvas != null)
        {
            floorCanvas.SetActive(true);
        }

        // Main Canvas 활성화
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }

        // Bus Canvas 비활성화
        if (busPrisoner != null)
        {
            busPrisoner.SetActive(false);
        }
    }

    private void UpdatePrisonerNames()
    {
        // 이름 리스트 복사 및 셔플
        List<string> shuffledNames = new List<string>(prisonerNames);
        ShuffleList(shuffledNames);

        // 현재 죄수 이름 리스트를 초기화하고 셔플된 리스트에서 3명 선택
        currentPrisonerNames.Clear();
        for (int i = 0; i < Mathf.Min(3, shuffledNames.Count); i++)
        {
            currentPrisonerNames.Add(shuffledNames[i]);
        }

        // 현재 선택된 죄수 이름들로 텍스트 설정
        SetPrisonerNames();
    }

    private void SetPrisonerNames()
    {
        // 텍스트에 현재 선택된 죄수 이름 배정
        for (int i = 0; i < nameTexts.Length; i++)
        {
            if (nameTexts[i] != null)
            {
                if (i < currentPrisonerNames.Count)
                {
                    nameTexts[i].text = currentPrisonerNames[i];
                }
                else
                {
                    // 이름이 부족한 경우 기본값 또는 빈 문자열 설정
                    nameTexts[i].text = "이름 없음";
                }
            }
        }
    }

    // 리스트를 무작위로 셔플하는 메서드
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
