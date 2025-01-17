using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class GYMSystem : MonoBehaviour
{
    public FloorSystem floorSystem;

    public GameObject GYMUnitUIPrefab;
    public GameObject GYMTrainingPrefab;

    public Transform contentUnitParent;
    public Transform contentTrainParent;
    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    public int trainingRoomCount = 3;


    [SerializeField]
    private List<RoomData> TrainDatas = new List<RoomData>();

    [System.Serializable]
    public class RoomData
    {
        public List<GYMData> GYMDataList = new List<GYMData>();
        public Transform roomTransform;
        public string name;
    }

    private DontDestroyObjectManager DDOManager;

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
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas
            .Where(prisoner => prisoner != null && prisoner.PrototypeUnitID == 100 && prisoner.Enforce >= 0 && prisoner.Enforce <= 15)
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

                Debug.Log($"PrototypeUnitID 100, Enforce >= 1: {prisoner.Name}, Enforce: {prisoner.Enforce}, InstanceID: {prisoner.InstanceID}");

                prisonerIndex++;
            }
        }
        else
        {
            Debug.LogWarning("조건에 맞는 죄수 데이터가 없습니다.");
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

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");
    }


    private GameObject CreateGYMPrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(GYMUnitUIPrefab, contentUnitParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("EXPText").GetComponent<TextMeshProUGUI>().text = $"LV_EXP: {prisoner.EXP}";
        prisonerUI.transform.Find("EnforceCountText").GetComponent<TextMeshProUGUI>().text = $"Cnt_EXP: {prisoner.Enforce}";
        prisonerUI.transform.Find("HealthEnforceText").GetComponent<TextMeshProUGUI>().text = $"HP_EXP: {prisoner.HealthEnforce}";
        prisonerUI.transform.Find("StrengthEnforceText").GetComponent<TextMeshProUGUI>().text = $"STR_EXP: {prisoner.StrengthEnforce}";
        prisonerUI.transform.Find("DefenseEnforceText").GetComponent<TextMeshProUGUI>().text = $"DEF_EXP: {prisoner.DefenseEnforce}";
        prisonerUI.transform.Find("HandicraftEnforceText").GetComponent<TextMeshProUGUI>().text = $"HCT_EXP: {prisoner.HandicraftEnforce}";

        Image headImage = prisonerUI.transform.Find("HeadImage").GetComponent<Image>();
        if (prisoner.HeadID >= 0 && prisoner.HeadID < headSprites.Length)
        {
            headImage.sprite = headSprites[prisoner.HeadID];
        }
        else
        {
            Debug.LogWarning($"Invalid HeadID: {prisoner.HeadID}");
        }

        Image bodyImage = prisonerUI.transform.Find("BodyImage").GetComponent<Image>();
        if (prisoner.BodyID >= 0 && prisoner.BodyID < bodySprites.Length)
        {
            bodyImage.sprite = bodySprites[prisoner.BodyID];
        }
        else
        {
            Debug.LogWarning($"Invalid BodyID: {prisoner.BodyID}");
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
                            gymData.InstanceID = DDOManager.UnitDatas.UnitDataDic[(0, 100, prisoner.InstanceID)].InstanceID;
                            Debug.Log($"Room에서 InstanceID가 -10인 값이 인스턴스 아이디로 변경되었습니다.");
                            prisoner.PrototypeUnitID = -1;
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

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");

        // Training room UI 생성
        for (int i = 0; i < TrainDatas.Count; i++)
        {
            GameObject trainingUI = Instantiate(GYMTrainingPrefab, contentTrainParent);
            trainingUI.name = $"TrainingRoom{i + 1}";
            Transform prisonerTransform = trainingUI.transform;

            RoomData room = TrainDatas[i];
            room.roomTransform = prisonerTransform;

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
                                Debug.Log($"Room {roomIndex}: 선택된 값이 -10으로 변경되었습니다.");
                            }
                            else
                            {
                                Debug.Log($"Room {roomIndex}: InstanceID가 0 이상인 값은 변경되지 않습니다.");
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
                            Debug.Log("InstanceID가 음수이므로 클릭할 수 없습니다.");
                            return;
                        }

                        clickedData.check = true;

                        int instanceID = clickedData.InstanceID;

                        DDOManager.UnitDatas.UnitDataDic[(0, 100, instanceID)].ActivityStatus = 1;

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

                        int headID = DDOManager.UnitDatas.UnitDataDic[(0, 100, instanceID)].HeadID;
                        int bodyID = DDOManager.UnitDatas.UnitDataDic[(0, 100, instanceID)].BodyID;

                        Transform headImage = trainingSelect.Find("HeadImage");
                        if (headImage != null)
                        {
                            if (headID >= 0 && headID < headSprites.Length)
                            {
                                headImage.GetComponent<Image>().sprite = headSprites[headID];
                                headImage.gameObject.SetActive(true);
                            }
                            else
                            {
                                Debug.LogWarning($"Invalid HeadID: {headID}");
                            }
                        }

                        Transform bodyImage = trainingSelect.Find("BodyImage");
                        if (bodyImage != null)
                        {
                            if (bodyID >= 0 && bodyID < bodySprites.Length)
                            {
                                bodyImage.GetComponent<Image>().sprite = bodySprites[bodyID];
                                bodyImage.gameObject.SetActive(true);
                            }
                            else
                            {
                                Debug.LogWarning($"Invalid BodyID: {bodyID}");
                            }
                        }
                    }
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
        // 1. 먼저 TrainDatas에서 각 GYMData의 InstanceID를 업데이트
        foreach (RoomData r in TrainDatas)
        {
            foreach (GYMData data in r.GYMDataList)
            {
                if (data.InstanceID == oldValue)
                {
                    data.InstanceID = newValue;
                    Debug.Log($"Room {r}: {oldValue}이(가) {newValue}로 변경되었습니다.");

                    Transform trainingSelect = FindTrainingSelectForData(r, TrainDatas.IndexOf(r));
                    if (trainingSelect != null)
                    {
                        UpdateTrainStateText(trainingSelect, false);
                    }
                }
            }
        }

        // 3. 각 room의 버튼 상태를 업데이트
        for (int k = 1; k <= 3; k++) // TrainingRoom 1~3까지 반복
        {
            Transform trainingRoom = parentTransform.Find($"TrainingRoom{k}");
            if (trainingRoom != null)
            {
                Transform gymTrainingImage = trainingRoom.Find("GymTrainingImage");
                if (gymTrainingImage != null)
                {
                    // 4. TrainingSelect 1~3까지 반복
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
}
