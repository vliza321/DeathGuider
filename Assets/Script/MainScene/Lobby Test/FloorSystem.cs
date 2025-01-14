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
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (DDOManager.UnitDatas.UnitDatas.Count > 0)
        {
            foreach (var prisoner in DDOManager.UnitDatas.UnitDatas)
            {
                CreatePrisonerUI(prisoner);
                Debug.Log("생성 완료");
            }

        }
        else
        {
            Debug.LogWarning("보유한 죄수 데이터가 없습니다.");
        }

        float newHeight = CalculateNewHeight();  // 이 값을 실제로 계산하는 코드 필요

        // contentParent의 RectTransform 가져오기
        RectTransform contentRect = contentParent.GetComponent<RectTransform>();

        // 레이아웃 그룹 비활성화 (필요시)
        var layoutGroup = contentParent.GetComponent<VerticalLayoutGroup>();
        bool layoutGroupActive = layoutGroup != null && layoutGroup.enabled;
        if (layoutGroupActive) layoutGroup.enabled = false;

        // 높이 갱신
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.rect.height + newHeight);

        // 레이아웃 강제 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        // 레이아웃 그룹을 다시 활성화 (필요시)
        if (layoutGroupActive) layoutGroup.enabled = true;
    }

    private float CalculateNewHeight()
    {
        // 자식들의 총 높이나 다른 방식으로 계산
        return 200f;  // 예시로 200을 더한다고 가정
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
}
