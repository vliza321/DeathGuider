using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GYMSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class BusRandomPrisoner : MonoBehaviour
{
    public List<UnitData> unitDatas = new List<UnitData>();
    public UnitData testUnit;
    public StorageUI storageUI;
    public GameObject unitUIPrefab;
    public Transform gridParent;

    private int dailyAcceptCount = 0;
    private int totalAcceptCount = 0;
    private int maxDailyAcceptCount = 3;
    public int availablePrisoner = 6;
    public Button changeDaysButton;
    public TextMeshProUGUI dailyAcceptCountText;
    public TextMeshProUGUI totalAcceptCountText;

    private readonly char[] name1 = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly char[] name2 = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private readonly char[] name3 = new char[] { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly string[] firstNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };

    public GYMSystem gYMSystem;
    public HealthSystem healthSystem;
    public ErosionSystem erosionSystem;
    public SmithSystem smithSystem;
    public BattleReadySystem battleReadySystem;
    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
    private void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }

            if(ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.GetComponent<GameManager>();
            }
        }
        DDO = null;

        if (changeDaysButton != null)
        {
            changeDaysButton.onClick.AddListener(OnChangeDaysButtonClicked);
        }

        for (int i = 0; i < availablePrisoner; i++)
        {
            GenerateRandomPrisoner();
        }
        changeDays();
       
    }

    public void UpdateUI()
    {
        if (dailyAcceptCountText != null)
            dailyAcceptCountText.text = $"오늘 수락 횟수: {dailyAcceptCount} / {maxDailyAcceptCount}";

        if (totalAcceptCountText != null)
        {
            int floorCount = DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor;

            int prisonerCount = 0;
            foreach (var unit in DDOManager.UnitDatas.UnitDatas)
            {
                if (unit.PrototypeUnitID == 100)
                {
                    prisonerCount++;
                }
            }

            totalAcceptCountText.text = $"전체 수락 횟수: {prisonerCount} / {4 * floorCount}";
        }
    }

    public void GenerateRandomPrisoner()
    {
        UnitData newUnit = new UnitData();

        newUnit.Name = GenerateRandomName();
        newUnit.Level = 1;
        newUnit.EXP = 0;
        newUnit.MaxHealthPoint = Random.Range(1, 101);
        newUnit.HealthPoint = newUnit.MaxHealthPoint;
        newUnit.Strength = Random.Range(1, 11);
        newUnit.Defense = Random.Range(1, 11);
        newUnit.Handicraft = Random.Range(1, 11);
        newUnit.DeathErosion = 0;
        newUnit.Enforce = 0;
        newUnit.HandicraftEnforce = 0;
        newUnit.Crime = Random.Range(0, 7);
        newUnit.ActivityStatus = 0;
        newUnit.HeadID = Random.Range(0, 3);
        newUnit.BodyID = Random.Range(0, 3);

        unitDatas.Add(newUnit);
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

        if (unicode >= 0xAC00 && unicode <= 0xD7A3)
        {
            return (char)unicode;
        }
        else
        {
            return CreateRandomKoreanChar();
        }
    }

    public void DisplayUnitDataUI()
    {
        foreach (var unit in unitDatas)
        {
            GameObject unitUI = Instantiate(unitUIPrefab, gridParent);

            unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = unit.Name;
            unitUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "Lv: " + unit.Level;
            unitUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "HP: " + unit.HealthPoint;
            unitUI.transform.Find("StrengthText").GetComponent<TextMeshProUGUI>().text = "STR:" + unit.Strength;
            unitUI.transform.Find("DefenseText").GetComponent<TextMeshProUGUI>().text = "DEF:" + unit.Defense;
            unitUI.transform.Find("CrimeText").GetComponent<TextMeshProUGUI>().text = "Crime: " + GetCrimeDescription(unit.Crime);

            if (unit.HeadID >= 0 && unit.HeadID < GameManager.PrisonerHeadImg.Count)
            {
                unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = GameManager.PrisonerHeadImg[unit.HeadID];
            }
            else
            {
                Debug.LogWarning($"Invalid HeadID: {unit.HeadID}");
            }

            if (unit.BodyID >= 0 && unit.BodyID < GameManager.PrisonerBodyImg.Count)
            {
                unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = GameManager.PrisonerBodyImg[unit.BodyID];
            }
            else
            {
                Debug.LogWarning($"Invalid BodyID: {unit.BodyID}");
            }

            Button acceptButton = unitUI.transform.Find("AcceptButton").GetComponent<Button>();
            if (acceptButton != null)
            {
                acceptButton.onClick.AddListener(() => AcceptUnitUI(unitUI));
            }

            Button rejectButton = unitUI.transform.Find("RejectButton").GetComponent<Button>();
            if (rejectButton != null)
            {
                rejectButton.onClick.AddListener(() => RemoveUnitUI(unitUI));
            }
        }

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * unitDatas.Count - spacingY + paddingUp;

        RectTransform contentRect = gridParent.GetComponent<RectTransform>();
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

    private void AcceptUnitUI(GameObject unitUI)
    {
        int floor = DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Floor;
        int maxTotalAcceptCount = floor * 4;

        if (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].UnitInstanceCounter >= maxTotalAcceptCount)
        {
            Debug.Log("최대 인원을 초과했습니다.");
            return;
        }

        if (dailyAcceptCount >= maxDailyAcceptCount)
        {
            Debug.Log("오늘 수락 횟수를 초과했습니다.");
            return;
        }

        string unitName = unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text;

        UnitData unitToRemove = null;
        foreach (var unit in unitDatas)
        {
            if (unit.Name == unitName)
            {
                unitToRemove = unit;
                break;
            }
        }

        if (unitToRemove != null)
        {
            unitToRemove.UserID = 0;
            unitToRemove.PrototypeUnitID = 100;
            unitToRemove.InstanceID = DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].UnitInstanceCounter;

            DDOManager.UnitDatas.UnitDatas.Add(unitToRemove);
            DDOManager.UnitDatas.UnitDataDic.Add((GameManager.SelectUserID, 100, DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].UnitInstanceCounter), unitToRemove);
            DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].UnitInstanceCounter++;

            unitDatas.Remove(unitToRemove);
        }
        Destroy(unitUI);

        dailyAcceptCount++;
        totalAcceptCount++;
        UpdateUI();
    }

    private void RemoveUnitUI(GameObject unitUI)
    {
        string unitName = unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text;

        UnitData unitToRemove = null;
        foreach (var unit in unitDatas)
        {
            if (unit.Name == unitName)
            {
                unitToRemove = unit;
                break;
            }
        }

        if (unitToRemove != null)
        {
            unitDatas.Remove(unitToRemove);
        }
        Destroy(unitUI);
    }


    private void OnChangeDaysButtonClicked()
    {
        DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Day++;
        DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Gold = 1000000;
        DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].DeathEssence = 1000000;
        DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].DarkEssence = 1000000;
        storageUI.UpdateDaysUI();
        storageUI.UpdateGold();
        storageUI.UpdatedeathEssence();
        storageUI.UpdatedarkEssence();
        dailyAcceptCount = 0;
        unitDatas.Clear();
        ClearExistingUnitUIs();
        gYMSystem.CheckAndResetGYMSystemState();
        smithSystem.GenerateNewWeaponDatas();
        healthSystem.CheckAndResetHealthSystemState();
        erosionSystem.CheckAndResetErosionSystemState();
        battleReadySystem.CheckAndResetDungeonSystemState();
        for (int i = 0; i < availablePrisoner; i++)
        {
            GenerateRandomPrisoner();
        }
        UpdateUI();
        DisplayUnitDataUI();
    }

    public void changeDays()
    {
        //storageUI.UpdateDaysUI();
        //storageUI.UpdateGold();
        //storageUI.UpdatedeathEssence();
        //storageUI.UpdatedarkEssence();
        dailyAcceptCount = 0;
        unitDatas.Clear();
        ClearExistingUnitUIs();
        gYMSystem.CheckAndResetGYMSystemState();
        smithSystem.GenerateNewWeaponDatas();
        healthSystem.CheckAndResetHealthSystemState();
        erosionSystem.CheckAndResetErosionSystemState();
        battleReadySystem.CheckAndResetDungeonSystemState();
        for (int i = 0; i < availablePrisoner; i++)
        {
            GenerateRandomPrisoner();
        }
        UpdateUI();
        DisplayUnitDataUI();
        DDOManager.SaveData();
    }

    private void ClearExistingUnitUIs()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
}