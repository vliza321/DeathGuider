using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FloorSystem : MonoBehaviour
{
    public FloorUIManager floorUIManager;
    public BusRandomPrisoner busRandomPrisoner;
    public MoveCamera moveCamera;
    public StorageUI storageUI;

    public GameObject floorPrefab;
    public GameObject upgradePage;
    public Transform parentTransform;
    public Button upgradeButton;
    private List<GameObject> floors = new List<GameObject>();

    public GameObject prisonerInfoPrefab;
    public Transform contentParent;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    [SerializeField]
    private DontDestroyObjectManager DDOManager;
    public FloorUpgradeCost upgradeCostData;

    [System.Serializable]
    public struct FloorUpgradeCost
    {
        public int CurrentFloor;
        public int BaseCost;
        public float CostIncreaseRate;
        public float DiscountRate;
        public float calculatedCost;
    }

    void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            Debug.Log(ddo.scene.name);
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
        }
        DDO = null;

        if (DDOManager != null)
        {
            float initialCost = 100 * Mathf.Pow(1.1f, DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor - 1);
            initialCost *= (1 - 0.1f * DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance);

            upgradeCostData = new FloorUpgradeCost
            {
                CurrentFloor = DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor,
                BaseCost = 100,
                CostIncreaseRate = 2.0f,
                DiscountRate = 0.1f * DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance,
                calculatedCost = (int)initialCost
            };
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
        if (upgradePage != null)
        {
            upgradePage.SetActive(true);
        }

        if (upgradeButton != null)
        {
            upgradeButton.gameObject.SetActive(false);
        }

        CalculateUpgradeCosts();

        Button UpgradeButton = upgradePage.transform.Find("UpgradeButton").GetComponent<Button>();
        if (UpgradeButton != null)
        {
            UpgradeButton.onClick.AddListener(FloorUpgrade);
        }
    }

    private void CalculateUpgradeCosts()
    {
        upgradeCostData.calculatedCost = Mathf.FloorToInt(upgradeCostData.BaseCost * (1 - upgradeCostData.DiscountRate));

        UpdateUpgradePageText();
    }

    public void FloorUpgrade()
    {
        if (floors.Count > 0)
        {
            float discountMultiplier = DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance * upgradeCostData.DiscountRate;
            float upgradeCost = CalculateUpgradeCostWithDiscount(discountMultiplier);

            if (DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold >= upgradeCost)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= (int)upgradeCost;

                GameObject topFloor = floors[floors.Count - 1];
                Vector2 newPosition = new Vector2(topFloor.transform.position.x, topFloor.transform.position.y + 192);
                CreateFloor(newPosition);

                upgradeCostData.CurrentFloor++;
                DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor++;

                storageUI.UpdateGold();

                busRandomPrisoner.UpdateUI();
                UpdateUpgradeCostData();
            }
            else
            {
                Debug.LogWarning("골드가 부족합니다!");
            }
        }
    }

    private float CalculateUpgradeCostWithDiscount(float discountMultiplier)
    {
        return upgradeCostData.BaseCost + (upgradeCostData.CurrentFloor - 1) * upgradeCostData.CostIncreaseRate - (DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance * discountMultiplier);
    }

    private void UpdateUpgradeCostData()
    {
        float newCalculatedCost = upgradeCostData.BaseCost * Mathf.Pow(upgradeCostData.CostIncreaseRate, upgradeCostData.CurrentFloor - 1);
        newCalculatedCost *= (1 - upgradeCostData.DiscountRate);
        upgradeCostData.calculatedCost = newCalculatedCost;

        UpdateUpgradePageText();
    }

    private void UpdateUpgradePageText()
    {
        UpdateText("GoldText", $"골드: {(int)upgradeCostData.calculatedCost}G");
        UpdateText("DiscountText", $"할인율: {upgradeCostData.DiscountRate * 100}%");
    }

    private void UpdateText(string textName, string textValue)
    {
        TextMeshProUGUI text = upgradePage.transform.Find(textName)?.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = textValue;
        }
        else
        {
            Debug.LogWarning($"{textName} not found in upgradePage!");
        }
    }

    public void CloseUpgradePage()
    {
        if (upgradePage != null)
        {
            upgradePage.SetActive(false);
        }

        if (upgradeButton != null)
        {
            upgradeButton.gameObject.SetActive(true);
        }
    }

    private void CreateFloor(Vector2 position)
    {
        moveCamera.UpdateMinY();

        //if (!DDOManager.SaveData())
        //{
        //    Debug.Log("Fail Save Data");
        //}

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

        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas.FindAll(prisoner =>
        {
            if (prisoner == null)
            {
                Debug.LogWarning("UnitData 객체가 null입니다.");
                return false;
            }

            return prisoner.PrototypeUnitID == 100;
        });

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

        RectTransform contentRect = contentParent.GetComponent<RectTransform>();

        GridLayoutGroup gridLayoutGroup = contentParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;
        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * filteredPrisoners.Count - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

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
                Debug.Log("Prisoner button clicked!");
                floorUIManager.openFloorPrisonerInfoUI(prisoner);
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
        GameObject prisonerInfoUI = floorUIManager.GetPrisonerInfoUI();

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
