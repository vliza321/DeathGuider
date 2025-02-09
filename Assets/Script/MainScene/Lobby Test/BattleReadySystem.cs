using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class BattleReadySystem : MonoBehaviour
{
    public GameObject BattleReadyPrisonerPrefab;
    public GameObject BattleReadyWeaponPrefab;

    public Transform BattleReadyPrisonerParent;
    public Transform BattleReadyWeaponParent;



    private DontDestroyObjectManager DDOManager;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    public GameObject chooseManager;
    public GameObject[] battleReadyPrisonerUI = new GameObject[4];

    private UnitData selectedManagerUnit;

    private UnitData[] battleReadyPrisoners = new UnitData[4];

    private void Start()
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
    }

    public void DisplayBattleReadyUnits()
    {
        foreach (Transform child in BattleReadyPrisonerParent)
        {
            Destroy(child.gameObject);
        }

        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            return;
        }

        var allUnits = DDOManager.UnitDatas.UnitDatas.FindAll(unit =>
        {
            if (unit == null)
            {
                return false;
            }

            return true;
        });

        if (allUnits.Count > 0)
        {
            foreach (var unit in allUnits)
            {
                CreateBattleReadyUnitUI(unit);
            }
        }
        AdjustUnitGridLayoutSize(allUnits.Count);
    }

    private void CreateBattleReadyUnitUI(UnitData unit)
    {
        if (selectedManagerUnit != null && selectedManagerUnit == unit)
        {
            return;
        }

        if (battleReadyPrisoners.Contains(unit))
        {
            return;
        }

        GameObject unitUI = Instantiate(BattleReadyPrisonerPrefab, BattleReadyPrisonerParent);

        unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = unit.Name;
        unitUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "Lv: " + unit.Level;
        unitUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "HP: " + unit.HealthPoint + " / " + unit.MaxHealthPoint;
        unitUI.transform.Find("StrengthText").GetComponent<TextMeshProUGUI>().text = "STR: " + unit.Strength;
        unitUI.transform.Find("DefenseText").GetComponent<TextMeshProUGUI>().text = "DEF: " + unit.Defense;
        unitUI.transform.Find("CrimeText").GetComponent<TextMeshProUGUI>().text = "Crime: " + GetCrimeDescription(unit.Crime);

        if (unit.HeadID >= 0 && unit.HeadID < headSprites.Length)
        {
            unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = headSprites[unit.HeadID];
        }

        if (unit.BodyID >= 0 && unit.BodyID < bodySprites.Length)
        {
            unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = bodySprites[unit.BodyID];
        }

        Button chooseButton = unitUI.transform.Find("ChooseButton").GetComponent<Button>();
        if (chooseButton != null)
        {
            chooseButton.onClick.AddListener(() => ChoosePrisoner(unit));
        }
    }

    private void ChoosePrisoner(UnitData unit)
    {
        if (selectedManagerUnit != null && selectedManagerUnit == unit)
        {
            return;
        }

        if (unit.PrototypeUnitID != 100)
        {
            if (selectedManagerUnit != null)
            {
                return;
            }

            selectedManagerUnit = unit;

            UnitParticipateData newData = new UnitParticipateData
            {
                UserID = unit.UserID,
                PrototypeUnitID = unit.PrototypeUnitID,
                InstanceID = unit.InstanceID,
                PartyID = 0,
                Position = 0
            };

            DDOManager.UnitParticipateDatas.UnitParticipateDatas.Add(newData);
            DDOManager.UnitParticipateDatas.UnitParticipateDataDic[(newData.UserID, newData.PrototypeUnitID, newData.InstanceID, newData.PartyID)] = newData;


            if (chooseManager != null)
            {
                TextMeshProUGUI managerNameText = chooseManager.transform.Find("ManagerNameText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerLevelText = chooseManager.transform.Find("ManagerLevelText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerHealthText = chooseManager.transform.Find("ManagerHealthText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerStrengthText = chooseManager.transform.Find("ManagerStrengthText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerCrimeText = chooseManager.transform.Find("ManagerCrimeText").GetComponent<TextMeshProUGUI>();

                Image bodyImage = chooseManager.transform.Find("BodyImage").GetComponent<Image>();
                Image headImage = chooseManager.transform.Find("HeadImage").GetComponent<Image>();

                if (managerNameText != null)
                    managerNameText.text = unit.Name;

                if (managerLevelText != null)
                    managerLevelText.text = "Lv: " + unit.Level;

                if (managerHealthText != null)
                    managerHealthText.text = "HP: " + unit.HealthPoint + " / " + unit.MaxHealthPoint;

                if (managerStrengthText != null)
                    managerStrengthText.text = "STR: " + unit.Strength;

                if (managerCrimeText != null)
                    managerCrimeText.text = "Crime: " + GetCrimeDescription(unit.Crime);

                if (unit.BodyID >= 0 && unit.BodyID < bodySprites.Length && bodyImage != null)
                {
                    bodyImage.sprite = bodySprites[unit.BodyID];
                }

                if (unit.HeadID >= 0 && unit.HeadID < headSprites.Length && headImage != null)
                {
                    headImage.sprite = headSprites[unit.HeadID];
                }

                Button closeButton = chooseManager.transform.Find("CloseManagerButton").GetComponent<Button>();
                if (closeButton != null)
                {
                    closeButton.onClick.AddListener(() => returnPrisoner(unit));
                }

            }
        }
        else if (unit.PrototypeUnitID == 100)
        {
            for (int i = 0; i < battleReadyPrisoners.Length; i++)
            {
                if (battleReadyPrisoners[i] == null)
                {
                    battleReadyPrisoners[i] = unit;
                    Debug.Log($"죄수 {unit.Name} 추가됨");

                    UnitParticipateData newData = new UnitParticipateData
                    {
                        UserID = unit.UserID,
                        PrototypeUnitID = unit.PrototypeUnitID,
                        InstanceID = unit.InstanceID,
                        PartyID = 0,
                        Position = i+1
                    };

                    DDOManager.UnitParticipateDatas.UnitParticipateDatas.Add(newData);
                    DDOManager.UnitParticipateDatas.UnitParticipateDataDic[(newData.UserID, newData.PrototypeUnitID, newData.InstanceID, newData.PartyID)] = newData;

                    UpdateBattleReadyUI(unit);
                    DisplayBattleReadyUnits();
                    return;
                }
            }
        }
        DisplayBattleReadyUnits();;
    }

    private void UpdateBattleReadyUI(UnitData unit)
    {
        for (int i = 0; i < battleReadyPrisoners.Length; i++)
        {
            if (battleReadyPrisoners[i] != null)
            {
                GameObject prisonerUI = battleReadyPrisonerUI[i];
                if (prisonerUI != null)
                {
                    TextMeshProUGUI prisonerNameText = prisonerUI.transform.Find("PrisonerNameText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerLevelText = prisonerUI.transform.Find("PrisonerLevelText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerHealthText = prisonerUI.transform.Find("PrisonerHealthText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerStrengthText = prisonerUI.transform.Find("PrisonerStrengthText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerCrimeText = prisonerUI.transform.Find("PrisonerCrimeText").GetComponent<TextMeshProUGUI>();

                    Image prisonerBodyImage = prisonerUI.transform.Find("BodyImage").GetComponent<Image>();
                    Image prisonerHeadImage = prisonerUI.transform.Find("HeadImage").GetComponent<Image>();

                    if (prisonerNameText != null)
                        prisonerNameText.text = battleReadyPrisoners[i].Name;

                    if (prisonerLevelText != null)
                        prisonerLevelText.text = "Lv: " + battleReadyPrisoners[i].Level;

                    if (prisonerHealthText != null)
                        prisonerHealthText.text = "HP: " + battleReadyPrisoners[i].HealthPoint + " / " + battleReadyPrisoners[i].MaxHealthPoint;

                    if (prisonerStrengthText != null)
                        prisonerStrengthText.text = "STR: " + battleReadyPrisoners[i].Strength;

                    if (prisonerCrimeText != null)
                        prisonerCrimeText.text = "Crime: " + GetCrimeDescription(battleReadyPrisoners[i].Crime);

                    if (prisonerBodyImage != null && battleReadyPrisoners[i].BodyID >= 0 && battleReadyPrisoners[i].BodyID < bodySprites.Length)
                        prisonerBodyImage.sprite = bodySprites[battleReadyPrisoners[i].BodyID];

                    if (prisonerHeadImage != null && battleReadyPrisoners[i].HeadID >= 0 && battleReadyPrisoners[i].HeadID < headSprites.Length)
                        prisonerHeadImage.sprite = headSprites[battleReadyPrisoners[i].HeadID];

                    Button closeButton = prisonerUI.transform.Find("PrisonerCloseButton").GetComponent<Button>();
                    if (closeButton != null)
                    {
                        int index = i;
                        closeButton.onClick.RemoveAllListeners();
                        closeButton.onClick.AddListener(() =>
                        {
                            if (battleReadyPrisoners[index] != null)
                            {
                                returnPrisoner(battleReadyPrisoners[index]);
                            }
                        });
                    }
                }
            }
        }
    }

    private void returnPrisoner(UnitData unit)
    {
        if (unit.PrototypeUnitID != 100)
        {
            TextMeshProUGUI managerNameText = chooseManager.transform.Find("ManagerNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerLevelText = chooseManager.transform.Find("ManagerLevelText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerHealthText = chooseManager.transform.Find("ManagerHealthText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerStrengthText = chooseManager.transform.Find("ManagerStrengthText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerCrimeText = chooseManager.transform.Find("ManagerCrimeText").GetComponent<TextMeshProUGUI>();

            Image bodyImage = chooseManager.transform.Find("BodyImage").GetComponent<Image>();
            Image headImage = chooseManager.transform.Find("HeadImage").GetComponent<Image>();

            if (managerNameText != null) managerNameText.text = "이름";
            if (managerLevelText != null) managerLevelText.text = "레벨";
            if (managerHealthText != null) managerHealthText.text = "체력";
            if (managerStrengthText != null) managerStrengthText.text = "근력";
            if (managerCrimeText != null) managerCrimeText.text = "범죄";

            if (bodyImage != null && bodySprites.Length > 0) bodyImage.sprite = bodySprites[0];
            if (headImage != null && headSprites.Length > 0) headImage.sprite = headSprites[0];

            var key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID, 0);
            if (DDOManager.UnitParticipateDatas.UnitParticipateDataDic.ContainsKey(key))
            {
                DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Remove(key);
                DDOManager.UnitParticipateDatas.UnitParticipateDatas.RemoveAll(data =>
                    data.UserID == unit.UserID &&
                    data.PrototypeUnitID == unit.PrototypeUnitID &&
                    data.InstanceID == unit.InstanceID &&
                    data.PartyID == 0);
            }

            selectedManagerUnit = null;
        }
        else if (unit.PrototypeUnitID == 100)
        {
            for (int i = 0; i < battleReadyPrisonerUI.Length; i++)
            {
                if (battleReadyPrisoners[i] != null && battleReadyPrisoners[i].InstanceID == unit.InstanceID)
                {
                    battleReadyPrisoners[i] = null;

                    var key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID, 0);
                    if (DDOManager.UnitParticipateDatas.UnitParticipateDataDic.ContainsKey(key))
                    {
                        DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Remove(key);
                    }

                    DDOManager.UnitParticipateDatas.UnitParticipateDatas.RemoveAll(data =>
                        data.UserID == unit.UserID &&
                        data.PrototypeUnitID == unit.PrototypeUnitID &&
                        data.InstanceID == unit.InstanceID &&
                        data.PartyID == 0);

                    if (battleReadyPrisonerUI[i] != null)
                    {
                        TextMeshProUGUI prisonerNameText = battleReadyPrisonerUI[i].transform.Find("PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerLevelText = battleReadyPrisonerUI[i].transform.Find("PrisonerLevelText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerHealthText = battleReadyPrisonerUI[i].transform.Find("PrisonerHealthText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerStrengthText = battleReadyPrisonerUI[i].transform.Find("PrisonerStrengthText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerCrimeText = battleReadyPrisonerUI[i].transform.Find("PrisonerCrimeText")?.GetComponent<TextMeshProUGUI>();

                        Image prisonerBodyImage = battleReadyPrisonerUI[i].transform.Find("BodyImage")?.GetComponent<Image>();
                        Image prisonerHeadImage = battleReadyPrisonerUI[i].transform.Find("HeadImage")?.GetComponent<Image>();

                        if (prisonerNameText != null)
                            prisonerNameText.text = "이름";

                        if (prisonerLevelText != null)
                            prisonerLevelText.text = "레벨";

                        if (prisonerHealthText != null)
                            prisonerHealthText.text = "체력";

                        if (prisonerStrengthText != null)
                            prisonerStrengthText.text = "근력";

                        if (prisonerCrimeText != null)
                            prisonerCrimeText.text = "범죄";

                        if (prisonerBodyImage != null && bodySprites.Length > 0)
                            prisonerBodyImage.sprite = bodySprites[0];
                        if (prisonerHeadImage != null && headSprites.Length > 0)
                            prisonerHeadImage.sprite = headSprites[0];
                    }
                    break;
                }
            }
            DisplayBattleReadyUnits();
        }
        else
        {
            Debug.Log("오류");
        }

        DisplayBattleReadyUnits();
    }

    private void AdjustUnitGridLayoutSize(int unitCount)
    {
        RectTransform contentRect = BattleReadyPrisonerParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = BattleReadyPrisonerParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * unitCount - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    public void DisplayBattleReadyWeapons()
    {
        foreach (Transform child in BattleReadyWeaponParent)
        {
            Destroy(child.gameObject);
        }

        if (DDOManager.WeaponDatas == null || DDOManager.WeaponDatas.WeaponDatas == null)
        {
            return;
        }

        var allWeapons = DDOManager.WeaponDatas.WeaponDatas.FindAll(weapon =>
        {
            if (weapon == null)
            {
                return false;
            }

            return true;
        });

        if (allWeapons.Count > 0)
        {
            foreach (var weapon in allWeapons)
            {
                CreateBattleReadyWeaponUI(weapon);
            }
        }

        AdjustWeaponGridLayoutSize(allWeapons.Count);
    }

    private void CreateBattleReadyWeaponUI(WeaponData weapon)
    {
        GameObject weaponUI = Instantiate(BattleReadyWeaponPrefab, BattleReadyWeaponParent);

        weaponUI.transform.Find("WeaponNameText").GetComponent<TextMeshProUGUI>().text = weapon.Name;
        weaponUI.transform.Find("WeaponRarityText").GetComponent<TextMeshProUGUI>().text = "등급: " + weapon.Rank;
        weaponUI.transform.Find("WeaponAttackText").GetComponent<TextMeshProUGUI>().text = "공격력: " + weapon.AttackPoint;
        weaponUI.transform.Find("WeaponDurabilityText").GetComponent<TextMeshProUGUI>().text = "내구도: " + weapon.Durability;
        weaponUI.transform.Find("WeaponCrimeText").GetComponent<TextMeshProUGUI>().text = "Crime: " + GetCrimeDescription(weapon.Crime);
    }

    private void AdjustWeaponGridLayoutSize(int weaponCount)
    {
        RectTransform contentRect = BattleReadyWeaponParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = BattleReadyWeaponParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * weaponCount - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
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
