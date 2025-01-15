using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorSystem : MonoBehaviour
{
    public FloorUIManager floorUIManager;


    public GameObject floorPrefab;
    public Transform parentTransform;
    public Button upgradeButton;
    private List<GameObject> floors = new List<GameObject>();

    public GameObject prisonerInfoPrefab;
    public Transform contentParent;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    private DontDestroyObjectManager DDOManager;
    void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            DDO = null;
        }

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }
        else
        {
            Debug.LogWarning("Upgrade 버튼이 설정되지 않았습니다!");
        }

        GameObject firstFloor = GameObject.Find("Floor 1");
        if (firstFloor != null)
        {
            floors.Add(firstFloor);
        }
        else
        {
            Debug.LogError("Floor 1이 Hierarchy에 없습니다!");
        }
    }

    public void OnUpgradeButtonClicked()
    {
        if (floors.Count > 0)
        {
            GameObject topFloor = floors[floors.Count - 1];

            Vector2 newPosition = new Vector2(
                topFloor.transform.position.x,
                topFloor.transform.position.y + 192
            );

            CreateFloor(newPosition);
        }
    }

    private void CreateFloor(Vector2 position)
    {
        DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor++;

        if (!DDOManager.SaveData())
        {
            Debug.Log("Fail Save Data");
        }

        int currentFloor = DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor;

        if (floorPrefab != null)
        {
            GameObject newFloor = Instantiate(floorPrefab, position, Quaternion.identity, parentTransform);
            newFloor.name = $"Floor {currentFloor}";
            floors.Add(newFloor);
            currentFloor++;

            Button floorButton = newFloor.GetComponent<Button>();
            if (floorButton != null)
            {
                floorButton.onClick.AddListener(() =>
                {
                    Debug.Log($"{newFloor.name} 클릭됨!");
                    floorUIManager.OpenFloorPrisonerUI();
                });
            }
            else
            {
                Debug.LogWarning($"Floor {currentFloor}에 Button 컴포넌트가 없습니다.");
            }

            Debug.Log($"{newFloor.name}이 생성되었습니다.");
        }
        else
        {
            Debug.LogWarning("FloorPrefab이 설정되지 않았습니다!");
        }
    }

    public void DisplayFloorPrisoners()
    {
        // 기존 자식 객체 제거
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // UnitDatas가 null이거나 초기화되지 않은 경우 처리
        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        // PrototypeUnitID가 100인 데이터만 필터링
        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas.FindAll(prisoner =>
        {
            if (prisoner == null)
            {
                Debug.LogWarning("UnitData 객체가 null입니다.");
                return false;
            }

            return prisoner.PrototypeUnitID == 100;
        });

        // 필터링된 데이터가 있는 경우 UI 생성
        if (filteredPrisoners.Count > 0)
        {
            foreach (var prisoner in filteredPrisoners)
            {
                CreatePrisonerUI(prisoner);
                Debug.Log($"PrototypeUnitID 100: {prisoner.Name}");
            }
        }
        else
        {
            Debug.LogWarning("PrototypeUnitID가 100인 죄수 데이터가 없습니다.");
        }

        // contentParent의 RectTransform 가져오기
        RectTransform contentRect = contentParent.GetComponent<RectTransform>();

        // GridLayoutGroup에서 셀 크기와 간격 값 확인
        GridLayoutGroup gridLayoutGroup = contentParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // 열 수를 1로 고정
        gridLayoutGroup.constraintCount = 1; // 열 수를 1로 설정
        float cellHeight = gridLayoutGroup.cellSize.y;  // 셀의 높이
        float spacingY = gridLayoutGroup.spacing.y;     // 세로 간격
        float paddingUp = gridLayoutGroup.padding.top;

        // 새로운 height 계산: 셀 높이 * 항목 수 + 간격
        float newHeight = (cellHeight + spacingY) * filteredPrisoners.Count - spacingY + paddingUp;

        // 새로운 height 값 반영
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        // 레이아웃 강제 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");
    }

    private void CreatePrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(prisonerInfoPrefab, contentParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = $"HP: {prisoner.HealthPoint}/{prisoner.MaxHealthPoint}";
        prisonerUI.transform.Find("StrengthText").GetComponent<TextMeshProUGUI>().text = $"STR: {prisoner.Strength}";
        prisonerUI.transform.Find("DefenseText").GetComponent<TextMeshProUGUI>().text = $"DEF: {prisoner.Defense}";
        prisonerUI.transform.Find("CrimeText").GetComponent<TextMeshProUGUI>().text = GetCrimeDescription(prisoner.Crime);

        Image headImage = prisonerUI.transform.Find("HeadImage").GetComponent<Image>();
        if (prisoner.HeadID >= 0 && prisoner.HeadID < headSprites.Length)
        {
            headImage.sprite = headSprites[prisoner.HeadID];
        }
        else
        {
            Debug.LogWarning($"Invalid HeadID: {prisoner.HeadID}");
        }

        // BodyID 이미지 설정
        Image bodyImage = prisonerUI.transform.Find("BodyImage").GetComponent<Image>();
        if (prisoner.BodyID >= 0 && prisoner.BodyID < bodySprites.Length)
        {
            bodyImage.sprite = bodySprites[prisoner.BodyID];
        }
        else
        {
            Debug.LogWarning($"Invalid BodyID: {prisoner.BodyID}");
        }

        Button prisonerButton = prisonerUI.transform.Find("FloorPrisonerButton").GetComponent<Button>();
        if (prisonerButton != null)
        {
            prisonerButton.onClick.AddListener(() =>
            {
                prisonerButton.onClick.AddListener(() =>
                {
                    Debug.Log("Prisoner button clicked!");
                    floorUIManager.openFloorPrisonerInfoUI(prisoner);  // 해당 죄수 정보를 UI에 업데이트
                });
            });
        }
        else
        {
            Debug.LogWarning("Prisoner button is missing!");
        }
    }

    private string GetCrimeDescription(int crimeId)
    {
        switch (crimeId)
        {
            case 0: return "자만령";
            case 1: return "질투령";
            case 2: return "혈마";
            case 3: return "패륜령";
            case 4: return "반역마";
            case 5: return "포식귀";
            case 6: return "탐닉귀";
            default: return "알 수 없음";
        }
    }

    public void UpdatePrisonerInfoUI(UnitData prisoner)
    {
        // "Floor Prisoner Info" UI를 업데이트
        GameObject prisonerInfoUI = floorUIManager.GetPrisonerInfoUI();  // GetPrisonerInfoUI()로 UI 가져오기

        if (prisonerInfoUI != null)
        {
            prisonerInfoUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
            prisonerInfoUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
            prisonerInfoUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = $"HP: {prisoner.HealthPoint}/{prisoner.MaxHealthPoint}";
            prisonerInfoUI.transform.Find("StrengthText").GetComponent<TextMeshProUGUI>().text = $"STR: {prisoner.Strength}";
            prisonerInfoUI.transform.Find("DefenseText").GetComponent<TextMeshProUGUI>().text = $"DEF: {prisoner.Defense}";
            prisonerInfoUI.transform.Find("CrimeText").GetComponent<TextMeshProUGUI>().text = GetCrimeDescription(prisoner.Crime);
            prisonerInfoUI.transform.Find("HandicraftText").GetComponent<TextMeshProUGUI>().text = $"HCR: {prisoner.Handicraft}";
            prisonerInfoUI.transform.Find("DeathErosionText").GetComponent<TextMeshProUGUI>().text = $"DES: {prisoner.DeathErosion}/100";

            Image headImage = prisonerInfoUI.transform.Find("HeadImage").GetComponent<Image>();
            if (prisoner.HeadID >= 0 && prisoner.HeadID < headSprites.Length)
            {
                headImage.sprite = headSprites[prisoner.HeadID];
            }

            Image bodyImage = prisonerInfoUI.transform.Find("BodyImage").GetComponent<Image>();
            if (prisoner.BodyID >= 0 && prisoner.BodyID < bodySprites.Length)
            {
                bodyImage.sprite = bodySprites[prisoner.BodyID];
            }

            Slider levelSlider = prisonerInfoUI.transform.Find("LevelSlider").GetComponent<Slider>();
            if (levelSlider != null)
            {
                levelSlider.maxValue = 100;
                levelSlider.value = prisoner.EXP;
            }

            Slider healthSlider = prisonerInfoUI.transform.Find("HealthSlider").GetComponent<Slider>();
            if (levelSlider != null)
            {
                healthSlider.maxValue = prisoner.MaxHealthPoint;
                healthSlider.value = prisoner.HealthPoint;
            }

            Slider deathErosionSlider = prisonerInfoUI.transform.Find("DeathErosionSlider").GetComponent<Slider>();
            if (deathErosionSlider != null)
            {
                deathErosionSlider.maxValue = 100;
                deathErosionSlider.value = prisoner.DeathErosion;
            }
            else
            {
                Debug.LogWarning("DeathErosionSlider를 찾을 수 없습니다!");
            }
        }
    }

}
