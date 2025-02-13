using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SmithSystem : MonoBehaviour
{
    public StorageUI storageUI;

    public GameObject newWeaponPrefab;
    public GameObject haveWeaponPrefab;

    public Transform newWeaponParent;
    public Transform haveWeaponParent;

    public int newWeaponIndex = 3;
    public int currentWeaponRank = 0;
    public int discountWeapon = 1;

    [System.Serializable]
    public class newWeaponDataList
    {
        public int ID;
        public string WeaponName;
        public int AttackPoint;
        public int Type;
        public int Enforce = 0;
        public int Durability = 100;
        public int Crime;
        public int Rank;
        public int GoldCost;
        public int DarkCost;
        public int calculatedGoldCost;
        public int calculatedDarkCost;

        public GameObject ParentWeaponObj;
        public void SetRank(int newRank)
        {
            Rank = newRank;
            SetCostOnRank();
        }

        public void SetCostOnRank()
        {
            switch (Rank)
            {
                case 0:
                    GoldCost = 100;
                    DarkCost = 0;
                    break;
                case 1:
                    GoldCost = 200;
                    DarkCost = 50;
                    break;
                case 2:
                    GoldCost = 300;
                    DarkCost = 100;
                    break;
                case 3:
                    GoldCost = 400;
                    DarkCost = 150;
                    break;
                case 4:
                    GoldCost = 500;
                    DarkCost = 200;
                    break;
                case 5:
                    GoldCost = 600;
                    DarkCost = 300;
                    break;
                default:
                    GoldCost = 0;
                    DarkCost = 0;
                    break;
            }
        }
    }

    public struct EnforceCost
    {
        public int GoldCost;
        public int DarkCost;

        public EnforceCost(int gold, int dark)
        {
            GoldCost = gold;
            DarkCost = dark;
        }
    }

    private Dictionary<int, EnforceCost> enforceCosts = new Dictionary<int, EnforceCost>
{
    { 0, new EnforceCost(100, 50) },  // 기본 가격
    { 1, new EnforceCost(200, 100) },
    { 2, new EnforceCost(300, 150) },
    { 3, new EnforceCost(400, 200) },
    { 4, new EnforceCost(500, 250) },
    { 5, new EnforceCost(600, 300) }
};

    // 각 Type에 맞게 가격을 조정하기 위해 곱할 배수 정의
    private float[] typeMultipliers = new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f };

    public EnforceCost GetAdjustedEnforceCost(int weaponRank, int level)
    {
        // 유효한 weaponRank, level 값 확인
        if (weaponRank < 0 || weaponRank > 5 || level < 0 || level > 5)
        {
            Debug.LogError("잘못된 weaponType 또는 level");
            return new EnforceCost(0, 0);
        }

        // 기본 가격을 가져온 뒤, 해당 Type의 배수로 가격을 조정
        EnforceCost baseCost = enforceCosts[level];
        float multiplier = typeMultipliers[weaponRank];

        // smithEnhance 적용: SmithEnhance 값에 따라 강화 비용을 감소시킴
        float smithEnhanceMultiplier = 1.0f - (DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance * 0.1f);

        // 타입에 따른 배수와 smithEnhance를 적용한 최종 가격 계산
        int adjustedGoldCost = Mathf.CeilToInt(baseCost.GoldCost * multiplier * smithEnhanceMultiplier);
        int adjustedDarkCost = Mathf.CeilToInt(baseCost.DarkCost * multiplier * smithEnhanceMultiplier);

        // 계산된 강화 비용 반환
        return new EnforceCost(adjustedGoldCost, adjustedDarkCost);
    }

    public struct EvolveCost
    {
        public int GoldCost;
        public int DarkCost;

        public EvolveCost(int goldCost, int darkCost)
        {
            GoldCost = goldCost;
            DarkCost = darkCost;
        }
    }

    // 무기의 진화 비용을 가져오는 메서드
    private EvolveCost GetAdjustedEvolveCost(int weaponRank)
    {
        int smithEnhance = DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance;

        EvolveCost baseCost = weaponRank switch
        {
            0 => new EvolveCost(5000, 50),
            1 => new EvolveCost(10000, 100),
            2 => new EvolveCost(20000, 200),
            3 => new EvolveCost(30000, 300),
            4 => new EvolveCost(40000, 400),
            _ => new EvolveCost(0, 0)
        };

        float discountRate = 1.0f - (smithEnhance * 0.1f);

        int discountedGoldCost = Mathf.Max((int)(baseCost.GoldCost * discountRate), 0);
        int discountedDarkCost = Mathf.Max((int)(baseCost.DarkCost * discountRate), 0);

        return new EvolveCost(discountedGoldCost, discountedDarkCost);
    }

    // 수리 비용을 계산하는 함수
    private RepairCost CalculateRepairCost(WeaponData weaponData)
    {
        // 기본 수리 비용 계산 (레벨과 랭크에 따른)
        int baseGoldCost = 100 + weaponData.Rank * 500 + weaponData.Enforce * 200;
        int baseDarkCost = 10 + weaponData.Rank * 30 + weaponData.Enforce * 10;

        // SmithEnhance에 따른 할인 계산 (예: 1당 5% 할인)
        float smithEnhanceDiscount = 1 - (DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance * 0.05f);

        // 할인된 수리 비용
        int discountedGoldCost = Mathf.Max(1, Mathf.FloorToInt(baseGoldCost * smithEnhanceDiscount));
        int discountedDarkCost = Mathf.Max(1, Mathf.FloorToInt(baseDarkCost * smithEnhanceDiscount));

        return new RepairCost
        {
            GoldCost = discountedGoldCost,
            DarkCost = discountedDarkCost
        };
    }


    // 수리 비용을 나타내는 구조체
    public struct RepairCost
    {
        public int GoldCost;
        public int DarkCost;
    }

    public List<newWeaponDataList> newWeaponDatas = new List<newWeaponDataList>();

    private struct WeaponStatsRange
    {
        public int MinAttack;
        public int MaxAttack;

        public WeaponStatsRange(int min, int max)
        {
            MinAttack = min;
            MaxAttack = max;
        }

        public int GetRandomAttackPoint()
        {
            return UnityEngine.Random.Range(MinAttack, MaxAttack + 1);
        }
    }

    private static readonly Dictionary<int, WeaponStatsRange> weaponAttackRanges = new()
    {
        { 0, new WeaponStatsRange(50, 100) },
        { 1, new WeaponStatsRange(100, 150) },
        { 2, new WeaponStatsRange(150, 200) },
        { 3, new WeaponStatsRange(200, 250) },
        { 4, new WeaponStatsRange(250, 300) },
        { 5, new WeaponStatsRange(300, 350) }
    };

    private int GetRandomAttackPoint(int rank)
    {
        return weaponAttackRanges.TryGetValue(rank, out var range) ? range.GetRandomAttackPoint() : 0;
    }

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;

    void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.name == "GameManager")
            {
                GameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;
        GenerateNewWeaponDatas();
    }

    public void GenerateHaveWeaponDatas()
    {
        foreach (Transform child in haveWeaponParent)
        {
            Debug.Log($"Destroying: {child.gameObject.name}");
            Destroy(child.gameObject);
        }

        foreach (var weaponData in DDOManager.WeaponDatas.WeaponDatas)
        {
            GameObject weaponObj = Instantiate(haveWeaponPrefab, haveWeaponParent);
            weaponObj.name = $"Weapon {weaponData.PrototypeWeaponID}";
            TextMeshProUGUI rarityText = weaponObj.transform.Find("WeaponRarityText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = weaponObj.transform.Find("WeaponNameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackText = weaponObj.transform.Find("WeaponAttackText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI durabilityText = weaponObj.transform.Find("WeaponDurabilityText")?.GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI weaponEnforceCost = weaponObj.transform.Find("WeaponEnforceCost")?.GetComponent <TextMeshProUGUI>();
            TextMeshProUGUI weaponRepairCost = weaponObj.transform.Find("WeaponRepairCost")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponSaleCost = weaponObj.transform.Find("WeaponSaleCost")?.GetComponent<TextMeshProUGUI>();

            Slider weaponLevel = weaponObj.transform.Find("WeaponLevel")?.GetComponent<Slider>();

            Button weaponEnforceButton =  weaponObj.transform.Find("WeaponEnforceButton")?.GetComponent<Button>();
            Button weaponRepairButton = weaponObj.transform.Find("WeaponRepairButton")?.GetComponent<Button>();
            Button weaponSaleButton = weaponObj.transform.Find("WeaponSaleButton")?.GetComponent<Button>();

            if (rarityText != null) rarityText.text = "등급: " + weaponData.Rank.ToString();
            if (nameText != null) nameText.text = weaponData.Name;
            if (attackText != null) attackText.text = "공격력: " + weaponData.AttackPoint.ToString();
            if (durabilityText != null) durabilityText.text = "내구도: " + weaponData.Durability.ToString();

            if (weaponLevel != null)
            {
                weaponLevel.maxValue = 5;
                weaponLevel.value = weaponData.Enforce;
            }

            TextMeshProUGUI enforceButtonText = weaponEnforceButton?.GetComponentInChildren<TextMeshProUGUI>();
            if (weaponEnforceCost != null)
            {
                int level = weaponData.Enforce;

                // 강화 가능 레벨이 5 이상이면 진화 비용을 표시
                if (level >= 5)
                {
                    // 진화 비용을 계산
                    EvolveCost adjustedEvolveCost = GetAdjustedEvolveCost(weaponData.Rank);
                    weaponEnforceCost.text = $"{adjustedEvolveCost.GoldCost}G / {adjustedEvolveCost.DarkCost}D";

                    // 버튼 텍스트를 "진화"로 변경
                    if (weaponEnforceButton != null)
                    {
                        enforceButtonText.text = "진화";
                    }
                }
                else
                {
                    // 강화 비용을 계산
                    EnforceCost adjustedCost = GetAdjustedEnforceCost(weaponData.Rank, level);
                    weaponEnforceCost.text = $"{adjustedCost.GoldCost}G / {adjustedCost.DarkCost}D";

                    // 버튼 텍스트를 "강화"로 유지
                    if (weaponEnforceButton != null)
                    {
                        enforceButtonText.text = "강화";
                    }
                }
            }

            if (weaponRepairCost != null)
            {
                // 수리 비용 계산
                RepairCost repairCost = CalculateRepairCost(weaponData);

                // 수리 비용을 텍스트로 표시
                weaponRepairCost.text = $"{repairCost.GoldCost}G / {repairCost.DarkCost}D";
            }

            if (weaponSaleCost != null)
            {
                // 판매 금액 계산
                int salePrice = CalculateSalePrice(weaponData); // 판매 금액을 계산하는 함수 (기존에 정의된 함수)

                // 판매 금액을 텍스트로 표시
                weaponSaleCost.text = $"{salePrice}G";
            }

            Image weaponImage = weaponObj.transform.Find("WeaponBox/WeaponImage")?.GetComponent<Image>();
            if (weaponImage != null)
            {
                weaponImage.sprite = GameManager.WeaponImg[weaponData.PrototypeWeaponID];
            }

            if (weaponEnforceButton != null)
                weaponEnforceButton.onClick.AddListener(() => EnforceButtonClick(weaponData, weaponEnforceCost, attackText, weaponLevel, enforceButtonText));
            if (weaponRepairButton != null) weaponRepairButton.onClick.AddListener(() => RepairButtonClick(weaponData, weaponRepairCost));
            if (weaponSaleButton != null) weaponSaleButton.onClick.AddListener(() => SaleButtonClick(weaponData));
        }

        RectTransform contentRect = haveWeaponParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = haveWeaponParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;
        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;
        int itemCount = DDOManager.WeaponDatas.WeaponDatas.Count;
        float newHeight = (cellHeight + spacingY) * itemCount - spacingY + paddingUp;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    private void EnforceButtonClick(WeaponData weaponData, TextMeshProUGUI enforceCostText, TextMeshProUGUI attackText, Slider levelSlider, TextMeshProUGUI enforceButtonText)
    {
        int level = weaponData.Enforce; // 현재 레벨

        // 레벨이 5 이상이면 강화 불가
        if (level >= 5)
        {
            // Rank와 Enforce가 모두 5일 경우 'Max' 표시
            if (weaponData.Rank >= 5 && weaponData.Enforce >= 5)
            {
                enforceButtonText.text = "Max";
                return;
            }

            // Rank가 5일 경우 진화 불가
            if (weaponData.Rank >= 5)
            {
                Debug.Log("최대 레벨입니다. 더 이상 진화할 수 없습니다.");
                return;
            }

            // 진화 가능
            EvolveCost evolveCost = GetAdjustedEvolveCost(weaponData.Rank);

            // 필요 골드와 다크가 부족한지 확인
            if (DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold < evolveCost.GoldCost ||
                DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence < evolveCost.DarkCost)
            {
                Debug.Log("진화에 필요한 재화가 부족합니다.");
                return;
            }

            // 진화 비용 차감
            DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= evolveCost.GoldCost;
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= evolveCost.DarkCost;

            //임시코드
            weaponData.AttackPoint += GetEvolveAttackBonus(weaponData.Rank);

            // 진화: Enforce를 0으로 리셋하고, Rank를 +1
            weaponData.Durability = 100;
            weaponData.Enforce = 0;
            weaponData.Rank++;
            
            var key = (weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID);
            if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
            {
                // 무기 데이터를 업데이트
                DDOManager.WeaponDatas.WeaponDataDic[key] = weaponData;
                Debug.Log($"[딕셔너리 업데이트] {weaponData.Name} 진화 완료: 새로운 등급: {weaponData.Rank}, 새로운 공격력: {weaponData.AttackPoint}");
            }
            else
            {
                Debug.LogError("[딕셔너리 오류] 무기 데이터가 딕셔너리에 존재하지 않습니다.");
            }

            // 추후 업데이트 되어야하는 코드
            //int EvolutionedWeaponID = weaponData.PrototypeWeaponID + 1000;
            //DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[EvolutionedWeaponID].InstanceCounter++;

            //weaponData.PrototypeWeaponID = EvolutionedWeaponID;
            //weaponData.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weaponData.PrototypeWeaponID].InstanceCounter;
            //weaponData.Name = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weaponData.PrototypeWeaponID].Name;
            //weaponData.AttackPoint = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weaponData.PrototypeWeaponID].AttackPoint;
            //weaponData.Durability = 100;
            //weaponData.Enforce = 0;
            //weaponData.Rank++;

            // 버튼 텍스트 변경
            enforceButtonText.text = (weaponData.Rank >= 5) ? "Max" : "강화";


            // 진화 후 비용을 강화 비용으로 변경
            EnforceCost nextCost = GetAdjustedEnforceCost(weaponData.Rank, weaponData.Enforce);
            enforceCostText.text = $"{nextCost.GoldCost}G / {nextCost.DarkCost}D"; // 강화 비용 업데이트

            Debug.Log($"진화 완료: {weaponData.Name}, 새로운 등급: {weaponData.Rank}, 새로운 공격력: {weaponData.AttackPoint}");

            GenerateHaveWeaponDatas();
            // UI 업데이트
            storageUI.UpdateGold();
            storageUI.UpdatedarkEssence();
            return;
        }

        // 레벨이 5 미만일 때는 기존 강화 코드 실행
        if (!enforceCosts.ContainsKey(level)) return; // 해당 레벨에 대한 비용이 존재하는지 확인

        // 강화 비용을 조정하여 가져오기
        EnforceCost adjustedCost = GetAdjustedEnforceCost(weaponData.Type, level);

        // 필요 골드와 다크가 부족한지 확인
        if (DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold < adjustedCost.GoldCost ||
            DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence < adjustedCost.DarkCost)
        {
            Debug.Log("강화에 필요한 재화가 부족합니다.");
            return;
        }

        // 재화 차감
        DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= adjustedCost.GoldCost;
        DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= adjustedCost.DarkCost;

        // 공격력 증가 (강화 시 증가량 적용)
        weaponData.AttackPoint += GetEnforceAttackBonus(weaponData.Rank, weaponData.Enforce);

        // 레벨 증가
        weaponData.Enforce++;

        var keyEnforce = (weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID);
        if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(keyEnforce))
        {
            // 무기 데이터를 업데이트
            DDOManager.WeaponDatas.WeaponDataDic[keyEnforce] = weaponData;
            Debug.Log($"[딕셔너리 업데이트] {weaponData.Name} 강화 완료: 새로운 레벨: {weaponData.Enforce}, 새로운 공격력: {weaponData.AttackPoint}");
        }
        else
        {
            Debug.LogError("[딕셔너리 오류] 무기 데이터가 딕셔너리에 존재하지 않습니다.");
        }

        // 레벨 슬라이더 업데이트
        if (levelSlider != null)
        {
            levelSlider.value = weaponData.Enforce;
        }

        // 강화 비용 텍스트 업데이트
        if (enforceCostText != null)
        {
            if (weaponData.Enforce >= 5)
            {
                EvolveCost nextEvolveCost = GetAdjustedEvolveCost(weaponData.Rank);
                enforceCostText.text = $"최고 단계";
            }
            else
            {
                EnforceCost nextCost = GetAdjustedEnforceCost(weaponData.Rank, weaponData.Enforce);
                enforceCostText.text = $"{nextCost.GoldCost}G / {nextCost.DarkCost}D";
            }
        }

        if (attackText != null)
        {
            attackText.text = $"공격력: {weaponData.AttackPoint}";
        }

        if (enforceButtonText != null)
        {
            if (weaponData.Enforce >= 5 && weaponData.Rank >= 5)
            {
                enforceButtonText.text = "Max";
            }
            else if (weaponData.Enforce >= 5)
            {
                enforceButtonText.text = "진화";
            }
            else
            {
                enforceButtonText.text = "강화";
            }
        }

        Debug.Log($"무기 강화 완료: {weaponData.Name}, 새로운 레벨: {weaponData.Enforce}, 새로운 공격력: {weaponData.AttackPoint}");

        // UI 업데이트
        storageUI.UpdateGold();
        storageUI.UpdatedarkEssence();
    }

    private int GetEnforceAttackBonus(int rank, int level)
    {
        return (rank + 1) * (level + 1) * 2;
    }

    private int GetEvolveAttackBonus(int rank)
    {
        return (rank + 1) * 10;
    }

    private void RepairButtonClick(WeaponData weaponData, TextMeshProUGUI weaponRepairCost)
    {
        // 내구도가 100이면 수리 불가
        if (weaponData.Durability >= 100)
        {
            Debug.Log($"[수리 불가] {weaponData.Name}의 내구도가 이미 최대치입니다. 내구도: {weaponData.Durability}");
            return;
        }

        // 수리 비용 계산
        RepairCost repairCost = CalculateRepairCost(weaponData);
        Debug.Log($"[수리 비용] {weaponData.Name} 수리 비용 계산 완료: {repairCost.GoldCost}G / {repairCost.DarkCost}D");

        // 유저가 필요한 자원이 있는지 확인 (골드 및 다크에센스)
        int userGold = DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold;
        int userDark = DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence;

        Debug.Log($"[자원 확인] 현재 유저 자원: {userGold}G / {userDark}D");

        if (userGold < repairCost.GoldCost || userDark < repairCost.DarkCost)
        {
            Debug.LogError("[수리 실패] 수리에 필요한 자원이 부족합니다.");
            return;
        }

        // 자원 차감
        DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= repairCost.GoldCost;
        DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= repairCost.DarkCost;
        Debug.Log($"[자원 차감] 수리 비용 차감: {repairCost.GoldCost}G / {repairCost.DarkCost}D");

        // SmithEnhance에 따라 내구도 회복 (2씩 회복)
        int recoverAmount = 2 + DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance * 2;
        Debug.Log($"[회복량 계산] SmithEnhance에 따른 회복량: {recoverAmount} (SmithEnhance: {DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance})");

        weaponData.Durability += recoverAmount;
        Debug.Log($"[내구도 변경] 수리 후 내구도: {weaponData.Durability}");

        // 내구도가 100을 넘지 않도록 제한
        if (weaponData.Durability > 100)
        {
            weaponData.Durability = 100;
            Debug.Log("[내구도 제한] 내구도가 100을 초과하여 100으로 제한되었습니다.");
        }

        // 딕셔너리에서 해당 무기 데이터 찾기 (수정만 하면 됨)
        var key = (weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID);
        Debug.Log($"[딕셔너리 키 확인] 딕셔너리 키: ({key.UserID}, {key.PrototypeWeaponID}, {key.InstanceID})");

        // 수리 후 내구도 업데이트
        if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
        {
            // 내구도를 업데이트
            DDOManager.WeaponDatas.WeaponDataDic[key].Durability = weaponData.Durability;
            Debug.Log($"[딕셔너리 업데이트] {weaponData.Name} 내구도 업데이트 완료: {weaponData.Durability}");
        }
        else
        {
            Debug.LogError("[딕셔너리 오류] 무기 데이터가 딕셔너리에 존재하지 않습니다.");
        }

        // UI 갱신 (골드 및 다크에센스 업데이트)
        storageUI.UpdateGold();
        storageUI.UpdatedarkEssence();
        Debug.Log("[UI 업데이트] 골드 및 다크에센스 UI 갱신 완료.");

        // 수리 완료 로그
        Debug.Log($"[수리 완료] {weaponData.Name} 수리 완료: 새로운 내구도 = {weaponData.Durability}");

        // 수리 비용 텍스트 갱신
        if (weaponRepairCost != null)
        {
            weaponRepairCost.text = $"{repairCost.GoldCost}G / {repairCost.DarkCost}D";
            Debug.Log($"[수리 비용 텍스트 갱신] 수리 비용: {repairCost.GoldCost}G / {repairCost.DarkCost}D");
        }

        // UI 갱신
        GenerateHaveWeaponDatas();
        Debug.Log("[UI 갱신] 수리 후 보유 무기 목록 UI 갱신 완료.");
    }

    private void SaleButtonClick(WeaponData weaponData)
    {
        int salePrice = CalculateSalePrice(weaponData);
        
        DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold += salePrice;
        Debug.Log($"무기 판매 완료: {weaponData.Name}, 판매 금액: {salePrice}G");

        // 무기 데이터를 삭제할 때, 딕셔너리와 리스트에서 제거
        var key = (weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID);

        if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
        {
            DDOManager.WeaponDatas.WeaponDataDic.Remove(key);
            Debug.Log($"무기 데이터 딕셔너리에서 삭제: {weaponData.Name}");
        }

        if (DDOManager.WeaponDatas.WeaponDatas.Contains(weaponData))
        {
            DDOManager.WeaponDatas.WeaponDatas.Remove(weaponData);
            Debug.Log($"무기 리스트에서 삭제: {weaponData.Name}");
        }

        GenerateHaveWeaponDatas();

        storageUI.UpdateGold();
    }

    private int CalculateSalePrice(WeaponData weaponData)
    {
        int basePrice = 100;

        int plusPrice = weaponData.Rank * 1000 + weaponData.Enforce * 500;

        float smithEnhanceMultiplier = 1.0f + (DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance * 0.2f);
        smithEnhanceMultiplier = Mathf.Clamp(smithEnhanceMultiplier, 1.0f, 2.0f);

        int adjustedPrice = Mathf.RoundToInt((basePrice + plusPrice) * smithEnhanceMultiplier);

        return adjustedPrice; ;
    }

    public void UpdateWeaponUI(newWeaponDataList newWeapon, TextMeshProUGUI goldText, TextMeshProUGUI darkText)
    {
        int smithEnhance = Mathf.Clamp(DDOManager.LocalUserDatas.LocalUserDataDic[0].SmithEnhance, 0, 5);

        newWeapon.calculatedGoldCost = Mathf.Max(0, newWeapon.GoldCost - smithEnhance);
        newWeapon.calculatedDarkCost = Mathf.Max(0, newWeapon.DarkCost - smithEnhance);

        if (goldText != null)
            goldText.text = $"{newWeapon.calculatedGoldCost}G";
        if (darkText != null)
<<<<<<< Updated upstream
            darkText.text = $"{newWeapon.calculatedDarkCost}D";
=======
            darkText.text = $"{newWeapon.calculatedDarkCost}K";
>>>>>>> Stashed changes
    }

    //public void GenerateNewWeaponDatas()
    //{
    //    foreach (Transform child in newWeaponParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    newWeaponDatas.Clear();

    //    Dictionary<(int, int), int> instanceIDTracker = new Dictionary<(int, int), int>();
    //    foreach (var weaponData in DDOManager.WeaponDatas.WeaponDatas)
    //    {
    //        var key = (weaponData.UserID, weaponData.PrototypeWeaponID);
    //        if (!instanceIDTracker.ContainsKey(key))
    //        {
    //            instanceIDTracker[key] = weaponData.InstanceID;
    //        }
    //        else
    //        {
    //            instanceIDTracker[key] = Mathf.Max(instanceIDTracker[key], weaponData.InstanceID);
    //        }
    //    }

    //    List<int> selectedIDs = new List<int>();

    //    for (int i = 0; i < newWeaponIndex; i++)
    //    {
    //        GameObject weaponObj = Instantiate(newWeaponPrefab, newWeaponParent);
    //        weaponObj.name = $"Smith New Weapon {i + 1}";

    //        TextMeshProUGUI goldText = weaponObj.transform.Find("GoldText")?.GetComponent<TextMeshProUGUI>();
    //        TextMeshProUGUI darkText = weaponObj.transform.Find("DarkText")?.GetComponent<TextMeshProUGUI>();
    //        Button chooseButton = weaponObj.transform.Find("ChooseButton")?.GetComponent<Button>();

    //        int selectedID;
    //        do
    //        {
    //            selectedID = UnityEngine.Random.Range(0, 3);
    //        } while (selectedIDs.Contains(selectedID));

    //        selectedIDs.Add(selectedID);

    //        PrototypeWeaponData baseWeapon = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDatas[selectedID];

    //        var key = (baseWeapon.ID, baseWeapon.InstanceCounter);
    //        if (!instanceIDTracker.ContainsKey(key))
    //        {
    //            instanceIDTracker[key] = baseWeapon.InstanceCounter;
    //        }
    //        else
    //        {
    //            instanceIDTracker[key]++;
    //        }

    //        int newInstanceID = instanceIDTracker[key];

    //        newWeaponDatas.Add(new newWeaponDataList
    //        {
    //            ID = baseWeapon.ID,
    //            WeaponName = baseWeapon.Name,
    //            AttackPoint = baseWeapon.AttackPoint,
    //            Type = baseWeapon.Type,
    //            InstanceCounter = newInstanceID,
    //            Durability = 100,
    //            Enforce = 0,
    //            Rank = currentWeaponRank,
    //            GoldCost = 0,
    //            DarkCost = 0,
    //            calculatedGoldCost = 0,
    //            calculatedDarkCost = 0,
    //            ParentWeaponObj = weaponObj
    //        });

    //        Image weaponImage = weaponObj.transform.Find("Image/WeaponImage")?.GetComponent<Image>();
    //        if (weaponImage != null)
    //        {
    //            weaponImage.sprite = GameManager.WeaponImg[baseWeapon.ID];
    //        }


    //        newWeaponDatas.Last().SetRank(newWeaponDatas.Last().Rank);
    //        UpdateWeaponUI(newWeaponDatas.Last(), goldText, darkText);

    //        if (chooseButton != null)
    //        {
    //            int index = i; // index를 버튼 클릭 시 전달
    //            chooseButton.onClick.AddListener(() => OnChooseButtonClick(index));
    //        }
    //    }
    //}

    public void UpdateSmithEnhanceAndUI()
    {
        foreach (var newWeapon in newWeaponDatas)
        {
            // UI 요소 가져오기
            TextMeshProUGUI goldText = newWeapon.ParentWeaponObj.transform.Find("GoldText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI darkText = newWeapon.ParentWeaponObj.transform.Find("DarkText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI enforceCostText = newWeapon.ParentWeaponObj.transform.Find("EnforceCostText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI enforceButtonText = newWeapon.ParentWeaponObj.transform.Find("EnforceButtonText")?.GetComponent<TextMeshProUGUI>();

            // 진화 비용 계산
            EvolveCost nextEvolveCost = GetAdjustedEvolveCost(newWeapon.Rank);

            // 진화 비용 텍스트 업데이트
            if (enforceCostText != null)
            {
                enforceCostText.text = $"{nextEvolveCost.GoldCost}G / {nextEvolveCost.DarkCost}D";
            }

            // 진화 버튼 텍스트 업데이트
            if (enforceButtonText != null)
            {
                if (newWeapon.Enforce >= 5)
                {
                    enforceButtonText.text = (newWeapon.Rank >= 5) ? "Max" : "진화";
                }
                else
                {
                    enforceButtonText.text = "강화";
                }
            }

            // UI 업데이트
            UpdateWeaponUI(newWeapon, goldText, darkText);
        }
    }

    public void GenerateNewWeaponDatas()
    {
        foreach (Transform child in newWeaponParent)
        {
            Destroy(child.gameObject);
        }

        newWeaponDatas.Clear();

        // 각 ID별로 최대 InstanceID 값을 추적
        Dictionary<(int, int), int> instanceIDTracker = new Dictionary<(int, int), int>();

        // 기존 WeaponData에서 InstanceID 최대값 추적
        foreach (var weaponData in DDOManager.WeaponDatas.WeaponDatas)
        {
            var key = (weaponData.UserID, weaponData.PrototypeWeaponID);
            if (!instanceIDTracker.ContainsKey(key))
            {
                instanceIDTracker[key] = weaponData.InstanceID;
            }
            else
            {
                instanceIDTracker[key] = Mathf.Max(instanceIDTracker[key], weaponData.InstanceID);
            }
        }

        for (int i = 0; i < newWeaponIndex; i++)
        {
            GameObject weaponObj = Instantiate(newWeaponPrefab, newWeaponParent);
            weaponObj.name = $"Smith New Weapon {i + 1}";

            TextMeshProUGUI goldText = weaponObj.transform.Find("GoldText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI darkText = weaponObj.transform.Find("DarkText")?.GetComponent<TextMeshProUGUI>();
            Button chooseButton = weaponObj.transform.Find("ChooseButton")?.GetComponent<Button>();

            // 랜덤하게 무기 선택
            int selectedID = UnityEngine.Random.Range(0, 3/*DDOManager.PrototypeWeaponDatas.PrototypeWeaponDatas.Count // 추후에는 상수값으로 해야됨*/);
            int rank = UnityEngine.Random.Range(0,currentWeaponRank);
            PrototypeWeaponData baseWeapon = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDatas[selectedID /* + rank * 1000*/];
            // 새 무기 데이터 생성 (newWeaponDatas 리스트에 추가)
            newWeaponDatas.Add(new newWeaponDataList
            {
                ID = baseWeapon.ID,
                WeaponName = baseWeapon.Name ,
                AttackPoint = baseWeapon.AttackPoint,
                Type = baseWeapon.Type,
                Durability = 100,
                Enforce = 0,
                Rank = rank,
                GoldCost = 0, // 적절한 금액을 설정
                DarkCost = 0, // 적절한 다크 코스트 설정
                calculatedGoldCost = 0, // 계산된 금액 설정
                calculatedDarkCost = 0, // 계산된 다크 코스트 설정
                ParentWeaponObj = weaponObj // 새로 생성된 오브젝트 추가
            });

            Image weaponImage = weaponObj.transform.Find("Image/WeaponImage")?.GetComponent<Image>();
            if (weaponImage != null)
            {
                weaponImage.sprite = GameManager.WeaponImg[baseWeapon.ID];
            }

            newWeaponDatas.Last().SetRank(newWeaponDatas.Last().Rank);
            UpdateWeaponUI(newWeaponDatas.Last(), goldText, darkText);

            if (chooseButton != null)
            {
                int index = i; // index를 버튼 클릭 시 전달
                chooseButton.onClick.AddListener(() => OnChooseButtonClick(index));
            }
        }
    }

    public void OnChooseButtonClick(int index)
    {
        var weaponDataDic = DDOManager.WeaponDatas.WeaponDataDic;
        int newKey = weaponDataDic.Count;
        var selectedWeapon = newWeaponDatas[index]; // index로 선택된 무기 찾기

        int userGold = DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold;
        int userDark = DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence;

        if (userGold < selectedWeapon.calculatedGoldCost || userDark < selectedWeapon.calculatedDarkCost)
        {
            Debug.LogError("Not enough resources to purchase the weapon.");
            return;
        }

        DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold -= selectedWeapon.calculatedGoldCost;
        DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence -= selectedWeapon.calculatedDarkCost;
        storageUI.UpdateGold();
        storageUI.UpdatedarkEssence();

        WeaponData newWeaponData = new WeaponData
        {
            UserID = GameManager.SelectUserID,
            PrototypeWeaponID = selectedWeapon.ID,
            InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDatas[selectedWeapon.ID].InstanceCounter,
            Name = selectedWeapon.WeaponName,
            AttackPoint = selectedWeapon.AttackPoint,
            Durability = selectedWeapon.Durability,
            Rank = selectedWeapon.Rank,
            Type = selectedWeapon.Type,
            Enforce = selectedWeapon.Enforce,
            Crime = selectedWeapon.Crime
        };

        DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[(selectedWeapon.ID)].InstanceCounter++;

        var key = (newWeaponData.UserID, newWeaponData.PrototypeWeaponID, newWeaponData.InstanceID);

        if (!weaponDataDic.ContainsKey(key))
        {
            DDOManager.WeaponDatas.WeaponDatas.Add(newWeaponData);
            weaponDataDic.Add(key, newWeaponData);
            Debug.Log($"Weapon added: {newWeaponData.Name} with key ({newWeaponData.UserID}, {newWeaponData.PrototypeWeaponID}, {newWeaponData.InstanceID})");
        }
        else
        {
            Debug.LogError($"Weapon with the same key already exists: ({newWeaponData.UserID}, {newWeaponData.PrototypeWeaponID}, {newWeaponData.InstanceID})");
        }

        // 선택된 무기 UI 비활성화
        if (index >= 0 && index < newWeaponParent.childCount)
        {
            newWeaponParent.GetChild(index).gameObject.SetActive(false);
        }

        // 버튼 리스너 업데이트
        for (int i = 0; i < newWeaponParent.childCount; i++)
        {
            var button = newWeaponParent.GetChild(i).GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            int newIndex = i; // 인덱스를 새로 설정
            button.onClick.AddListener(() => OnChooseButtonClick(newIndex));
        }

        GenerateHaveWeaponDatas();
    }

}
