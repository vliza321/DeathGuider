using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaboratorySystem : MonoBehaviour
{
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

    public List<LabButtonPrice> labBusButtonPrices = new List<LabButtonPrice>();

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
        }

        Button[] buttons = parentObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == "LabBusButton")
            {
                LabButtonPrice buttonPrice = labBusButtonPrices.Find(bp => bp.labButton == button);
                if (buttonPrice != null)
                {
                    int index = labBusButtonPrices.IndexOf(buttonPrice);
                    button.onClick.AddListener(() => ActivateUpgradePage(index));
                }
            }
        }

        foreach (var labButtonPrice in labBusButtonPrices)
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
        foreach (var labButtonPrice in labBusButtonPrices)
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
        DeactivateAllButtons();

        if (index == 0)
        {
            if (DescriptionText1 != null)
            {
                DescriptionText1.text = "날짜의 리셋 시간 기준, 이감 대기 유닛의 최대 수치 증가.";
            }
            if (DescriptionText2 != null)
            {
                DescriptionText2.gameObject.SetActive(false);
            }
            if (DescriptionText3 != null)
            {
                DescriptionText3.gameObject.SetActive(false);
            }
            if (DescriptionText4 != null)
            {
                DescriptionText4.gameObject.SetActive(false);
            }
        }

        LabButtonPrice selectedButtonPrice = labBusButtonPrices[index];

        if (selectedButtonPrice.upgradeCount >= 5)
        {
            selectedButtonPrice.labButton.interactable = false;
            Debug.Log("최대 업그레이드 횟수에 도달했습니다. 버튼 비활성화.");
        }
        else
        {
            if (selectedButtonPrice.upgradeCount > 0)
            {
                selectedButtonPrice.calculatedGoldPrice = selectedButtonPrice.baseGoldPrice;
                selectedButtonPrice.calculatedDarkPrice = selectedButtonPrice.baseDarkPrice;
            }
            else
            {
                selectedButtonPrice.calculatedGoldPrice = Mathf.FloorToInt(selectedButtonPrice.baseGoldPrice + selectedButtonPrice.upgradeCount * selectedButtonPrice.goldMultiplier);
                selectedButtonPrice.calculatedDarkPrice = Mathf.FloorToInt(selectedButtonPrice.baseDarkPrice + selectedButtonPrice.upgradeCount * selectedButtonPrice.darkMultiplier);
            }

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
                    break;
                }
            }

            if (upgradeButton != null)
            {
                int currentIndex = index;
                upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked(currentIndex));
            }
        }
        else
        {
            Debug.LogWarning("UpgradePage UI가 할당되지 않았습니다.");
        }
    }

    public void CloseUpgradePage()
    {
        if (UpgradePage != null)
        {
            UpgradePage.SetActive(false);
            Debug.Log("UpgradePage 비활성화됨");
        }

        foreach (var labButtonPrice in labBusButtonPrices)
        {
            labButtonPrice.labButton.interactable = true;
        }

        Button[] buttons = parentObject.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    void OnUpgradeButtonClicked(int index)
    {

        LabButtonPrice selectedButtonPrice = labBusButtonPrices[index];

        if (DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold >= selectedButtonPrice.calculatedGoldPrice &&
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence >= selectedButtonPrice.calculatedDarkPrice)
        {
            DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= selectedButtonPrice.calculatedGoldPrice;
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= selectedButtonPrice.calculatedDarkPrice;

            selectedButtonPrice.upgradeCount++;

            selectedButtonPrice.calculatedGoldPrice = Mathf.FloorToInt(selectedButtonPrice.baseGoldPrice * Mathf.Pow(selectedButtonPrice.goldMultiplier, selectedButtonPrice.upgradeCount));
            selectedButtonPrice.calculatedDarkPrice = Mathf.FloorToInt(selectedButtonPrice.baseDarkPrice * Mathf.Pow(selectedButtonPrice.darkMultiplier, selectedButtonPrice.upgradeCount));
            selectedButtonPrice.baseGoldPrice = selectedButtonPrice.calculatedGoldPrice;
            selectedButtonPrice.baseDarkPrice = selectedButtonPrice.calculatedDarkPrice;

            busRandomPrisoner.availablePrisoner++;
            storageUI.UpdateGold();
            storageUI.UpdatedarkEssence();
            DDOManager.LocalUserDatas.LocalUserDataDic[0].BusEnhance++;
            selectedButtonPrice.labLevelSlider.value = selectedButtonPrice.upgradeCount;

            if (selectedButtonPrice.upgradeCount >= 5)
            {
                selectedButtonPrice.labButton.interactable = false;

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
