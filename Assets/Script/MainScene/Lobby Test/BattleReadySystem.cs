using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BattleReadySystem;
using static UnityEngine.UI.CanvasScaler;

public class BattleReadySystem : MonoBehaviour
{
    public int stageid = 0;
    public GameObject BattleReadyPrisonerPrefab;
    public GameObject BattleReadyWeaponPrefab;

    public Transform BattleReadyPrisonerParent;
    public Transform BattleReadyWeaponParent;

    public Sprite[] headSprites;
    //public Sprite[] bodySprites;

    public GameObject chooseManager;
    public GameObject[] battleReadyPrisonerUI = new GameObject[4];

    private UnitData selectedManagerUnit;
    private UnitData[] battleReadyPrisoners = new UnitData[4];

    [System.Serializable]
    public class WeaponSlot
    {
        public Button button;
        public int equipableState = -1;
        public int instanceID = -1;
        public string weaponName;
        public bool isCheck = false;
    }

    public WeaponSlot[] weaponSlots = new WeaponSlot[5];

    [System.Serializable]
    public class CrimeSlot
    {
        public TextMeshProUGUI crimeText;
        public int crimeCount;
    }
    public CrimeSlot[] crimeSlots = new CrimeSlot[7];

    public TextMeshProUGUI stageName;
    public TextMeshProUGUI stageProgress;
    public Image monsterImage;
    public Button stageEnterButton;
    public int lastCheckedDate = -1;

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
    private void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if(ddo.name == "GameManager")
            {
                GameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
            DDO = null;
        }

        if (stageEnterButton != null)
        {
            stageEnterButton.onClick.AddListener(OnStageEnterButtonClicked);
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

            return unit != null && unit.ActivityStatus == 0;
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

        if(unit.PrototypeUnitID == 100)
        {
            if (unit.HeadID >= 0 && unit.HeadID < headSprites.Length)
            {
                unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = headSprites[unit.HeadID];
            }

            if (unit.BodyID >= 0 && unit.BodyID < GameManager.PrisonerBodyImg.Count)//bodySprites.Length)
            {
                unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = GameManager.PrisonerBodyImg[unit.BodyID];
            }
        }
        else
        {
            unitUI.transform.Find("HeadImage").GetComponent<Image>().sprite = GameManager.GuiderHeadImg[unit.HeadID];
            unitUI.transform.Find("BodyImage").GetComponent<Image>().sprite = GameManager.GuiderBodyImg[unit.BodyID];
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
            weaponSlots[0].isCheck = true;
            if (chooseManager != null)
            {
                TextMeshProUGUI managerNameText = chooseManager.transform.Find("ManagerNameText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerLevelText = chooseManager.transform.Find("ManagerLevelText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerHealthText = chooseManager.transform.Find("ManagerHealthText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerStrengthText = chooseManager.transform.Find("ManagerStrengthText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI ManagerDefenseText = chooseManager.transform.Find("ManagerDefenseText").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI managerCrimeText = chooseManager.transform.Find("ManagerCrimeText").GetComponent<TextMeshProUGUI>();
                
                Image weaponImage = chooseManager.transform.Find("WeaponImage/Image").GetComponent<Image>();
                TextMeshProUGUI weaponNameText = chooseManager.transform.Find("WeaponImage/WeaponNameText").GetComponent<TextMeshProUGUI>();

                Image bodyImage = chooseManager.transform.Find("BodyImage").GetComponent<Image>();
                Image headImage = chooseManager.transform.Find("HeadImage").GetComponent<Image>();

                DDOManager.UnitDatas.UnitDataDic[(0, unit.PrototypeUnitID, unit.InstanceID)].ActivityStatus = 4;

                if (unit.Crime >= 0 && unit.Crime < crimeSlots.Length)
                {
                    crimeSlots[unit.Crime].crimeCount += 1;
                    crimeSlots[unit.Crime].crimeText.text = $"{crimeSlots[unit.Crime].crimeCount}";
                }

                if (managerNameText != null)
                    managerNameText.text = unit.Name;

                if (managerLevelText != null)
                    managerLevelText.text = "Lv: " + unit.Level;

                if (managerHealthText != null)
                    managerHealthText.text = "HP: " + unit.HealthPoint + " / " + unit.MaxHealthPoint;

                if (managerStrengthText != null)
                    managerStrengthText.text = "STR: " + unit.Strength;

                if(ManagerDefenseText != null)
                    ManagerDefenseText.text = "DEF: " + unit.Defense;

                if (managerCrimeText != null)
                    managerCrimeText.text = "Crime: " + GetCrimeDescription(unit.Crime);

                if (unit.BodyID >= 0 && unit.BodyID < GameManager.GuiderBodyImg.Count && bodyImage != null)
                {
                    bodyImage.sprite = GameManager.GuiderBodyImg[unit.BodyID];
                }

                if (unit.HeadID >= 0 && unit.HeadID < GameManager.GuiderHeadImg.Count && headImage != null)
                {
                    headImage.sprite = GameManager.GuiderHeadImg[unit.HeadID];
                }

                if (weaponNameText != null)
                {
                    string weaponName = "무기 없음";

                    if (!string.IsNullOrEmpty(weaponSlots[0].weaponName))
                    {
                        weaponName = weaponSlots[0].weaponName;
                    }

                    weaponNameText.text = weaponName;
                }

                if(weaponImage != null)
                {
                    weaponImage.gameObject.SetActive(false);
                }

                Button closeButton = chooseManager.transform.Find("CloseManagerButton").GetComponent<Button>();
                if (closeButton != null)
                {
                    closeButton.onClick.AddListener(() => returnPrisoner(unit));
                }

                Button weaponChooseButton = chooseManager.transform.Find("WeaponChooseButton").GetComponent<Button>();
                if (weaponChooseButton != null)
                {
                    weaponChooseButton.onClick.AddListener(() =>
                    {
                        for (int i = 0; i < weaponSlots.Length; i++)
                        {
                            Button button = weaponSlots[i].button;
                            if (button != null)
                            {
                                if(weaponSlots[0].isCheck)
                                {
                                    int index = i;
                                    button.onClick.AddListener(() =>
                                    {
                                        for (int j = 0; j < weaponSlots.Length; j++)
                                        {
                                            if (j != index && weaponSlots[j].equipableState == -10)
                                            {
                                                weaponSlots[j].equipableState = -1;
                                                weaponSlots[j].instanceID = -1;
                                                Debug.Log($"슬롯 {j} 상태가 -1로 변경됨");
                                            }
                                        }

                                        if (weaponSlots[index].equipableState == -1)
                                        {
                                            weaponSlots[index].equipableState = -10;
                                            Debug.Log($"슬롯 {index} 상태가 -10으로 변경됨");
                                        }
                                    });
                                }
                            }
                        }
                    });
                }

                Button weaponReturnButton = chooseManager.transform.Find("WeaponReturnButton").GetComponent<Button>();
                if (weaponReturnButton != null)
                {
                    weaponReturnButton.onClick.AddListener(() =>
                    {
                        var keyToRemove = DDOManager.UseWeaponDatas.UseWeaponDataDic
    .FirstOrDefault(kv => kv.Value.UserID == selectedManagerUnit.UserID && kv.Value.PrototypeWeaponID == weaponSlots[0].equipableState).Key;

                        bool removedFromDic = DDOManager.UseWeaponDatas.UseWeaponDataDic.Remove(keyToRemove);
                        Debug.Log($"딕셔너리에서 제거 성공 여부: {removedFromDic}");

                        int removedFromList = DDOManager.UseWeaponDatas.UseWeaponDatas.RemoveAll(data =>
                            data.UserID == keyToRemove.Item1 && data.PrototypeWeaponID == keyToRemove.Item2);
                        Debug.Log($"리스트에서 제거된 개수: {removedFromList}");

                        weaponSlots[0].equipableState = -1;
                        weaponSlots[0].instanceID = -1;
                        weaponSlots[0].weaponName = null;

                        if (DDOManager.WeaponDatas.WeaponDataDic.TryGetValue((keyToRemove.Item1, keyToRemove.Item2, keyToRemove.Item3), out var weaponData))
                        {
                            weaponData.ActivityStatus = 0;
                        }

                        if (chooseManager != null)
                        {
                            if (weaponNameText != null)
                            {
                                weaponNameText.text = "무기 없음";
                            }
                            if(weaponImage != null)
                            {
                                weaponImage.sprite = null;
                                weaponImage.gameObject.SetActive(false);
                            }
                        }
                        DisplayBattleReadyWeapons();
                    });
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

                    UnitParticipateData newData = new UnitParticipateData
                    {
                        UserID = unit.UserID,
                        PrototypeUnitID = unit.PrototypeUnitID,
                        InstanceID = unit.InstanceID,
                        PartyID = 0,
                        Position = i+1
                    };
                    DDOManager.UnitDatas.UnitDataDic[(0, unit.PrototypeUnitID, unit.InstanceID)].ActivityStatus = 4;

                    if (unit.Crime >= 0 && unit.Crime < crimeSlots.Length)
                    {
                        crimeSlots[unit.Crime].crimeCount += 1;
                        crimeSlots[unit.Crime].crimeText.text = $"{crimeSlots[unit.Crime].crimeCount}";
                    }

                    DDOManager.UnitParticipateDatas.UnitParticipateDatas.Add(newData);
                    DDOManager.UnitParticipateDatas.UnitParticipateDataDic[(newData.UserID, newData.PrototypeUnitID, newData.InstanceID, newData.PartyID)] = newData;
                    weaponSlots[i+1].isCheck = true;
                    UpdateBattleReadyUI(unit);
                    DisplayBattleReadyUnits();
                    return;
                }
            }
        }
        DisplayBattleReadyUnits();
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
                    TextMeshProUGUI prisonerNameText = prisonerUI.transform.Find("PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerLevelText = prisonerUI.transform.Find("PrisonerLevelText")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerHealthText = prisonerUI.transform.Find("PrisonerHealthText")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerStrengthText = prisonerUI.transform.Find("PrisonerStrengthText")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerDefenseText = prisonerUI.transform.Find("PrisonerDefenseText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI prisonerCrimeText = prisonerUI.transform.Find("PrisonerCrimeText")?.GetComponent<TextMeshProUGUI>();

                    Image weaponImage = prisonerUI.transform.Find("PrisonerImage/WeaponImage").GetComponent<Image>();
                    TextMeshProUGUI weaponNameText = prisonerUI.transform.Find("PrisonerImage/PrisonerNameText")?.GetComponent<TextMeshProUGUI>();

                    if (weaponNameText != null)
                    {
                        string weaponName = "무기 없음";  // 기본값 설정

                        if (!string.IsNullOrEmpty(weaponSlots[i+1].weaponName))
                        {
                            weaponName = weaponSlots[i+1].weaponName;
                            break;
                        }

                        weaponNameText.text = weaponName;  // UI 업데이트
                    }

                    if (prisonerNameText != null)
                        prisonerNameText.text = battleReadyPrisoners[i].Name;

                    if (prisonerLevelText != null)
                        prisonerLevelText.text = "Lv: " + battleReadyPrisoners[i].Level;

                    if (prisonerHealthText != null)
                        prisonerHealthText.text = "HP: " + battleReadyPrisoners[i].HealthPoint + " / " + battleReadyPrisoners[i].MaxHealthPoint;

                    if (prisonerStrengthText != null)
                        prisonerStrengthText.text = "STR: " + battleReadyPrisoners[i].Strength;

                    if(prisonerDefenseText != null)
                        prisonerDefenseText.text = "DEF: " + battleReadyPrisoners[i].Defense;

                    if (prisonerCrimeText != null)
                        prisonerCrimeText.text = "Crime: " + GetCrimeDescription(battleReadyPrisoners[i].Crime);

                    Image prisonerBodyImage = prisonerUI.transform.Find("BodyImage")?.GetComponent<Image>();
                    Image prisonerHeadImage = prisonerUI.transform.Find("HeadImage")?.GetComponent<Image>();

                    if (prisonerBodyImage != null && battleReadyPrisoners[i].BodyID >= 0 && battleReadyPrisoners[i].BodyID < GameManager.PrisonerBodyImg.Count /*bodySprites.Length*/)
                        prisonerBodyImage.sprite = /*bodySprites*/GameManager.PrisonerBodyImg[battleReadyPrisoners[i].BodyID];

                    if (prisonerHeadImage != null && battleReadyPrisoners[i].HeadID >= 0 && battleReadyPrisoners[i].HeadID < headSprites.Length)
                        prisonerHeadImage.sprite = headSprites[battleReadyPrisoners[i].HeadID];

                    Button closeButton = prisonerUI.transform.Find("PrisonerCloseButton")?.GetComponent<Button>();
                    if (closeButton != null)
                    {
                        int index = i;
                        closeButton.onClick.RemoveAllListeners();
                        closeButton.onClick.AddListener(() =>
                        {
                            if (battleReadyPrisoners[index] != null)
                            {
                                returnPrisoner(battleReadyPrisoners[index]);
                                Debug.Log($"슬롯 {index}의 죄수를 반환");
                            }
                        });
                    }

                    Button prisonerChooseButton = prisonerUI.transform.Find("PrisonerChooseButton")?.GetComponent<Button>();
                    if (prisonerChooseButton != null)
                    {
                        prisonerChooseButton.onClick.AddListener(() =>
                        {
                            Debug.Log($"슬롯 {i}에서 죄수 선택 버튼 클릭됨");
                            for (int i = 0; i < weaponSlots.Length; i++)
                            {
                                Button button = weaponSlots[i].button;
                                if (button != null)
                                {
                                    if (weaponSlots[i].isCheck)
                                    {
                                        int index = i;
                                        button.onClick.AddListener(() =>
                                        {
                                            Debug.Log($"슬롯 {index}의 무기 버튼 클릭됨");
                                            for (int j = 0; j < weaponSlots.Length; j++)
                                            {
                                                if (j != index && weaponSlots[j].equipableState == -10)
                                                {
                                                    weaponSlots[j].equipableState = -1;
                                                    weaponSlots[j].instanceID = -1;
                                                    Debug.Log($"슬롯 {j} 상태가 -1로 변경됨");
                                                }
                                            }

                                            if (weaponSlots[index].equipableState == -1)
                                            {
                                                weaponSlots[index].equipableState = -10;
                                                Debug.Log($"슬롯 {index} 상태가 -10으로 변경됨");
                                            }
                                        });
                                    }
                                }
                            }
                        });
                    }

                    Button prisonerReturnButton = prisonerUI.transform.Find("PrisonerReturnButton")?.GetComponent<Button>();
                    if (prisonerReturnButton != null)
                    {
                        int index = i + 1;
                        prisonerReturnButton.onClick.AddListener(() =>
                        {
                            int weaponEquipableState = weaponSlots[index].equipableState;
                            int weaponInstanceID = weaponSlots[index].instanceID;

                            if (weaponEquipableState != -1 && weaponInstanceID != -1)
                            {
                                var weaponKey = (0, weaponEquipableState, weaponInstanceID);

                                // WeaponDataDic에서 제거
                                if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(weaponKey))
                                {
                                    DDOManager.WeaponDatas.WeaponDataDic[weaponKey].ActivityStatus = 0;
                                }
                                else
                                {
                                    Debug.LogWarning($"WeaponDataDic에서 해당 키({weaponKey})를 찾을 수 없습니다.");
                                }

                                var keyToRemove = DDOManager.UseWeaponDatas.UseWeaponDataDic
                .FirstOrDefault(kv =>
                    kv.Value.UserID == 0 &&
                    kv.Value.PrototypeWeaponID == weaponEquipableState &&
                    kv.Value.InstanceID == weaponInstanceID
                ).Key;

                                Debug.Log($"찾은 키: {keyToRemove}");

                                // UseWeaponDataDic에서 존재하는지 확인 후 제거
                                if (DDOManager.UseWeaponDatas.UseWeaponDataDic.ContainsKey(keyToRemove))
                                {
                                    bool removedFromDic = DDOManager.UseWeaponDatas.UseWeaponDataDic.Remove(keyToRemove);
                                    Debug.Log($"UseWeaponDataDic에서 제거 성공 여부: {removedFromDic}");

                                    int removedFromList = DDOManager.UseWeaponDatas.UseWeaponDatas.RemoveAll(data =>
                                        data.UserID == keyToRemove.Item1 &&
                                        data.PrototypeWeaponID == keyToRemove.Item2 &&
                                        data.InstanceID == keyToRemove.Item3);
                                    Debug.Log($"UseWeaponDatas 리스트에서 제거된 개수: {removedFromList}");
                                }
                                else
                                {
                                    Debug.LogWarning($"UseWeaponDataDic에서 해당 키를 찾을 수 없습니다. (PrototypeWeaponID: {weaponEquipableState}, InstanceID: {weaponInstanceID})");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"유효하지 않은 무기 데이터: equipableState = {weaponEquipableState}, instanceID = {weaponInstanceID}");
                            }

                            weaponSlots[index].equipableState = -1;
                            weaponSlots[index].instanceID = -1;
                            weaponSlots[index].weaponName = null;

                            if (weaponNameText != null)
                            {
                                weaponNameText.text = "무기 없음";
                            }

                            if (weaponImage != null)
                            {
                                weaponImage.sprite = null;
                                weaponImage.gameObject.SetActive(false);
                            }

                            DisplayBattleReadyWeapons();
                        });
                    }

                }
                else
                {
                    Debug.LogWarning($"battleReadyPrisonerUI[{i}]가 null입니다.");
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
            TextMeshProUGUI managerDefenseText = chooseManager.transform.Find("ManagerDefenseText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerCrimeText = chooseManager.transform.Find("ManagerCrimeText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponNameText = chooseManager.transform.Find("WeaponImage/WeaponNameText").GetComponent<TextMeshProUGUI>();
            Image weaponImage = chooseManager.transform.Find("WeaponImage/Image").GetComponent<Image>();
            Image bodyImage = chooseManager.transform.Find("BodyImage").GetComponent<Image>();
            Image headImage = chooseManager.transform.Find("HeadImage").GetComponent<Image>();

            if (managerNameText != null) managerNameText.text = "이름";
            if (managerLevelText != null) managerLevelText.text = "레벨";
            if (managerHealthText != null) managerHealthText.text = "체력";
            if (managerStrengthText != null) managerStrengthText.text = "근력";
            if (managerDefenseText != null) managerDefenseText.text = "방어력";
            if (managerCrimeText != null) managerCrimeText.text = "범죄";
            if (weaponNameText != null) weaponNameText.text = "무기 없음";

            if(weaponImage != null)
            {
                weaponImage.sprite = null;
                weaponImage.gameObject.SetActive(false);
            }

            if (bodyImage != null && /*bodySprites.Length*/GameManager.PrisonerBodyImg.Count > 0) bodyImage.sprite = /*bodySprites*/GameManager.PrisonerBodyImg[0];
            if (headImage != null && headSprites.Length > 0) headImage.sprite = headSprites[0];

            DDOManager.UnitDatas.UnitDataDic[(0, unit.PrototypeUnitID, unit.InstanceID)].ActivityStatus = 0;
            int index = weaponSlots[0].equipableState;
            if(index != -1)
            {
                DDOManager.WeaponDatas.WeaponDataDic[(0, index, 0)].ActivityStatus = 0;
            }

            if (unit.Crime >= 0 && unit.Crime < crimeSlots.Length)
            {
                crimeSlots[unit.Crime].crimeCount -= 1;
                crimeSlots[unit.Crime].crimeText.text = $"{crimeSlots[unit.Crime].crimeCount}";
            }

            var keyToRemove = DDOManager.UseWeaponDatas.UseWeaponDataDic
    .FirstOrDefault(kv => kv.Value.UserID == selectedManagerUnit.UserID && kv.Value.PrototypeWeaponID == weaponSlots[0].equipableState).Key;

            bool removedFromDic = DDOManager.UseWeaponDatas.UseWeaponDataDic.Remove(keyToRemove);
            Debug.Log($"딕셔너리에서 제거 성공 여부: {removedFromDic}");

            int removedFromList = DDOManager.UseWeaponDatas.UseWeaponDatas.RemoveAll(data =>
                data.UserID == keyToRemove.Item1 && data.PrototypeWeaponID == keyToRemove.Item2);
            Debug.Log($"리스트에서 제거된 개수: {removedFromList}");

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
            weaponSlots[0].isCheck= false;
            weaponSlots[0].equipableState = -1;
            weaponSlots[0].instanceID = -1;
            weaponSlots[0].weaponName = null;
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

                    int index = weaponSlots[i + 1].equipableState;
                    int instanceID = weaponSlots[i + 1].instanceID;

                    if (index != -1)
                    {
                        // 무기 딕셔너리에서 활동 상태 0으로 설정
                        if (DDOManager.WeaponDatas.WeaponDataDic.ContainsKey((0, index, instanceID)))
                        {
                            DDOManager.WeaponDatas.WeaponDataDic[(0, index, instanceID)].ActivityStatus = 0;
                        }
                        else
                        {
                            Debug.LogWarning($"WeaponDataDic에 {(0, index, instanceID)} 키가 존재하지 않습니다.");
                        }
                    }

                    // UseWeaponDataDic에서 제거
                    var keyToRemove = DDOManager.UseWeaponDatas.UseWeaponDataDic
                        .FirstOrDefault(kv => kv.Value.UserID == unit.UserID && kv.Value.PrototypeWeaponID == index)
                        .Key;

                    if (DDOManager.UseWeaponDatas.UseWeaponDataDic.ContainsKey(keyToRemove))
                    {
                        bool removedFromDic = DDOManager.UseWeaponDatas.UseWeaponDataDic.Remove(keyToRemove);
                        Debug.Log($"UseWeaponDataDic에서 제거 성공 여부: {removedFromDic}");

                        int removedFromList = DDOManager.UseWeaponDatas.UseWeaponDatas.RemoveAll(data =>
                            data.UserID == keyToRemove.Item1 && data.PrototypeWeaponID == keyToRemove.Item2);
                        Debug.Log($"리스트에서 제거된 개수: {removedFromList}");
                    }

                    if (battleReadyPrisonerUI[i] != null)
                    {
                        TextMeshProUGUI prisonerNameText = battleReadyPrisonerUI[i].transform.Find("PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerLevelText = battleReadyPrisonerUI[i].transform.Find("PrisonerLevelText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerHealthText = battleReadyPrisonerUI[i].transform.Find("PrisonerHealthText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerStrengthText = battleReadyPrisonerUI[i].transform.Find("PrisonerStrengthText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerDefenseText = battleReadyPrisonerUI[i].transform.Find("PrisonerDefenseText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI prisonerCrimeText = battleReadyPrisonerUI[i].transform.Find("PrisonerCrimeText")?.GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI weaponNameText = battleReadyPrisonerUI[i].transform.Find("PrisonerImage/PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
                        Image weaponImage = battleReadyPrisonerUI[i].transform.Find("PrisonerImage/WeaponImage")?.GetComponent<Image>();
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

                        if (prisonerDefenseText != null)
                            prisonerDefenseText.text = "방어력";

                        if (prisonerCrimeText != null)
                            prisonerCrimeText.text = "범죄";

                        if (weaponNameText != null)
                            weaponNameText.text = "무기 없음";

                        if(weaponImage != null)
                        {
                            weaponImage.sprite = null;
                            weaponImage.gameObject.SetActive(false);
                        }

                        if (prisonerBodyImage != null && GameManager.PrisonerBodyImg.Count/*bodySprites.Length*/ > 0)
                            prisonerBodyImage.sprite = /*bodySprites*/GameManager.PrisonerBodyImg[0];
                        if (prisonerHeadImage != null && headSprites.Length > 0)
                            prisonerHeadImage.sprite = headSprites[0];

                        DDOManager.UnitDatas.UnitDataDic[(0, unit.PrototypeUnitID, unit.InstanceID)].ActivityStatus = 0;

                        if (unit.Crime >= 0 && unit.Crime < crimeSlots.Length)
                        {
                            crimeSlots[unit.Crime].crimeCount -= 1;
                            crimeSlots[unit.Crime].crimeText.text = $"{crimeSlots[unit.Crime].crimeCount}";
                        }

                        weaponSlots[i+1].isCheck = false;
                        weaponSlots[i + 1].equipableState = -1;
                        weaponSlots[i + 1].instanceID = -1;
                        weaponSlots[i + 1].weaponName = null;
                    }
                    break;
                }
            }
            DisplayBattleReadyUnits();
            DisplayBattleReadyWeapons();
        }
        else
        {
            Debug.Log("오류");
        }
        DisplayBattleReadyWeapons();
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

            return weapon!=null && weapon.ActivityStatus == 0;
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
        weaponUI.transform.Find("Image/WeaponImage").GetComponent<Image>().sprite = GameManager.WeaponImg[weapon.PrototypeWeaponID];

        Button chooseButton = weaponUI.transform.Find("ChooseButton").GetComponent<Button>();
        if (chooseButton != null)
        {
            chooseButton.onClick.AddListener(() =>
            {
                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (weaponSlots[i].isCheck)
                    {
                        if (weaponSlots[i].equipableState == -10)
                        {
                            weaponSlots[i].equipableState = weapon.PrototypeWeaponID;
                            weaponSlots[i].instanceID = weapon.InstanceID;
                            weaponSlots[i].weaponName = weapon.Name;

                            if (i == 0 && selectedManagerUnit != null)
                            {
                                if (chooseManager != null)
                                {
                                    TextMeshProUGUI weaponNameText = chooseManager.transform.Find("WeaponImage/WeaponNameText").GetComponent<TextMeshProUGUI>();
                                    if (weaponNameText != null)
                                    {
                                        string weaponName = "무기 없음";
                                        if (!string.IsNullOrEmpty(weaponSlots[0].weaponName))
                                        {
                                            weaponName = weaponSlots[0].weaponName;
                                        }

                                        weaponNameText.text = weaponName;
                                    }

                                    Image weponImage = chooseManager.transform.Find("WeaponImage/Image").GetComponent<Image>();
                                    if (weponImage != null)
                                    {
                                        weponImage.gameObject.SetActive(true);
                                        weponImage.sprite = GameManager.WeaponImg[weapon.PrototypeWeaponID];
                                    }
                                }

                                UseWeaponData weaponData = new UseWeaponData
                                {
                                    UserID = weapon.UserID,
                                    PrototypeWeaponID = weapon.PrototypeWeaponID,
                                    InstanceID = weapon.InstanceID,
                                    PartyID = 0,
                                    Position = 0
                                };

                                DDOManager.WeaponDatas.WeaponDataDic[(weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID)].ActivityStatus = 1;
                                DDOManager.UseWeaponDatas.UseWeaponDataDic[(weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID, weaponData.PartyID)] = weaponData;
                                DDOManager.UseWeaponDatas.UseWeaponDatas.Add(weaponData);
                            }
                            else
                            {
                                if (battleReadyPrisoners != null)
                                {
                                    Debug.Log("들어왔디롱");
                                    if (i > 0 && battleReadyPrisoners[i - 1] != null)
                                    {
                                        Debug.Log(i-1);
                                        TextMeshProUGUI battleReadyWeaponNameText = battleReadyPrisonerUI[i - 1].transform.Find("PrisonerImage/PrisonerNameText").GetComponent<TextMeshProUGUI>();

                                        if (battleReadyWeaponNameText != null)
                                        {
                                            Debug.Log("시발");
                                            string battleReadyWeaponName = "무기 없음";

                                            if (!string.IsNullOrEmpty(weaponSlots[i].weaponName))
                                            {
                                                battleReadyWeaponName = weaponSlots[i].weaponName;
                                                Debug.Log("WTF");
                                            }
                                            battleReadyWeaponNameText.text = battleReadyWeaponName;
                                        }

                                        Image weponImage = battleReadyPrisonerUI[i - 1].transform.Find("PrisonerImage/WeaponImage").GetComponent<Image>();
                                        if (weponImage != null)
                                        {
                                            weponImage.gameObject.SetActive(true);
                                            weponImage.sprite = GameManager.WeaponImg[weapon.PrototypeWeaponID];
                                        }

                                        UseWeaponData weaponData = new UseWeaponData
                                        {
                                            UserID = weapon.UserID,
                                            PrototypeWeaponID = weapon.PrototypeWeaponID,
                                            InstanceID = weapon.InstanceID,
                                            PartyID = 0,
                                            Position = i
                                        };

                                        DDOManager.WeaponDatas.WeaponDataDic[(weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID)].ActivityStatus = 1;
                                        DDOManager.UseWeaponDatas.UseWeaponDataDic[(weaponData.UserID, weaponData.PrototypeWeaponID, weaponData.InstanceID, weaponData.PartyID)] = weaponData;
                                        DDOManager.UseWeaponDatas.UseWeaponDatas.Add(weaponData);
                                    }
                                }
                            }
                        }
                    }
                    DisplayBattleReadyWeapons();
                }
            });
        }
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

    public void StageName(int index)
    {
        if (DDOManager.StageDatas != null && DDOManager.StageDatas.StageDatas.Count > index)
        {
            
            stageName.text = $"스테이지{index + 1} {DDOManager.StageDatas.StageDatas[index].Name}";
        }
    }

    public void StageProgress(int index)
    {
        if (DDOManager.ProgressDatas != null && DDOManager.ProgressDatas.ProgressDatas.Count > index)
        {
            stageProgress.text = $"진척도 {DDOManager.ProgressDatas.ProgressDatas[index].Progress}%";
        }
    }

    public void stagemonsterImage(int index)
    {
        if (DDOManager.MonsterDatas != null && DDOManager.MonsterDatas.MonsterDatas.Count > index)
        {

            GameObject monsterObject = GameManager.Monster[index];

            if (monsterObject == null)
            {
                Debug.LogError($"GameManager.Monster[{index}]가 null입니다.");
                return;
            }

            SpriteRenderer spriteRenderer = monsterObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError($"GameManager.Monster[{index}]에 SpriteRenderer 컴포넌트가 없습니다.");
                return;
            }

            monsterImage.sprite = spriteRenderer.sprite;
        }
    }

    public void setStageIndex(int index)
    {
        stageid = index;
    }

    public int getStageIndex()
    {
        return stageid;
    }

    private void OnStageEnterButtonClicked()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            int index = getStageIndex();
            GameManager.SelectStageID = index;
            Debug.Log($"선택된 스테이지 ID: {GameManager.SelectStageID}");
        }
    }
    public void CheckAndResetDungeonSystemState()
    {
        int currentDate = DDOManager.LocalUserDatas.LocalUserDataDic[0].Day;

        if (currentDate != lastCheckedDate)
        {
            ResetAllDungeonStates();
            lastCheckedDate = currentDate;
        }
    }

    private void ResetAllDungeonStates()
    {
        // 모든 슬롯 초기화
        foreach (var slot in weaponSlots)
        {
            slot.equipableState = -1;
            slot.instanceID = -1;
            slot.weaponName = null;
            slot.isCheck = false;
        }

        foreach (var slot in crimeSlots)
        {
            slot.crimeCount = 0;
            if (slot.crimeText != null)
            {
                slot.crimeText.text = "0";
            }
        }

        if (selectedManagerUnit != null)
        {
            TextMeshProUGUI managerNameText = chooseManager.transform.Find("ManagerNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerLevelText = chooseManager.transform.Find("ManagerLevelText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerHealthText = chooseManager.transform.Find("ManagerHealthText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerStrengthText = chooseManager.transform.Find("ManagerStrengthText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerDefenseText = chooseManager.transform.Find("ManagerDefenseText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI managerCrimeText = chooseManager.transform.Find("ManagerCrimeText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponNameText = chooseManager.transform.Find("WeaponImage/WeaponNameText").GetComponent<TextMeshProUGUI>();
            Image weaponImage = chooseManager.transform.Find("WeaponImage/Image").GetComponent<Image>();
            Image bodyImage = chooseManager.transform.Find("BodyImage").GetComponent<Image>();
            Image headImage = chooseManager.transform.Find("HeadImage").GetComponent<Image>();

            if (managerNameText != null) managerNameText.text = "이름";
            if (managerLevelText != null) managerLevelText.text = "레벨";
            if (managerHealthText != null) managerHealthText.text = "체력";
            if (managerStrengthText != null) managerStrengthText.text = "근력";
            if (managerDefenseText != null) managerDefenseText.text = "방어력";
            if (managerCrimeText != null) managerCrimeText.text = "범죄";
            if (weaponNameText != null) weaponNameText.text = "무기 없음";
            if(weaponImage != null)
            {
                weaponImage.sprite = null;
                weaponImage.gameObject.SetActive(false);
            }
            if (bodyImage != null && /*bodySprites.Length*/GameManager.PrisonerBodyImg.Count > 0) bodyImage.sprite = /*bodySprites*/GameManager.PrisonerBodyImg[0];
            if (headImage != null && headSprites.Length > 0) headImage.sprite = headSprites[0];
        }

        for(int i = 0; i <  battleReadyPrisoners.Length; i++)
        {
            TextMeshProUGUI prisonerNameText = battleReadyPrisonerUI[i].transform.Find("PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prisonerLevelText = battleReadyPrisonerUI[i].transform.Find("PrisonerLevelText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prisonerHealthText = battleReadyPrisonerUI[i].transform.Find("PrisonerHealthText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prisonerStrengthText = battleReadyPrisonerUI[i].transform.Find("PrisonerStrengthText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prisonerDefenseText = battleReadyPrisonerUI[i].transform.Find("PrisonerDefenseText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prisonerCrimeText = battleReadyPrisonerUI[i].transform.Find("PrisonerCrimeText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI weaponNameText = battleReadyPrisonerUI[i].transform.Find("PrisonerImage/PrisonerNameText")?.GetComponent<TextMeshProUGUI>();
            Image weaponImage = battleReadyPrisonerUI[i].transform.Find("PrisonerImage/WeaponImage").GetComponent<Image>();
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

            if (prisonerDefenseText != null)
                prisonerDefenseText.text = "방어력";

            if (prisonerCrimeText != null)
                prisonerCrimeText.text = "범죄";

            if (weaponNameText != null)
                weaponNameText.text = "무기 없음";

            if (weaponImage != null)
            {
                weaponImage.sprite = null;
                weaponImage.gameObject.SetActive(false);
            }
            if (prisonerBodyImage != null && GameManager.PrisonerBodyImg.Count/*bodySprites.Length*/ > 0)
                prisonerBodyImage.sprite = /*bodySprites*/GameManager.PrisonerBodyImg[0];
            if (prisonerHeadImage != null && headSprites.Length > 0)
                prisonerHeadImage.sprite = headSprites[0];
        }

        // 모든 딕셔너리와 리스트 초기화
        DDOManager.UseWeaponDatas.UseWeaponDataDic.Clear();
        DDOManager.UseWeaponDatas.UseWeaponDatas.Clear();
        DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Clear();
        DDOManager.UnitParticipateDatas.UnitParticipateDatas.Clear();

        selectedManagerUnit = null;
        for (int i = 0; i < battleReadyPrisoners.Length; i++)
        {
            battleReadyPrisoners[i] = null;
        }

        foreach (var weapon in DDOManager.WeaponDatas.WeaponDataDic.Values)
        {
            if(weapon.ActivityStatus == 1)
            {
                weapon.ActivityStatus = 0;
            }
        }

        foreach (var unit in DDOManager.UnitDatas.UnitDataDic.Values)
        {
            if (unit.ActivityStatus == 4)
            {
                unit.ActivityStatus = 0;
            }
        }
    }
}