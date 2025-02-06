using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErosionSystem : MonoBehaviour
{
    public GameObject ErosionUnitUIPrefab;
    public GameObject ErosionRoomPrefab;

    public Transform contentUnitParent;
    public Transform contentRoomParent;

    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    public int ErosionRoomCount = 1;
    public int lastCheckedDate = -1;
    public List<ErosionData> ErosionDataList = new List<ErosionData>();

    [System.Serializable]
    public class ErosionData
    {
        public int InstanceID;

        public ErosionData()
        {
            InstanceID = -1;
        }
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
        }

        for (int i = 0; i < ErosionRoomCount; i++)
        {
            ErosionData newErosionData = new ErosionData(); // InstanceID는 0부터 시작
            ErosionDataList.Add(newErosionData);
        }
    }

    public void DisplayErosionPrisoners()
    {
        foreach (Transform child in contentUnitParent)
        {
            Destroy(child.gameObject);
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas
    .Where(prisoner => prisoner != null && prisoner.ActivityStatus == 0 && prisoner.PrototypeUnitID == 100 && prisoner.DeathErosion != 0)
    .OrderBy(prisoner => prisoner.InstanceID)
    .ToList();

        Debug.Log(filteredPrisoners.Count);
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

        if (filteredPrisoners.Count > 0)
        {
            int prisonerIndex = 0; // 초기화된 인덱스 사용
            foreach (var prisoner in filteredPrisoners)
            {
                GameObject prisonerUI = contentUnitParent.Find($"Erosion Prisoner{prisonerIndex + 1}")?.gameObject;
                prisonerUI = CreateErosionPrisonerUI(prisoner);
                prisonerUI.name = $"Erosion Prisoner{prisonerIndex + 1}";
                UpdateErosionPrisonerUI(prisonerUI, prisoner);
                prisonerIndex++;
            }
        }
        else
        {
            Debug.LogWarning("조건에 맞는 죄수 데이터가 없습니다.");
        }
    }

    private void UpdateErosionPrisonerUI(GameObject prisonerUI, UnitData prisoner)
    {
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("ErosionText").GetComponent<TextMeshProUGUI>().text = $"Erosion: {prisoner.DeathErosion} / 100";

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
    }

    private GameObject CreateErosionPrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(ErosionUnitUIPrefab, contentUnitParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("ErosionText").GetComponent<TextMeshProUGUI>().text = $"Erosion: {prisoner.DeathErosion} / 100";

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
                bool roomUpdated = false;

                for (int i = 0; i < ErosionRoomCount; i++)
                {
                    Transform targetErosionRoom = contentRoomParent.Find("ErosionRoom" + (i + 1));
                    if (targetErosionRoom != null)
                    {
                        if (ErosionDataList[i].InstanceID >= 0)
                        {
                            continue;
                        }

                        ErosionDataList[i].InstanceID = prisoner.InstanceID;
                        Transform prisonerImageTransform = targetErosionRoom.Find("PrisonerImage");
                        if (prisonerImageTransform != null)
                        {
                            Transform prisonerNameTransform = prisonerImageTransform.Find("NameText");
                            if (prisonerNameTransform != null)
                            {
                                TextMeshProUGUI nameText = prisonerNameTransform.GetComponent<TextMeshProUGUI>();
                                if (nameText != null)
                                {
                                    nameText.text = prisoner.Name;
                                }
                            }
                            Transform prisonerHPTransform = prisonerImageTransform.Find("ErosionText");
                            if (prisonerHPTransform != null)
                            {
                                TextMeshProUGUI prisonerHPText = prisonerHPTransform.GetComponent<TextMeshProUGUI>();
                                if (prisonerHPText != null)
                                {
                                    prisonerHPText.text = $"Erosion: {prisoner.DeathErosion} / 100";
                                }
                            }

                            Slider HPslideBar = targetErosionRoom.Find("ErosionSlider").GetComponent<Slider>();
                            if (HPslideBar != null)
                            {

                                HPslideBar.maxValue = 100;
                                HPslideBar.value = prisoner.DeathErosion;
                            }
                            else
                            {
                                Debug.LogWarning("ErosionSlider가 없거나 Slider 컴포넌트를 찾을 수 없습니다.");
                            }

                            Image BodyImage = prisonerImageTransform.Find("BodyImage").GetComponent<Image>();
                            if (prisoner.BodyID >= 0 && prisoner.BodyID < bodySprites.Length)
                            {
                                BodyImage.gameObject.SetActive(true);
                                BodyImage.sprite = bodySprites[prisoner.BodyID];
                            }

                            Image HeadImage = prisonerImageTransform.Find("HeadImage").GetComponent<Image>();
                            if (prisoner.HeadID >= 0 && prisoner.HeadID < headSprites.Length)
                            {
                                HeadImage.gameObject.SetActive(true);
                                HeadImage.sprite = headSprites[prisoner.HeadID];
                            }
                        }
                        DDOManager.UnitDatas.UnitDataDic[(0, 100, prisoner.InstanceID)].ActivityStatus = 4;
                        Destroy(prisonerUI);
                        roomUpdated = true;
                        break;
                    }
                }
            });
        }
        return prisonerUI;
    }

    public void DisplayErosionRoomUI()
    {
        for (int i = 0; i < ErosionRoomCount; i++)
        {
            Transform targetErosionRoom = contentRoomParent.Find("ErosionRoom" + (i + 1));

            if (targetErosionRoom == null)
            {
                GameObject ErosionRoomUI = Instantiate(ErosionRoomPrefab, contentRoomParent);
                ErosionRoomUI.name = $"ErosionRoom{i + 1}";

                Transform deleteButton = ErosionRoomUI.transform.Find("DeleteButton");
                Button button = deleteButton.GetComponent<Button>();
                if (button != null)
                {
                    int roomIndex = i;
                    button.onClick.AddListener(() =>
                    {
                        if (ErosionDataList[roomIndex].InstanceID == -1)
                        {
                            return;
                        }

                        Transform prisonerImageTransform = ErosionRoomUI.transform.Find("PrisonerImage");
                        if (prisonerImageTransform != null)
                        {
                            Transform prisonerNameTransform = prisonerImageTransform.Find("NameText");
                            if (prisonerNameTransform != null)
                            {
                                TextMeshProUGUI nameText = prisonerNameTransform.GetComponent<TextMeshProUGUI>();
                                if (nameText != null)
                                {
                                    nameText.text = "| -------";
                                }
                            }

                            Transform prisonerErosionTransform = prisonerImageTransform.Find("ErosionText");
                            if (prisonerErosionTransform != null)
                            {
                                TextMeshProUGUI HpText = prisonerErosionTransform.GetComponent<TextMeshProUGUI>();
                                if (HpText != null)
                                {
                                    HpText.text = "침식도";
                                }
                            }

                            Slider ErosionSlideBar = ErosionRoomUI.transform.Find("ErosionSlider")?.GetComponent<Slider>();
                            if (ErosionSlideBar != null)
                            {
                                ErosionSlideBar.maxValue = 100;
                                ErosionSlideBar.value = 0;
                            }

                            Image headImage = prisonerImageTransform.Find("HeadImage")?.GetComponent<Image>();
                            if (headImage != null)
                            {
                                headImage.sprite = null;
                                headImage.gameObject.SetActive(false);
                            }

                            Image bodyImage = prisonerImageTransform.Find("BodyImage")?.GetComponent<Image>();
                            if (bodyImage != null)
                            {
                                bodyImage.sprite = null;
                                bodyImage.gameObject.SetActive(false);
                            }

                            DDOManager.UnitDatas.UnitDataDic[(0, 100, ErosionDataList[roomIndex].InstanceID)].ActivityStatus = 0;
                            ErosionDataList[roomIndex].InstanceID = -1;

                            DisplayErosionPrisoners();

                            Debug.Log($"ErosionRoom {roomIndex + 1} 초기화 완료");
                        }
                    });
                }
            }
        }
        GridLayoutGroup gridLayoutGroup = contentRoomParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 1;
        float cellHeight = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.spacing.y;
        float paddingUp = gridLayoutGroup.padding.top;

        // 새로운 높이 계산
        int currentChildCount = contentRoomParent.childCount;  // 현재 자식 수를 가져옴
        float newHeight = (cellHeight + spacingY) * currentChildCount - spacingY + paddingUp;

        // RectTransform 크기 변경
        RectTransform contentRect = contentRoomParent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        Debug.Log($"Content 크기 갱신 완료: {newHeight}");
    }

    public void CheckAndResetErosionSystemState()
    {
        int currentDate = DDOManager.LocalUserDatas.LocalUserDataDic[0].Day;

        if (currentDate != lastCheckedDate)
        {
            ResetAllErosionStates();
            lastCheckedDate = currentDate;
        }
    }

    private void ResetAllErosionStates()
    {
        foreach (var ErosionData in ErosionDataList)
        {
            if (ErosionData.InstanceID == -1)
                continue;

            var unitData = DDOManager.UnitDatas.UnitDataDic[(0, 100, ErosionData.InstanceID)];

            int erosionRate = 2;
            int erosionAmount = 10 + (DDOManager.LocalUserDatas.LocalUserDataDic[0].ErosionEnhance * erosionRate);

            if (unitData.ActivityStatus == 4)
            {
                unitData.DeathErosion -= erosionAmount;
                if (unitData.DeathErosion < 0)
                {
                    unitData.DeathErosion = 0;
                }
            }

            unitData.ActivityStatus = 0;
            ErosionData.InstanceID = -1;
        }

        foreach (Transform healthRoom in contentRoomParent)
        {
            Transform prisonerImageTransform = healthRoom.Find("PrisonerImage");
            if (prisonerImageTransform != null)
            {
                prisonerImageTransform.Find("NameText")?.GetComponent<TextMeshProUGUI>().SetText("| -------");
                prisonerImageTransform.Find("ErosionText")?.GetComponent<TextMeshProUGUI>().SetText("clatlreh");

                Slider HPslideBar = healthRoom.Find("ErosionSlider")?.GetComponent<Slider>();
                if (HPslideBar != null)
                {
                    HPslideBar.maxValue = 1;
                    HPslideBar.value = 0;
                }

                Image headImage = prisonerImageTransform.Find("HeadImage")?.GetComponent<Image>();
                if (headImage != null)
                {
                    headImage.sprite = null;
                    headImage.gameObject.SetActive(false);
                }

                Image bodyImage = prisonerImageTransform.Find("BodyImage")?.GetComponent<Image>();
                if (bodyImage != null)
                {
                    bodyImage.sprite = null;
                    bodyImage.gameObject.SetActive(false);
                }
            }
        }

        DisplayErosionPrisoners();
        DisplayErosionRoomUI();
    }
}
