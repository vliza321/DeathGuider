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
    public Button[] rejectButtons; // 각 죄수에 대한 Reject 버튼 배열
    public Button[] transferButtons; // 각 죄수에 대한 Transfer 버튼 배열

    public Text[] nameTexts; // 6개의 Text UI 요소 배열
    public Text[] hpTexts; // 6개의 HP Text UI 요소 배열
    public Text[] proficiencyTexts; // 6개의 숙련도 Text UI 요소 배열
    public Text[] strengthTexts; // 6개의 힘 Text UI 요소 배열
    public Text[] crimeTexts; // 6개의 범죄 Text UI 요소 배열
    public Image[] prisonerImages; // 각 죄수에 대한 이미지 배열 (옵션)
    //public Image warningImage; // 공간 부족 경고 이미지
    public GameObject warningImage;

    private readonly char[] name1 = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly char[] name2 = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private readonly char[] name3 = new char[] { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    private readonly string[] firstNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };
    private readonly string[] crimes = new string[] { "방화", "살인", "패륜", "사기", "절도" };

    private List<string> currentPrisonerNames = new List<string>();
    private List<int> currentPrisonerHPs = new List<int>();
    private List<int> currentPrisonerProficiencies = new List<int>();
    private List<int> currentPrisonerStrength = new List<int>();
    private List<string> currentPrisonerCrimes = new List<string>();

    public UpgradeFloor upgradeFloor; // UpgradeFloor 스크립트 참조
    public Transform prisonerParent; // 하이어라키의 prisoner 오브젝트
    private int currentPrisonerCount = 0; // 현재 추가된 수감자 수를 추적하는 카운터


    void Start()
    {
        // ScrollRect의 위치를 초기화
        scrollRect.verticalNormalizedPosition = 1f;

        // 날짜 변경 버튼에 리스너 추가
        if (changeDateButton != null)
        {
            changeDateButton.onClick.AddListener(OnChangeDateButtonClick);
        }

        // Reject 버튼에 리스너 추가
        if (rejectButtons != null)
        {
            for (int i = 0; i < rejectButtons.Length; i++)
            {
                int index = i; // 로컬 변수로 인덱스 저장
                rejectButtons[i].onClick.AddListener(() => OnRejectButtonClick(index));
            }
        }

        // Transfer 버튼에 리스너 추가
        if (transferButtons != null)
        {
            for (int i = 0; i < transferButtons.Length; i++)
            {
                int index = i; // 로컬 변수로 인덱스 저장
                transferButtons[i].onClick.AddListener(() => OnTransferButtonClick(index));
            }
        }

        // 날짜 변경 이벤트 구독
        if (dateSystem != null)
        {
            dateSystem.OnDateChanged += UpdatePrisoner;
            // 초기화 시 첫 날짜에 맞는 이름과 체력 설정
            UpdatePrisoner();
        }

        // ScrollRect의 위치를 초기화하는 코루틴 시작
        StartCoroutine(SetScrollPosition());

        // UpgradeFloor 스크립트 참조 설정
        upgradeFloor = FindObjectOfType<UpgradeFloor>();

        // 경고 이미지 초기 숨김
        if (warningImage != null)
        {
            warningImage.gameObject.SetActive(false);
        }

        // 하이어라키의 prisoner 오브젝트 찾기
        prisonerParent = GameObject.Find("StoredPrisoner")?.transform;
    }

    void OnDestroy()
    {
        // 날짜 변경 이벤트 구독 해제
        if (dateSystem != null)
        {
            dateSystem.OnDateChanged -= UpdatePrisoner;
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

    void OnRejectButtonClick(int index)
    {
        // 인덱스에 해당하는 죄수를 제거합니다.
        if (index >= 0 && index < currentPrisonerNames.Count)
        {
            currentPrisonerNames.RemoveAt(index);
            currentPrisonerHPs.RemoveAt(index);
            currentPrisonerProficiencies.RemoveAt(index);
            currentPrisonerStrength.RemoveAt(index);
            currentPrisonerCrimes.RemoveAt(index);

            // UI 업데이트
            SetPrisoner();
        }
    }

    public void OnTransferButtonClick(int index)
    {
        if (upgradeFloor == null || prisonerParent == null)
        {
            Debug.LogError("UpgradeFloor or prisonerParent is not set.");
            return;
        }

        int floorCapacity = upgradeFloor.floorCount * 4; // 한 층당 최대 4명 수용 가능
        int currentCapacity = prisonerParent.childCount; // 현재 층의 수감자 수

        if (index >= 0 && index < currentPrisonerNames.Count)
        {
            if (currentCapacity >= floorCapacity)
            {
                // 공간 부족 시 경고 이미지 표시
                if (warningImage != null)
                {
                    warningImage.SetActive(true);
                }
            }
            else
            {
                // 공간이 충분할 경우, 수감자 정보를 하이어라키에 이동
                GameObject prisonerObj = new GameObject("Prisoner_" + (currentPrisonerCount + 1)); // 고유 이름 설정
                prisonerObj.transform.SetParent(prisonerParent);

                StoredPrisoner prisonerScript = prisonerObj.AddComponent<StoredPrisoner>();
                prisonerScript.prisonerName = currentPrisonerNames[index];
                prisonerScript.hp = currentPrisonerHPs[index];
                prisonerScript.proficiency = currentPrisonerProficiencies[index];
                prisonerScript.strength = currentPrisonerStrength[index];
                prisonerScript.crime = currentPrisonerCrimes[index];

                // 리스트에서 제거
                currentPrisonerNames.RemoveAt(index);
                currentPrisonerHPs.RemoveAt(index);
                currentPrisonerProficiencies.RemoveAt(index);
                currentPrisonerStrength.RemoveAt(index);
                currentPrisonerCrimes.RemoveAt(index);

                // 수감자 카운터 증가
                currentPrisonerCount++;

                // UI 업데이트
                SetPrisoner();
            }
        }
    }


    private void UpdatePrisoner()
    {
        // 랜덤한 이름과 정보를 생성합니다.
        currentPrisonerNames.Clear();
        currentPrisonerHPs.Clear();
        currentPrisonerProficiencies.Clear();
        currentPrisonerStrength.Clear();
        currentPrisonerCrimes.Clear();

        for (int i = 0; i < 6; i++)
        {
            currentPrisonerNames.Add(GenerateRandomName());
            currentPrisonerHPs.Add(Random.Range(1, 11));
            currentPrisonerProficiencies.Add(Random.Range(1, 11));
            currentPrisonerStrength.Add(Random.Range(1, 11));
            currentPrisonerCrimes.Add(crimes[Random.Range(0, crimes.Length)]);
        }

        // 현재 선택된 죄수 이름들로 텍스트 설정
        SetPrisoner();
    }

    private void SetPrisoner()
    {
        for (int i = 0; i < nameTexts.Length; i++)
        {
            if (i < currentPrisonerNames.Count)
            {
                nameTexts[i].text = currentPrisonerNames[i];
                hpTexts[i].text = "체력: " + currentPrisonerHPs[i];
                proficiencyTexts[i].text = "숙련도: " + currentPrisonerProficiencies[i];
                strengthTexts[i].text = "힘: " + currentPrisonerStrength[i];
                crimeTexts[i].text = "범죄: " + currentPrisonerCrimes[i];

                if (prisonerImages[i] != null)
                {
                    prisonerImages[i].gameObject.SetActive(true);
                }
            }
            else
            {
                // 정보가 없는 경우 UI 요소를 숨깁니다.
                nameTexts[i].text = "";
                hpTexts[i].text = "";
                proficiencyTexts[i].text = "";
                strengthTexts[i].text = "";
                crimeTexts[i].text = "";

                if (prisonerImages[i] != null)
                {
                    prisonerImages[i].gameObject.SetActive(false);
                }
            }

            // Reject 버튼 활성화/비활성화
            if (rejectButtons[i] != null)
            {
                rejectButtons[i].gameObject.SetActive(i < currentPrisonerNames.Count);
            }

            // Transfer 버튼 활성화/비활성화
            if (transferButtons[i] != null)
            {
                transferButtons[i].gameObject.SetActive(i < currentPrisonerNames.Count);
            }
        }
    }

    private string GenerateRandomName()
    {
        string familyName = firstNames[Random.Range(0, firstNames.Length)];
        string firstName = CreateRandomKoreanChar().ToString() + CreateRandomKoreanChar().ToString();
        return familyName + firstName;
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

        // 유니티가 읽지 못하는 범위의 유니코드 문자 배제
        if (unicode < 0xAC00 || unicode > 0xD7A3)
        {
            unicode = 0xAC00; // 기본값으로 설정
        }

        return (char)unicode;
    }
}
