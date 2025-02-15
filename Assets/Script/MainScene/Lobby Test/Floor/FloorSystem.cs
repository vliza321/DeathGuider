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

    [SerializeField]
    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
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
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }

            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.GetComponent<GameManager>();
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

        GameObject firstFloor = GameObject.Find("Floor 1");
        if (firstFloor != null)
        {
            floors.Add(firstFloor);
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
            float discountMultiplier = upgradeCostData.DiscountRate;
            float upgradeCost = CalculateUpgradeCostWithDiscount(discountMultiplier);

            if (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Gold >= upgradeCost)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Gold -= (int)upgradeCost;

                GameObject topFloor = floors[floors.Count - 1];
                Vector2 newPosition = new Vector2(topFloor.transform.position.x, topFloor.transform.position.y + 192);
                CreateFloor(newPosition);

                upgradeCostData.CurrentFloor++;
                DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Floor++;

                storageUI.UpdateGold();

                busRandomPrisoner.UpdateUI();
                UpdateUpgradeCostData();
            }
        }
    }

    private float CalculateUpgradeCostWithDiscount(float discountMultiplier)
    {
        float costBeforeDiscount = upgradeCostData.BaseCost
                                 * Mathf.Pow(upgradeCostData.CostIncreaseRate, upgradeCostData.CurrentFloor - 1);
        return costBeforeDiscount * (1 - discountMultiplier);
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
        UpdateText("GoldText", $"°ñµå: {(int)upgradeCostData.calculatedCost}G");
        UpdateText("DiscountText", $"ÇÒÀÎÀ²: {upgradeCostData.DiscountRate * 100}%");
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

        int currentFloor = DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Floor;

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
                    floorUIManager.OpenFloorPrisonerUI();
                });
            }
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
            return;
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas.FindAll(prisoner =>
        {
            if (prisoner == null)
            {
                return false;
            }

            return prisoner.PrototypeUnitID == 100;
        });

        if (filteredPrisoners.Count > 0)
        {
            foreach (var prisoner in filteredPrisoners)
            {
                CreatePrisonerUI(prisoner);
            }
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
        if (prisoner.HeadID >= 0 && prisoner.HeadID < GameManager.PrisonerHeadImg.Count)
        {
            headImage.sprite = GameManager.PrisonerHeadImg[prisoner.HeadID];
        }

        Image bodyImage = prisonerUI.transform.Find("BodyImage").GetComponent<Image>();
        if (prisoner.BodyID >= 0 && prisoner.BodyID < GameManager.PrisonerBodyImg.Count)
        {
            bodyImage.sprite = GameManager.PrisonerBodyImg[prisoner.BodyID];
        }

        Button prisonerButton = prisonerUI.transform.Find("FloorPrisonerButton").GetComponent<Button>();
        if (prisonerButton != null)
        {
            prisonerButton.onClick.AddListener(() =>
            {
                floorUIManager.openFloorPrisonerInfoUI(prisoner);
            });
        }
    }

    private string GetCrimeDescription(int crimeId)
    {
        switch (crimeId)
        {
            case 0: return "ÀÚ¸¸·É";
            case 1: return "ÁúÅõ·É";
            case 2: return "Ç÷¸¶";
            case 3: return "ÆÐ·û·É";
            case 4: return "¹Ý¿ª¸¶";
            case 5: return "Æ÷½Ä±Í";
            case 6: return "Å½´Ð±Í";
            default: return "¾Ë ¼ö ¾øÀ½";
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
            if (prisoner.HeadID >= 0 && prisoner.HeadID < GameManager.PrisonerHeadImg.Count)
            {
                headImage.sprite = GameManager.PrisonerHeadImg[prisoner.HeadID];
            }

            Image bodyImage = prisonerInfoUI.transform.Find("BodyImage").GetComponent<Image>();
            if (prisoner.BodyID >= 0 && prisoner.BodyID < GameManager.PrisonerBodyImg.Count)
            {
                bodyImage.sprite = GameManager.PrisonerBodyImg[prisoner.BodyID];
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
        }
    }
}
