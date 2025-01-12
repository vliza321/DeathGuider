using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusPrisonerUI : MonoBehaviour
{
    public PrisonerDataUI prisonerDataUI; // Prisoner Data UI 스크립트 참조
    public StoredPrisoner storedPrisoner;

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

    public Sprite[] prisonerHeads; //  죄수의 얼굴 배열
    public Sprite[] prisonerBodies; // 죄수의 몸 배열

    public Image[] prisonerHeadsAppearence; //  죄수의 얼굴 배열
    public Image[] prisonerBodiesAppearence; // 죄수의 몸 배열

    public GameObject warningImage;

    private readonly char[] name1 = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly char[] name2 = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private readonly char[] name3 = new char[] { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    private readonly string[] firstNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };
    private readonly string[] crimes = new string[] { "방화", "살인", "패륜", "사기", "절도" };


    public UpgradeFloor upgradeFloor; // UpgradeFloor 스크립트 참조

    //public List<Prisoner> busPrisoners = new List<Prisoner>();

    public PrototypeUnitDataList busPrisonerDataList;

    void Start()
    {
        // DontDestroyOnLoad 객체에서 busPrisonerDataList를 로드
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                var manager = ddo.GetComponent<DontDestroyObjectManager>();
                if (manager != null)
                {
                    busPrisonerDataList = manager.PrototypeUnitDatas;
                    if (busPrisonerDataList == null)
                    {
                        Debug.LogError("busPrisonerDataList is not found in DontDestroyObjectManager.");
                    }
                    break;
                }
            }
        }

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
        if(busPrisonerDataList.PrototypeUnitDatas.Count > index)
        {
            busPrisonerDataList.PrototypeUnitDatas.RemoveAt(index);
            SetPrisoner();
            DisplayAllPrisonerData();
        }
    }

    void OnTransferButtonClick(int index)
    {
        if (upgradeFloor == null || prisonerDataUI == null)
        {
            Debug.LogError("UpgradeFloor or prisonerDataUI is not set.");
            return;
        }

        int floorCapacity = upgradeFloor.GetCapacityForCurrentFloor(); // 현재 층의 수용 한계
        int currentCapacity = storedPrisoner.uiContentParent.childCount; // 현재 층의 수감자 수

        // 새로운 프리펩 추가
        if (index >= 0 && index < busPrisonerDataList.PrototypeUnitDatas.Count)
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

            // 이동할 수감자 선택
            PrototypeUnitData selectedPrisoner = busPrisonerDataList.PrototypeUnitDatas[index];

            // 수감자에게 새로운 아이디 부여
            selectedPrisoner.ID = storedPrisoner.currentPrisonerCount++;  // currentPrisonerCount로 ID 부여 후 증가

            // 수감자 이동 작업
            storedPrisoner.AddPrisonerPrefab(index);

            // 리스트에서 제거
            busPrisonerDataList.PrototypeUnitDatas.RemoveAt(index);

            // UI 업데이트
            SetPrisoner();
            DisplayAllPrisonerData();
        }
    }



    void DisplayAllPrisonerData()
    {
        if (prisonerDataUI == null)
        {
            Debug.LogError("prisonerDataUI is not set.");
            return;
        }

        if (busPrisonerDataList == null || busPrisonerDataList.PrototypeUnitDatas == null)
        {
            Debug.LogError("busPrisonerDataList or PrototypeUnitDatas is not set.");
            return;
        }

        // 수감자 정보를 배열로 전달
        prisonerDataUI.DisplayPrisonerData(
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.Name).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.MaxHealthPoint).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.Handicraft).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.Strength).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.Crime.ToString()).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.ID).ToArray(),
        busPrisonerDataList.PrototypeUnitDatas.ConvertAll(p => p.Defense).ToArray() // Defense는 int[]로 전달
        );
    }

    private void UpdatePrisoner()
    {
        // 랜덤한 이름과 정보를 생성합니다.
        //busPrisoners.Clear();

        // 기존 데이터 초기화
        if (busPrisonerDataList == null || busPrisonerDataList.PrototypeUnitDatas == null)
        {
            Debug.LogError("busPrisonerDataList or PrototypeUnitDatas is not initialized.");
            return;
        }
        busPrisonerDataList.PrototypeUnitDatas.Clear();

        for (int i = 0; i < 6; i++)
        {
            var randomPrisoner = new PrototypeUnitData
            {
                Name = GenerateRandomName(),
                MaxHealthPoint = Random.Range(1, 11), // HP
                Handicraft = Random.Range(1, 11), // Proficiency
                Strength = Random.Range(1, 11), // Strength
                Crime = Random.Range(1, 11), // Crime
                Defense = Random.Range(1, 11), // Defense (디펜스 값 추가)
                HeadID = Random.Range(0, prisonerHeads.Length), // Head (ID로 처리)
                BodyID = Random.Range(0, prisonerBodies.Length) // Body (ID로 처리)
            };

            busPrisonerDataList.PrototypeUnitDatas.Add(randomPrisoner);
        }

        // 현재 선택된 죄수 이름들로 텍스트 설정
        SetPrisoner();
        DisplayAllPrisonerData();
    }

    void SetPrisoner()
    {
        //int maxPrisoners = Mathf.Min(nameTexts.Length, busPrisoners.Count);
        int maxPrisoners = Mathf.Min(nameTexts.Length, busPrisonerDataList.PrototypeUnitDatas.Count);

        for (int i = 0; i < nameTexts.Length; i++)
        {
            if (i < maxPrisoners)
            {
                var prisoner = busPrisonerDataList.PrototypeUnitDatas[i];

                nameTexts[i].text = prisoner.Name;
                hpTexts[i].text = "체력: " + prisoner.MaxHealthPoint;
                proficiencyTexts[i].text = "숙련도: " + prisoner.Handicraft;
                strengthTexts[i].text = "힘: " + prisoner.Strength;
                crimeTexts[i].text = "범죄: " + prisoner.Crime;

                prisonerHeadsAppearence[i].sprite = GetSpriteFromID(prisoner.HeadID);
                prisonerBodiesAppearence[i].sprite = GetSpriteFromID(prisoner.BodyID);

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

                prisonerHeadsAppearence[i].sprite = null;
                prisonerBodiesAppearence[i].sprite = null;

                if (prisonerImages[i] != null)
                {
                    prisonerImages[i].gameObject.SetActive(false);
                }
            }

            // Reject 버튼 활성화/비활성화
            if (rejectButtons[i] != null)
            {
                rejectButtons[i].gameObject.SetActive(i < maxPrisoners);
            }

            // Transfer 버튼 활성화/비활성화
            if (transferButtons[i] != null)
            {
                transferButtons[i].gameObject.SetActive(i < maxPrisoners);
            }
        }
    }

    // ID에 맞는 스프라이트를 반환하는 함수
    private Sprite GetSpriteFromID(int id)
    {
        // 예시: headID 또는 bodyID에 해당하는 스프라이트 배열이 있다고 가정
        if (id >= 0 && id < prisonerHeads.Length)
        {
            return prisonerHeads[id]; // head sprite
        }
        return null;
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