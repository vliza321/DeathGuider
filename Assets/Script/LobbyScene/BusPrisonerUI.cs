using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusPrisonerUI : MonoBehaviour
{
    public PrisonerDataUI prisonerDataUI; // Prisoner Data UI 스크립트 참조
    public ScrollRect scrollRect; // ScrollRect 참조
    public RectTransform content; // Content 참조
    public DateSystem dateSystem;
    public Button changeDateButton;
    public Button[] rejectButtons; // 각 죄수에 대한 Reject 버튼 배열
    public Button[] transferButtons; // 각 죄수에 대한 Transfer 버튼 배열

    public TextMeshProUGUI[] nameTexts; // 6개의 Text UI 요소 배열
    public Text[] hpTexts; // 6개의 HP Text UI 요소 배열
    public Text[] proficiencyTexts; // 6개의 숙련도 Text UI 요소 배열
    public Text[] strengthTexts; // 6개의 힘 Text UI 요소 배열
    public Text[] crimeTexts; // 6개의 범죄 Text UI 요소 배열
    public Image[] prisonerImages; // 각 죄수에 대한 이미지 배열 (옵션)
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
    private List<int> currentPrisonerErosions = new List<int>();

    public UpgradeFloor upgradeFloor; // UpgradeFloor 스크립트 참조
    public int currentPrisonerCount = 0; // 현재 추가된 수감자 수를 추적하는 카운터
    public GameObject prisonerUIPrefab;
    public RectTransform uiContentParent;
    public float prefabSpacing = 10f;
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

        // 초기 UI 콘텐츠 높이 조정
        //AdjustContentHeight();
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
            // 데이터 제거
            currentPrisonerNames.RemoveAt(index);
            currentPrisonerHPs.RemoveAt(index);
            currentPrisonerProficiencies.RemoveAt(index);
            currentPrisonerStrength.RemoveAt(index);
            currentPrisonerCrimes.RemoveAt(index);
            currentPrisonerErosions.RemoveAt(index); // 침식도 데이터도 제거

            // UI 업데이트
            SetPrisoner();
            DisplayAllPrisonerData();

            // ContentHeight 조정
            //AdjustContentHeight();
        }
    }

    void OnTransferButtonClick(int index)
    {
        if (upgradeFloor == null || uiContentParent == null || prisonerDataUI == null)
        {
            Debug.LogError("UpgradeFloor, uiContentParent or prisonerDataUI is not set.");
            return;
        }

        int floorCapacity = upgradeFloor.GetCapacityForCurrentFloor(); // 현재 층의 수용 한계
        int currentCapacity = uiContentParent.childCount; // 현재 층의 수감자 수

        // 새로운 프리펩 추가
        if (index >= 0 && index < currentPrisonerNames.Count)
        {
            // 수용 한계를 초과하는 경우 경고 이미지 표시 및 이동 작업 중단
            if (currentCapacity >= floorCapacity)
            {
                if (warningImage != null)
                {
                    warningImage.SetActive(true); // 경고 이미지 표시
                }
                return; // 이동 작업 중단
            }

            // 경고 이미지가 이미 비활성화된 경우 다시 활성화
            if (warningImage != null && warningImage.activeSelf)
            {
                warningImage.SetActive(false);
            }
            // 수감자 이동 작업
            AddPrisonerPrefab(index);

            // 리스트에서 제거
            currentPrisonerNames.RemoveAt(index);
            currentPrisonerHPs.RemoveAt(index);
            currentPrisonerProficiencies.RemoveAt(index);
            currentPrisonerStrength.RemoveAt(index);
            currentPrisonerCrimes.RemoveAt(index);
            currentPrisonerErosions.RemoveAt(index);

            // UI 업데이트
            SetPrisoner();
            DisplayAllPrisonerData();
        }
    }
    void AddPrisonerPrefab(int index)
    {
        MoveExistingPrefabsDown();
        ++currentPrisonerCount;
        AdjustContentHeight();
        // 새로운 프리펩을 생성합니다.
        GameObject prisonerUI = Instantiate(prisonerUIPrefab, uiContentParent);

        // RectTransform 가져오기
        RectTransform rectTransform = prisonerUI.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // 프리펩의 높이와 콘텐츠의 높이
            float prefabHeight = rectTransform.rect.height;

            float contentHeight = uiContentParent.GetComponent<RectTransform>().rect.height;
            Debug.Log(contentHeight);
            // 프리펩의 위치를 콘텐츠의 최상단에 맞추기 위한 yOffset 계산
            //float yOffset = (prefabHeight) * currentPrisonerCount - prefabSpacing;
            float yOffset = (contentHeight / 2) - prefabSpacing;
            // 새 프리펩의 y 위치
            rectTransform.anchoredPosition = new Vector2(0, yOffset); //이건 프리펩 생성 스크립트
            
        }

        ScrollRect scrollRect = uiContentParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        // StoredPrisoner 컴포넌트에 데이터 설정
        StoredPrisoner prisonerScript = prisonerUI.GetComponent<StoredPrisoner>();
        if (prisonerScript != null)
        {
            prisonerScript.prisonerName = currentPrisonerNames[index];
            prisonerScript.hp = currentPrisonerHPs[index];
            prisonerScript.proficiency = currentPrisonerProficiencies[index];
            prisonerScript.strength = currentPrisonerStrength[index];
            prisonerScript.crime = currentPrisonerCrimes[index];
            prisonerScript.erosion = currentPrisonerErosions[index];
        }
        // uiContentParent의 높이를 조정
    }

    void MoveExistingPrefabsDown()
    {

        for (int i = 0; i < uiContentParent.childCount; i++)
        {
            Transform child = uiContentParent.GetChild(i);
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // 프리펩의 현재 위치를 가져옵니다.
                Vector2 newPosition = rectTransform.anchoredPosition;

                // 프리펩을 아래로 이동시키기 위해 위치를 조정합니다.
                newPosition.y -= prefabSpacing+ 80; // 모든 기존 프리펩을 내려서 새 프리펩을 최상단에 맞춤
                //80의 숫자는 프리펩이 증가할때 비율을 맞춰주기 위한 수
                rectTransform.anchoredPosition = newPosition;
            }
        }
    }

    void AdjustContentHeight()
    {
        RectTransform contentRectTransform = uiContentParent.GetComponent<RectTransform>();

        // 프리펩의 높이와 간격
        RectTransform prefabRectTransform = prisonerUIPrefab.GetComponent<RectTransform>();
        float prefabHeight = prefabRectTransform.rect.height;

        // 총 높이 계산 180 360
        float totalHeight = (prefabSpacing + (prefabHeight)) * (currentPrisonerCount);

        // Content의 높이를 조정
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);

        // Content의 y 위치를 조정 -> 아래로만 증가
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, -totalHeight);
    }

    void DisplayAllPrisonerData()
    {
        if (prisonerDataUI == null)
        {
            Debug.LogError("prisonerDataUI is not set.");
            return;
        }

        // 수감자 정보를 배열로 전달
        prisonerDataUI.DisplayPrisonerData(
            currentPrisonerNames.ToArray(),
            currentPrisonerHPs.ToArray(),
            currentPrisonerProficiencies.ToArray(),
            currentPrisonerStrength.ToArray(),
            currentPrisonerCrimes.ToArray(),
            currentPrisonerErosions.ToArray()
        );
    }

    private void UpdatePrisoner()
    {
        // 랜덤한 이름과 정보를 생성합니다.
        currentPrisonerNames.Clear();
        currentPrisonerHPs.Clear();
        currentPrisonerProficiencies.Clear();
        currentPrisonerStrength.Clear();
        currentPrisonerCrimes.Clear();
        currentPrisonerErosions.Clear(); // 침식도 데이터도 초기화

        for (int i = 0; i < 6; i++)
        {
            currentPrisonerNames.Add(GenerateRandomName());
            currentPrisonerHPs.Add(Random.Range(1, 11));
            currentPrisonerProficiencies.Add(Random.Range(1, 11));
            currentPrisonerStrength.Add(Random.Range(1, 11));
            currentPrisonerCrimes.Add(crimes[Random.Range(0, crimes.Length)]);
            currentPrisonerErosions.Add(0);
        }

        // 현재 선택된 죄수 이름들로 텍스트 설정
        SetPrisoner();
        DisplayAllPrisonerData();
    }

    void SetPrisoner()
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

        // 한글 범위에 있는지 확인
        if (unicode >= 0xAC00 && unicode <= 0xD7A3)
        {
            return (char)unicode;
        }
        else
        {
            // 한글 범위가 아니면, 다시 시도하도록
            return CreateRandomKoreanChar();
        }
    }


}
