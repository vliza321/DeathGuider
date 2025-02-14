using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LaboratorySystem : MonoBehaviour
{
    public FloorSystem floorSystem;
    public HealthSystem healthSystem;
    public ErosionSystem erosionSystem;
    public SmithSystem smithSystem;
    public StorageUI storageUI;
    public GameObject parentObject;
    public BusRandomPrisoner busRandomPrisoner;

    public GameObject UpgradePage;
    public TextMeshProUGUI DescriptionText1;
    public TextMeshProUGUI DescriptionText2;
    public TextMeshProUGUI DescriptionText3;
    public TextMeshProUGUI DescriptionText4;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI DarkText;

    [System.Serializable]
    public class LabButtonPrice
    {
        public Button labButton;
        public Slider labLevelSlider;
        public int upgradeCount = 0;
        public int baseGoldPrice;
        public int baseDarkPrice;
        public float goldMultiplier = 1.2f;
        public float darkMultiplier = 1.5f;
        public int calculatedGoldPrice;
        public int calculatedDarkPrice;
    }

    public List<LabButtonPrice> labButtonPrices = new List<LabButtonPrice>();

    private DontDestroyObjectManager DDOManager;

    void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
<<<<<<< Updated upstream
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
=======
            if (ddo.name == "DDOManager" && SceneManager.GetActiveScene() == ddo.scene)
>>>>>>> Stashed changes
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
        }
        DDO = null;

        Button[] buttons = parentObject.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            List<LabButtonPrice> buttonPrices = null;

            if (button.name == "LabBusButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if (button.name == "LabFloorButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabGYMButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabHealthButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabErosionButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabSmithButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabEfficiencyButton")
            {
                buttonPrices = labButtonPrices;
            }
            else if(button.name == "LabRewardButton")
            {
                buttonPrices = labButtonPrices;
            }

            if (buttonPrices != null)
            {
                LabButtonPrice buttonPrice = buttonPrices.Find(bp => bp.labButton == button);
                if (buttonPrice != null)
                {
                    int index = buttonPrices.IndexOf(buttonPrice);
                    button.onClick.AddListener(() => ActivateUpgradePage(index));
                }
            }
        }

        foreach (var labButtonPrice in labButtonPrices)
        {
            if (labButtonPrice.labLevelSlider != null)
            {
                labButtonPrice.labLevelSlider.maxValue = 5;
                labButtonPrice.labLevelSlider.value = Mathf.Min(DDOManager.LocalUserDatas.LocalUserDataDic[0].BusEnhance, 5);
            }
        }
    }

    void DeactivateAllButtons()
    {
        foreach (var labButtonPrice in labButtonPrices)
        {
            labButtonPrice.labButton.interactable = false;
        }

        Button[] buttons = parentObject.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.name != "Easter Egg")
            {
                button.interactable = false;
            }
        }
    }

    void ActivateUpgradePage(int index)
    {
        Debug.Log("ActivateUpgradePage called with index: " + index);
        DeactivateAllButtons();

        LabButtonPrice selectedButtonPrice = labButtonPrices[index];

        if (index == 0)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "날짜의 리셋 시간 기준, 이감 대기 유닛의 최대 수치 증가.";
            }
            if (DescriptionText2 != null) DescriptionText2.gameObject.SetActive(false);
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if (index == 1)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "수용소 증축 비용 감소";
            }
            if (DescriptionText2 != null) DescriptionText2.gameObject.SetActive(false);
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if (index == 2)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "스탯 증가량 비율 증가";
            }
            if (DescriptionText2 != null) DescriptionText2.gameObject.SetActive(false);
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if (index == 3)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "체력 회복 비율 증가";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(true);
                DescriptionText2.text = "최대 수용 인원 증가";
            }
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if(index == 4)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "죽음 침식도 회복 비율 증가";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(true);
                DescriptionText2.text = "최대 수용 인원 증가";
            }
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if(index == 5)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "무기 재련 / 내구도 수리 / 무기 진화 비용 감소";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(true);
                DescriptionText2.text = "내구도 회복 비율 증가";
            }
            if (DescriptionText3 != null)
            {
                DescriptionText3.gameObject.SetActive(true);
                DescriptionText3.text = "무기 구매 대상 무기 등급 증가";
            }
            if (DescriptionText4 != null)
            {
                DescriptionText4.gameObject.SetActive(true);
                DescriptionText4.text = "무기 구매 대상 무기 가격 감소 및 무기 판매 대상 무기 가격 증가";
            }
        }
        else if(index == 6)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "전투 효율 관련";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(true);
                DescriptionText2.text = "전투 효율 관련2";
            }
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }
        else if (index == 7)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.gameObject.SetActive(true);
                DescriptionText1.text = "전투를 통해 획득하는 재화(골드 및 어둠 정수)의 증가";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(true);
                DescriptionText2.text = "전투 종료로 얻는 탐사 진척도 증가";
            }
            if (DescriptionText3 != null) DescriptionText3.gameObject.SetActive(false);
            if (DescriptionText4 != null) DescriptionText4.gameObject.SetActive(false);
        }


        if (selectedButtonPrice.upgradeCount >= 5)
        {
            selectedButtonPrice.labButton.interactable = false;
            Debug.Log("최대 업그레이드 횟수에 도달했습니다. 버튼 비활성화.");
        }
        else
        {
            selectedButtonPrice.calculatedGoldPrice = Mathf.FloorToInt(selectedButtonPrice.baseGoldPrice + selectedButtonPrice.upgradeCount * selectedButtonPrice.goldMultiplier);
            selectedButtonPrice.calculatedDarkPrice = Mathf.FloorToInt(selectedButtonPrice.baseDarkPrice + selectedButtonPrice.upgradeCount * selectedButtonPrice.darkMultiplier);

            UpdateUpgradePageText(selectedButtonPrice);
        }

        if (UpgradePage != null)
        {
            UpgradePage.SetActive(true);
            Debug.Log("UpgradePage 활성화됨");

            Button[] buttons = UpgradePage.GetComponentsInChildren<Button>();
            Button upgradeButton = null;

            foreach (Button button in buttons)
            {
                if (button.name == "UpgradeButton")
                {
                    upgradeButton = button;
                    upgradeButton.onClick.RemoveAllListeners();
                    break;
                }
            }

            if (upgradeButton != null)
            {
                int currentIndex = index;
                if (selectedButtonPrice.upgradeCount >= 5)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }

                upgradeButton.onClick.AddListener(() => OnLabUpgradeButtonClicked(currentIndex));
            }
        }
        else
        {
            Debug.LogWarning("UpgradePage UI가 할당되지 않았습니다.");
        }
    }

    void OnLabUpgradeButtonClicked(int index)
    {
        LabButtonPrice selectedButtonPrice = labButtonPrices[index];

        if (DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold >= selectedButtonPrice.calculatedGoldPrice &&
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence >= selectedButtonPrice.calculatedDarkPrice)
        { 
            DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= selectedButtonPrice.calculatedGoldPrice;
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= selectedButtonPrice.calculatedDarkPrice;

            selectedButtonPrice.calculatedGoldPrice = Mathf.FloorToInt(selectedButtonPrice.baseGoldPrice * Mathf.Pow(selectedButtonPrice.goldMultiplier, selectedButtonPrice.upgradeCount + 1));
            selectedButtonPrice.calculatedDarkPrice = Mathf.FloorToInt(selectedButtonPrice.baseDarkPrice * Mathf.Pow(selectedButtonPrice.darkMultiplier, selectedButtonPrice.upgradeCount + 1));

            selectedButtonPrice.baseGoldPrice = selectedButtonPrice.calculatedGoldPrice;
            selectedButtonPrice.baseDarkPrice = selectedButtonPrice.calculatedDarkPrice;

            selectedButtonPrice.upgradeCount++;

            if (index == 0 && selectedButtonPrice.upgradeCount <= 5)
            {
                busRandomPrisoner.availablePrisoner++;
                DDOManager.LocalUserDatas.LocalUserDataDic[0].BusEnhance++;
            }
            else if (index == 1 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance++;
                floorSystem.upgradeCostData.DiscountRate = 0.1f * DDOManager.LocalUserDatas.LocalUserDataDic[0].PrisonEnhance;
            }
            else if(index == 2 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].GYMEnhance++;
            }
            else if(index == 3 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].HealthEnhance++;
                healthSystem.healthRoomCount++;

                while (healthSystem.HealthDataList.Count < healthSystem.healthRoomCount)
                {
                    healthSystem.HealthDataList.Add(new HealthSystem.HealthData());
                }
            }
            else if (index == 4 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].ErosionEnhance++;
                erosionSystem.ErosionRoomCount++;

                while (erosionSystem.ErosionDataList.Count < erosionSystem.ErosionRoomCount)
                {
                    erosionSystem.ErosionDataList.Add(new ErosionSystem.ErosionData());
                }
            }
            else if (index == 5 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance++;
                smithSystem.UpdateSmithEnhanceAndUI();
                smithSystem.currentWeaponRank++;
            }
            else if (index == 6 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].BattleEfficiency++;
            }
            else if(index == 7 && selectedButtonPrice.upgradeCount <= 5)
            {
                DDOManager.LocalUserDatas.LocalUserDataDic[0].BattleReward++;
            }

            storageUI.UpdateGold();
            storageUI.UpdatedarkEssence();
            selectedButtonPrice.labLevelSlider.value = selectedButtonPrice.upgradeCount;

            if (labButtonPrices[index].upgradeCount >= 5)
            {
                labButtonPrices[index].labButton.interactable = false;

                Button[] buttons = UpgradePage.GetComponentsInChildren<Button>();
                foreach (Button button in buttons)
                {
                    if (button.name == "UpgradeButton")
                    {
                        button.interactable = false;
                        break;
                    }
                }
            }
            UpdateUpgradePageText(selectedButtonPrice);
        }
        else
        {
            Debug.Log("업그레이드에 필요한 자원이 부족합니다.");
        }
    }

    public void CloseUpgradePage()
    {
        if (UpgradePage != null)
        {
            UpgradePage.SetActive(false);
            Debug.Log("UpgradePage 비활성화됨");
        }

        foreach (var labButtonPrice in labButtonPrices)
        {
            labButtonPrice.labButton.interactable = true;
        }

        Button[] buttons = parentObject.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }


    void UpdateUpgradePageText(LabButtonPrice selectedButtonPrice)
    {
        if (GoldText != null)
        {
            int priceToDisplay = selectedButtonPrice.upgradeCount == 0 ? selectedButtonPrice.baseGoldPrice : selectedButtonPrice.calculatedGoldPrice;
            GoldText.text = $"{priceToDisplay} G";
        }

        if (DarkText != null)
        {
            int priceToDisplay = selectedButtonPrice.upgradeCount == 0 ? selectedButtonPrice.baseDarkPrice : selectedButtonPrice.calculatedDarkPrice;
            DarkText.text = $"{priceToDisplay} K";
        }
    }
}
