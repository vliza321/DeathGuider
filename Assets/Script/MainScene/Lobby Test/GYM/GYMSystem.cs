using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GYMSystem : MonoBehaviour
{
    public FloorSystem floorSystem;

    public GameObject GYMUnitUIPrefab;
    public GameObject GYMTrainingPrefab;

    public Transform contentUnitParent;
    public Transform contentTrainParent;
    //public Sprite[] headSprites;
    //public Sprite[] bodySprites;

    public int trainingRoomCount = 3;
    public int maxTrainingSlots = 3;
    public int lastCheckedDate = -1;

    public List<RoomData> TrainDatas = new List<RoomData>();

    [System.Serializable]
    public class RoomData
    {
        public List<GYMData> GYMDataList = new List<GYMData>();
        public Transform roomTransform;
        public string name;
    }

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;

    private void Start()
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
                GameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        for (int i = 0; i < trainingRoomCount; i++)
        {
            RoomData roomData = new RoomData();

            for (int j = 0; j < 3; j++)
            {
                roomData.GYMDataList.Add(new GYMData(-1));
            }

            TrainDatas.Add(roomData);
        }
    }

    public void DisplayGYMPrisoners()
    {
        foreach (Transform child in contentUnitParent)
        {
            Destroy(child.gameObject);
        }

        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            return;
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas
            .Where(prisoner => prisoner != null && prisoner.ActivityStatus == 0 && prisoner.PrototypeUnitID == 100 && prisoner.Enforce >= 0 && prisoner.Enforce < 15)
            .OrderByDescending(prisoner => prisoner.Enforce)
            .ThenBy(prisoner => prisoner.InstanceID)
            .ToList();

        if (filteredPrisoners.Count > 0)
        {
            int prisonerIndex = 1;
            foreach (var prisoner in filteredPrisoners)
            {
                GameObject prisonerUI = CreateGYMPrisonerUI(prisoner);
                prisonerUI.name = $"Gym Prisoner{prisonerIndex}";
                prisonerIndex++;
            }
        }

        RectTransform contentRect = contentUnitParent.GetComponent<RectTransform>();

        GridLayoutGroup gridLayoutGroup = contentUnitParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;
        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        float newHeight = (cellHeight + spacingY) * filteredPrisoners.Count - spacingY + paddingUp;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    private GameObject CreateGYMPrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(GYMUnitUIPrefab, contentUnitParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("EXPText").GetComponent<TextMeshProUGUI>().text = $"LV_EXP: {prisoner.EXP}";
        prisonerUI.transform.Find("EnforceCountText").GetComponent<TextMeshProUGUI>().text = $"강화 횟수: {prisoner.Enforce} / 15";
        prisonerUI.transform.Find("HealthEnforceText").GetComponent<TextMeshProUGUI>().text = $"HP 강화: {prisoner.HealthEnforce}";
        prisonerUI.transform.Find("StrengthEnforceText").GetComponent<TextMeshProUGUI>().text = $"STR 강화: {prisoner.StrengthEnforce}";
        prisonerUI.transform.Find("DefenseEnforceText").GetComponent<TextMeshProUGUI>().text = $"DEF 강화: {prisoner.DefenseEnforce}";
        prisonerUI.transform.Find("HandicraftEnforceText").GetComponent<TextMeshProUGUI>().text = $"HCT 강화: {prisoner.HandicraftEnforce}";

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

        Button chooseButton = prisonerUI.transform.Find("ChooseButton").GetComponent<Button>();
        if (chooseButton != null)
        {
            chooseButton.onClick.AddListener(() =>
            {
                foreach (var roomData in TrainDatas)
                {
                    foreach (var gymData in roomData.GYMDataList)
                    {
                        if (gymData.InstanceID == -10)
                        {
                            GameObject trainingUI = Instantiate(GYMTrainingPrefab, contentTrainParent);
                            Transform prisonerTransform = trainingUI.transform;

                            Transform gymTrainingImage = prisonerTransform.Find("GymTrainingImage");
                            Transform trainingSelect = FindTrainingSelectForData(roomData, roomData.GYMDataList.IndexOf(gymData));

                            gymData.InstanceID = DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID,prisoner.PrototypeUnitID,prisoner.InstanceID)].InstanceID;
                            UpdateRoomInstanceAndUI(gymData.InstanceID, trainingSelect);
                            Destroy(prisonerUI);
                        }
                    }
                }
            });
        }

        return prisonerUI;
    }

    public void generateTraingRoom()
    {
        RoomData newRoom = new RoomData();
        newRoom.GYMDataList.Add(new GYMData(-1));
        TrainDatas.Add(newRoom);
    }

    public void DisplayGYMTrainingUI()
    {
        foreach (Transform child in contentTrainParent)
        {
            Destroy(child.gameObject);
        }

        RectTransform contentRect = contentTrainParent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = contentTrainParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;
        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;
        float newHeight = (cellHeight + spacingY) * trainingRoomCount - spacingY + paddingUp;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        for (int i = 0; i < TrainDatas.Count; i++)
        {
            GameObject trainingUI = Instantiate(GYMTrainingPrefab, contentTrainParent);
            trainingUI.name = $"TrainingRoom{i + 1}";
            Transform prisonerTransform = trainingUI.transform;

            RoomData room = TrainDatas[i];
            room.roomTransform = prisonerTransform;

            Transform trainingNumberText = prisonerTransform.Find("TraningNumberText");
            if (trainingNumberText != null)
            {
                trainingNumberText.GetComponent<TextMeshProUGUI>().text = $"훈련 {i + 1}";
            }

            Transform trainingInfoText = prisonerTransform.Find("TraningInfoText");
            if (trainingInfoText != null)
            {
                string trainingInfo = string.Empty;

                if (i == 0)
                {
                    trainingInfo = "최대 체력, 근력";
                }
                else if (i == 1)
                {
                    trainingInfo = "숙련도";
                }
                else if (i == 2)
                {
                    trainingInfo = "최대 체력, 방어력";
                }

                trainingInfoText.GetComponent<TextMeshProUGUI>().text = trainingInfo;
            }

            for (int j = 1; j <= 3; j++)
            {
                Transform gymTrainingImage = prisonerTransform.Find("GymTrainingImage");

                Transform trainingSelect = FindTrainingSelectForData(room, j - 1);
                if (trainingSelect != null)
                {
                    int roomIndex = i;
                    int buttonIndex = j - 1;
                    GYMData clickedData = room.GYMDataList[buttonIndex];
                    Button button = trainingSelect.Find("TrainChooseButton").GetComponent<Button>();
                    Transform textImage = trainingSelect.Find("TextImage");
                    Transform trainStateText = textImage.Find("TrainStateText");

                    if (clickedData.check)
                    {
                        button.gameObject.SetActive(false);
                        trainStateText.GetComponent<TextMeshProUGUI>().text = "선택 완료";
                    }
                    else
                    {
                        button.onClick.AddListener(() =>
                        {
                            UpdateOtherRoomsData(-10, -1, contentTrainParent, roomIndex, buttonIndex);

                            if (clickedData.InstanceID < 0)
                            {
                                clickedData.InstanceID = -10;
                            }

                            UpdateAllTrainStateToWaiting(contentTrainParent, roomIndex, buttonIndex);

                            button.gameObject.SetActive(false);
                        });
                    }


                    Button okayButton = trainingSelect.Find("TrainingOkayButton").GetComponent<Button>();
                    okayButton.onClick.AddListener(() =>
                    {
                        GYMData clickedData = room.GYMDataList[buttonIndex];
                        Debug.Log(clickedData.InstanceID);
                        if (clickedData.InstanceID < 0)
                        {
                            return;
                        }

                        clickedData.check = true;

                        int activityStatus = -1;
                        if (roomIndex == 0)
                        {
                            activityStatus = 11;
                        }
                        else if (roomIndex == 1)
                        {
                            activityStatus = 12;
                        }
                        else if (roomIndex == 2)
                        {
                            activityStatus = 13;
                        }

                        if (activityStatus != -1)
                        {
                            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, clickedData.InstanceID)].ActivityStatus = activityStatus;
                        }

                        if (textImage != null)
                        {
                            if (trainStateText != null)
                            {
                                trainStateText.GetComponent<TextMeshProUGUI>().text = "선택 완료";
                            }
                        }
                    });

                    if (room.GYMDataList[buttonIndex].InstanceID >= 0)
                    {
                        int instanceID = room.GYMDataList[buttonIndex].InstanceID;
                        UpdateRoomInstanceAndUI(instanceID, trainingSelect);
                    }
                }
            }
        }
    }

    private void UpdateRoomInstanceAndUI(int instanceID, Transform trainingSelect)
    {
        if (instanceID >= 0)
        {
            var unitData = DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, instanceID)];

            // HeadID와 BodyID 업데이트
            int headID = unitData.HeadID;
            int bodyID = unitData.BodyID;

            Transform headImage = trainingSelect.Find("HeadImage");
            if (headImage != null)
            {
                if (headID >= 0 && headID < GameManager.PrisonerHeadImg.Count)
                {
                    headImage.GetComponent<Image>().sprite = GameManager.PrisonerHeadImg[headID];
                    headImage.gameObject.SetActive(true);
                }
            }

            Transform bodyImage = trainingSelect.Find("BodyImage");
            if (bodyImage != null)
            {
                if (bodyID >= 0 && bodyID < GameManager.PrisonerBodyImg.Count)
                {
                    bodyImage.GetComponent<Image>().sprite = GameManager.PrisonerBodyImg[bodyID];
                    bodyImage.gameObject.SetActive(true);
                }
            }

            Transform textImage = trainingSelect.Find("TextImage");
            if (textImage != null)
            {
                Transform nameText = textImage.Find("NameText");
                if (nameText != null)
                {
                    nameText.GetComponent<TextMeshProUGUI>().text = unitData.Name;
                }
            }
        }
    }

    public void UpdateAllTrainStateToWaiting(Transform parentTransform, int selectedRoomIndex, int selectedButtonIndex)
    {
        for (int i = 1; i <= 3; i++)
        {
            Transform trainingRoom = parentTransform.Find($"TrainingRoom{i}");
            if (trainingRoom != null)
            {
                Transform gymTrainingImage = trainingRoom.Find("GymTrainingImage");
                if (gymTrainingImage != null)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        Transform trainingSelect = gymTrainingImage.Find($"TrainingSelect{j}");
                        if (trainingSelect != null)
                        {
                            Transform textImage = trainingSelect.Find("TextImage");
                            if (textImage != null)
                            {
                                Transform trainStateText = textImage.Find("TrainStateText");
                                if (trainStateText != null)
                                {
                                    TextMeshProUGUI textComponent = trainStateText.GetComponent<TextMeshProUGUI>();

                                    if (textComponent.text != "선택 완료")
                                    {
                                        if (i == (selectedRoomIndex + 1) && j - 1 == selectedButtonIndex)
                                        {
                                            textComponent.text = "선택 중";
                                        }
                                        else
                                        {
                                            textComponent.text = "대기 상태";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateOtherRoomsData(int oldValue, int newValue, Transform parentTransform, int roomIndex, int buttonIndex)
    {
        foreach (RoomData r in TrainDatas)
        {
            foreach (GYMData data in r.GYMDataList)
            {
                if (data.InstanceID == oldValue)
                {
                    data.InstanceID = newValue;

                    Transform trainingSelect = FindTrainingSelectForData(r, TrainDatas.IndexOf(r));
                    if (trainingSelect != null)
                    {
                        UpdateTrainStateText(trainingSelect, false);
                    }
                }
            }
        }

        for (int k = 1; k <= 3; k++)
        {
            Transform trainingRoom = parentTransform.Find($"TrainingRoom{k}");
            if (trainingRoom != null)
            {
                Transform gymTrainingImage = trainingRoom.Find("GymTrainingImage");
                if (gymTrainingImage != null)
                {
                    for (int l = 1; l <= 3; l++)
                    {
                        Transform trainingSelect = gymTrainingImage.Find($"TrainingSelect{l}");
                        GYMData clickedData = TrainDatas[roomIndex].GYMDataList[buttonIndex];
                        GYMData clickedData2 = TrainDatas[k-1].GYMDataList[l-1];

                        if (trainingSelect != null)
                        {
                            Transform trainChooseButton = trainingSelect.Find("TrainChooseButton");
                            if (trainChooseButton != null)
                            {
                                Button button = trainChooseButton.GetComponent<Button>();
                                if (button != null)
                                {
                                    if (clickedData2.check == true)
                                    {
                                        button.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        button.gameObject.SetActive(true);
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private Transform FindTrainingSelectForData(RoomData room, int buttonIndex)
    {
    Transform gymTrainingImage = room.roomTransform.Find("GymTrainingImage");
    return gymTrainingImage?.Find($"TrainingSelect{buttonIndex + 1}");
    }

    public void UpdateTrainStateText(Transform trainingSelect, bool isSelected)
    {
        Transform textImage = trainingSelect.Find("TextImage");
        if (textImage != null)
        {
            Transform trainStateText = textImage.Find("TrainStateText");
            if (trainStateText != null)
            {
                TextMeshProUGUI trainStateTextComponent = trainStateText.GetComponent<TextMeshProUGUI>();

                if (isSelected)
                {
                    trainStateTextComponent.text = "선택 중";
                }
                else
                {
                    trainStateTextComponent.text = "대기 상태";
                }
            }
        }
    }

    public void CheckAndResetGYMSystemState()
    {
        int currentDate = DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Day;

        if (currentDate != lastCheckedDate)
        {
            ResetAllTrainingStates();
            lastCheckedDate = currentDate;
        }
    }

    private void ResetAllTrainingStates()
    {
        foreach (var unit in DDOManager.UnitDatas.UnitDataDic.Values)
        {
            int expGrowthRate = 2;
            int expGain = 10 + (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].GYMEnhance * expGrowthRate);

            if (unit.ActivityStatus == 11)
            {
                unit.StrengthEnforce++;
                unit.HealthEnforce++;
                unit.MaxHealthPoint += 6;
                unit.HealthPoint += 6;
                unit.Strength += 2;
                unit.Enforce++;
                unit.ActivityStatus = 0;
            }
            else if (unit.ActivityStatus == 12)
            {
                unit.HandicraftEnforce++;
                unit.Handicraft += 2;
                unit.Enforce++;
                unit.ActivityStatus = 0;
            }
            else if (unit.ActivityStatus == 13)
            {
                unit.DefenseEnforce++;
                unit.HealthEnforce++;
                unit.MaxHealthPoint += 6;
                unit.HealthPoint += 6;
                unit.DefenseEnforce += 2;
                unit.Enforce++;
                unit.ActivityStatus = 0;
            }

            unit.EXP += expGain;
            if (unit.EXP > 100)
            {
                unit.EXP = 100;
            }
        }

        //foreach (var roomData in TrainDatas)
        //{
        //    foreach (var gymData in roomData.GYMDataList)
        //    {
        //        if (gymData.InstanceID == -1)
        //            continue;

        //        var unitData = DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)];

        //        int expGrowthRate = 2;
        //        int expGain = 10 + (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].GYMEnhance * expGrowthRate);

        //        if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].ActivityStatus == 11)
        //        {
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].StrengthEnforce++;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].HealthEnforce++;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].MaxHealthPoint += 6;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].HealthPoint += 6;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].Strength += 2;
        //        }
        //        else if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].ActivityStatus == 12)
        //        {
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].HandicraftEnforce++;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].Handicraft += 2;
        //        }
        //        else if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].ActivityStatus == 13)
        //        {
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].DefenseEnforce++;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].HealthEnforce++;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].MaxHealthPoint += 6;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].HealthPoint += 6;
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].DefenseEnforce += 2;
        //        }

        //        DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].EXP += expGain;
        //        if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].EXP > 100)
        //        {
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].EXP = 100;
        //        }

        //        DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].ActivityStatus = 0;
        //        DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, gymData.InstanceID)].Enforce++;
        //        gymData.InstanceID = -1;
        //        gymData.check = false;
        //    }
        //}
    }

}
