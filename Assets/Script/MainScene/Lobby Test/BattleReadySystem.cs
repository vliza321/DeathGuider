using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleReadySystem : MonoBehaviour
{
    public GameObject BattleReadyPrisonerPrefab;
    public GameObject BattleReadyWeaponPrefab;

    public Transform BattleReadyPrisonerParent;
    public Transform BattleReadyWeaponParent;

    private DontDestroyObjectManager DDOManager;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

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
        // 기존 UI 객체들 삭제
        foreach (Transform child in BattleReadyPrisonerParent)
        {
            Destroy(child.gameObject);
        }

        // 유닛 데이터가 초기화되었는지 체크
        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        // 필터 없이 모든 유닛을 가져오기
        var allUnits = DDOManager.UnitDatas.UnitDatas.FindAll(unit =>
        {
            if (unit == null)
            {
                Debug.LogWarning("UnitData 객체가 null입니다.");
                return false; // null 객체는 제외
            }

            return true; // 모든 유닛을 처리
        });

        // 유닛이 있을 경우 UI 생성
        if (allUnits.Count > 0)
        {
            foreach (var unit in allUnits)
            {
                CreateBattleReadyUnitUI(unit);
                Debug.Log($"유닛 이름: {unit.Name}");
            }
        }
        else
        {
            Debug.LogWarning("유닛 데이터가 없습니다.");
        }

        // 그리드 레이아웃 크기 조정
        AdjustUnitGridLayoutSize(allUnits.Count);
    }

    private void CreateBattleReadyUnitUI(UnitData unit)
    {
        // 유닛 UI 생성
        GameObject unitUI = Instantiate(BattleReadyPrisonerPrefab, BattleReadyPrisonerParent);

        unitUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = unit.Name;
        unitUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "Lv: " + unit.Level;
        unitUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "HP: " + unit.HealthPoint;
        unitUI.transform.Find("StrengthText").GetComponent<TextMeshProUGUI>().text = "STR: " + unit.Strength;
        unitUI.transform.Find("DefenseText").GetComponent<TextMeshProUGUI>().text = "DEF: " + unit.Defense;
        unitUI.transform.Find("CrimeText").GetComponent<TextMeshProUGUI>().text = "Crime: " + GetCrimeDescription(unit.Crime);

        // 헤드 이미지 설정
        if (unit.HeadID >= 0 && unit.HeadID < headSprites.Length)
        {
            unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = headSprites[unit.HeadID];
        }
        else
        {
            Debug.LogWarning($"Invalid HeadID: {unit.HeadID}");
        }

        // 바디 이미지 설정
        if (unit.BodyID >= 0 && unit.BodyID < bodySprites.Length)
        {
            unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = bodySprites[unit.BodyID];
        }
        else
        {
            Debug.LogWarning($"Invalid BodyID: {unit.BodyID}");
        }

        // Accept 버튼 처리
        //Button acceptButton = unitUI.transform.Find("AcceptButton").GetComponent<Button>();
        //if (acceptButton != null)
        //{
        //    //acceptButton.onClick.AddListener(() => AcceptUnitUI(unitUI));
        //}

        //// Reject 버튼 처리
        //Button rejectButton = unitUI.transform.Find("RejectButton").GetComponent<Button>();
        //if (rejectButton != null)
        //{
        //    //rejectButton.onClick.AddListener(() => RemoveUnitUI(unitUI));
        //}
        //else
        //{
        //    Debug.LogWarning("RejectButton not found in prefab.");
        //}
    }

    private void AdjustUnitGridLayoutSize(int unitCount)
    {
        // 그리드 레이아웃 크기 조정
        RectTransform contentRect = BattleReadyPrisonerParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = BattleReadyPrisonerParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * unitCount - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        // 레이아웃 강제 재구성
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");
    }

    public void DisplayBattleReadyWeapons()
    {
        // 기존 UI 객체들 삭제
        foreach (Transform child in BattleReadyWeaponParent)
        {
            Destroy(child.gameObject);
        }

        // 웨폰 데이터가 초기화되었는지 체크 (여기서는 UnitDatas로 대체한다고 가정)
        if (DDOManager.WeaponDatas == null || DDOManager.WeaponDatas.WeaponDatas == null)
        {
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        var allWeapons = DDOManager.WeaponDatas.WeaponDatas.FindAll(weapon =>
        {
            if (weapon == null)
            {
                Debug.LogWarning("Weapon 객체가 null입니다.");
                return false; // null 객체는 제외
            }

            return true; // 모든 웨폰 처리
        });

        if (allWeapons.Count > 0)
        {
            foreach (var weapon in allWeapons)
            {
                CreateBattleReadyWeaponUI(weapon);
                Debug.Log($"웨폰 이름: {weapon.Name}");
            }
        }
        else
        {
            Debug.LogWarning("웨폰 데이터가 없습니다.");
        }

        // 그리드 레이아웃 크기 조정
        AdjustWeaponGridLayoutSize(allWeapons.Count);
    }

    private void CreateBattleReadyWeaponUI(WeaponData weapon)
    {
        // 웨폰 UI 생성
        GameObject weaponUI = Instantiate(BattleReadyWeaponPrefab, BattleReadyWeaponParent);

        weaponUI.transform.Find("WeaponNameText").GetComponent<TextMeshProUGUI>().text = weapon.Name;
        weaponUI.transform.Find("WeaponRarityText").GetComponent<TextMeshProUGUI>().text = "등급: " + weapon.Rank;
        weaponUI.transform.Find("WeaponAttackText").GetComponent<TextMeshProUGUI>().text = "공격력: " + weapon.AttackPoint;
        weaponUI.transform.Find("WeaponDurabilityText").GetComponent<TextMeshProUGUI>().text = "내구도: " + weapon.Durability;
        weaponUI.transform.Find("WeaponCrimeText").GetComponent<TextMeshProUGUI>().text = "Crime: " + GetCrimeDescription(weapon.Crime);
    }

    private void AdjustWeaponGridLayoutSize(int weaponCount)
    {
        // 그리드 레이아웃 크기 조정
        RectTransform contentRect = BattleReadyWeaponParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = BattleReadyWeaponParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;

        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * weaponCount - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        // 레이아웃 강제 재구성
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");
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
