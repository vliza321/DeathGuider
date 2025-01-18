using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusRandomPrisoner : MonoBehaviour
{
    public List<UnitData> unitDatas = new List<UnitData>();
    public UnitData testUnit;

    public GameObject unitUIPrefab;
    public Transform gridParent;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    private int dailyAcceptCount = 0; // 하루 수락 횟수
    private int totalAcceptCount = 0; // 전체 수락 횟수
    private int currentFloor = 1; // 현재 층
    private int maxDailyAcceptCount = 3; // 하루 최대 수락 가능 횟수
    private int maxTotalAcceptCount => currentFloor * 4; // 전체 최대 수락 가능 횟수 (층 * 4)

    public Button changeDaysButton;

    private readonly char[] name1 = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly char[] name2 = new char[] { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private readonly char[] name3 = new char[] { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private readonly string[] firstNames = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };

    private DontDestroyObjectManager DDOManager;

    private void Start()
    {
        // DontDestroyOnLoad 객체에서 busPrisonerDataList를 로드
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            DDO = null;
        }

        if (changeDaysButton != null)
        {
            changeDaysButton.onClick.AddListener(OnChangeDaysButtonClicked);
        }

        for (int i=0; i<6; i++)
        {
            GenerateRandomPrisoner();
        }

        DisplayUnitDataUI();
    }

    public void GenerateRandomPrisoner()
    {
        UnitData newUnit = new UnitData();

        newUnit.Name = GenerateRandomName();
        newUnit.Level = 1;
        newUnit.EXP = 0;
        newUnit.MaxHealthPoint = Random.Range(1, 100);
        newUnit.HealthPoint = newUnit.MaxHealthPoint;
        newUnit.Strength = Random.Range(1, 100);
        newUnit.Defense = Random.Range(1, 100);
        newUnit.Handicraft = Random.Range(1, 100);
        newUnit.DeathErosion = 0;
        newUnit.Enforce = 0;
        newUnit.HandicraftEnforce = 0;
        newUnit.Crime = Random.Range(0, 7);
        newUnit.ActivityStatus = 0;
        newUnit.HeadID = Random.Range(0, 6);
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

            if (unit.HeadID >= 0 && unit.HeadID < headSprites.Length)
            {
                unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = headSprites[unit.HeadID];
            }
            else
            {
                Debug.LogWarning($"Invalid HeadID: {unit.HeadID}");
            }

            if (unit.BodyID >= 0 && unit.BodyID < bodySprites.Length)
            {
                unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = bodySprites[unit.BodyID];
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
            else
            {
                Debug.LogWarning("RejectButton not found in prefab.");
            }
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

    private void AcceptUnitUI(GameObject unitUI)
    {
        if (dailyAcceptCount >= maxDailyAcceptCount)
        {
            Debug.Log("하루 수락 가능 횟수를 초과했습니다!");
            return;
        }

        if (totalAcceptCount >= maxTotalAcceptCount)
        {
            Debug.Log("전체 수락 가능 횟수를 초과했습니다!");
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
            unitToRemove.InstanceID = DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter;

            DDOManager.UnitDatas.UnitDatas.Add(unitToRemove);
            DDOManager.UnitDatas.UnitDataDic.Add((0, 100, DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter), unitToRemove);
            DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter++;
            unitDatas.Remove(unitToRemove);

            Debug.Log($"BodyID in DDOManager: {DDOManager.UnitDatas.UnitDatas[^1].BodyID}");

            if (!DDOManager.SaveData())
            {
                Debug.Log("Fail Save Data");
            }
        }
        Destroy(unitUI);

        dailyAcceptCount++;
        totalAcceptCount++;
        Debug.Log($"오늘 수락: {dailyAcceptCount}/{maxDailyAcceptCount}, 총 수락: {totalAcceptCount}/{maxTotalAcceptCount}");
    }

    private void RemoveUnitUI(GameObject unitUI)
    {
        string unitName = unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text;

        UnitData unitToRemove = null;
        foreach(var unit in unitDatas)
        {
            if(unit.Name == unitName)
            {
                unitToRemove = unit;
                break;
            }
        }

        if(unitToRemove != null)
        {
            unitDatas.Remove(unitToRemove);
        }
        Destroy(unitUI); // 해당 프리펩 삭제
    }

    private void OnChangeDaysButtonClicked()
    {
        DDOManager.LocalUserDatas.LocalUserDataDic[0].Day++;

        if (!DDOManager.SaveData())
        {
            Debug.Log("Fail Save Data");
        }

        dailyAcceptCount = 0;
        unitDatas.Clear();
        ClearExistingUnitUIs();

        for (int i = 0; i < 6; i++)
        {
            GenerateRandomPrisoner();
        }
        DisplayUnitDataUI();
    }

    private void ClearExistingUnitUIs()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
}