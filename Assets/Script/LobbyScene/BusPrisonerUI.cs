using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusPrisonerUI : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollRect 참조
    public RectTransform content; // Content 참조
    public DateSystem dateSystem;
    public Button changeDateButton;

    public Text[] nameTexts; // 6개의 Text UI 요소 배열
    private readonly char[] name1 = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly char[] name2 = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private readonly char[] name3 = new char[] { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    private readonly string[] firstNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };
    //private List<string> prisonerNames = new List<string> { "강병찬", "김민성", "박광희", "이상준", "이주원", "이영상", "장영규", "정재민", "진주형", "한정도" };
    private List<string> currentPrisonerNames = new List<string>();

    void Start()
    {
        // ScrollRect의 위치를 초기화
        scrollRect.verticalNormalizedPosition = 1f;

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

        // ScrollRect의 위치를 초기화하는 코루틴 시작
        StartCoroutine(SetScrollPosition());
    }

    void OnDestroy()
    {
        // 날짜 변경 이벤트 구독 해제
        if (dateSystem != null)
        {
            dateSystem.OnDateChanged -= UpdatePrisonerNames;
        }
    }

    IEnumerator SetScrollPosition()
    {
        // UI 레이아웃이 완료될 때까지 한 프레임 대기
        yield return null;
        scrollRect.verticalNormalizedPosition = 1f;
    }

    void OnChangeDateButtonClick()
    {
        if (dateSystem != null)
        {
            dateSystem.ChangeDate();
        }
    }

    private void UpdatePrisonerNames()
    {
        // 랜덤한 이름 생성
        currentPrisonerNames.Clear();
        for (int i = 0; i < 6; i++)
        {
            currentPrisonerNames.Add(GenerateRandomName());
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

    private string GenerateRandomName()
    {
        string familyName = firstNames[Random.Range(0, firstNames.Length)];
        char firstNameInitial1 = CreateRandomKoreanChar();
        char firstNameInitial2 = CreateRandomKoreanChar();
        return familyName + firstNameInitial1.ToString() + firstNameInitial2.ToString();
    }

    private char CreateRandomKoreanChar()
    {
        int index1 = Random.Range(0, name1.Length);
        int index2 = Random.Range(0, name2.Length);
        int index3 = Random.Range(0, name3.Length);

        int fName = name1[index1] - 'ㄱ';
        int sName = name2[index2] - 'ㅏ';
        int tName = name3[index3] == '\0' ? 0 : name3[index3] - 'ㄱ' + 1;

        int unicode = 0xAC00 + (fName * 21 * 28) + (sName * 28) + tName;

        return (char)unicode;
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
