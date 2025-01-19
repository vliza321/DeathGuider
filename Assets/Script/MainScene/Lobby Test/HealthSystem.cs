using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public GameObject HealthUnitUIPrefab;
    public GameObject HealthRoomPrefab;

    public Transform contentUnitParent;
    public Transform contentRoomParent;

    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    public int healthRoomCount = 3;

    public List<HealthData> HealthDataList = new List<HealthData>();

    [System.Serializable]
    public class HealthData
    {
        public int InstanceID;

        public HealthData()
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
            DDO = null;
        }

        for (int i = 0; i < healthRoomCount; i++)
        {
            HealthData newHealthData = new HealthData(); // InstanceID는 0부터 시작
            HealthDataList.Add(newHealthData);
        }

    }

    public void DisplayHealthPrisoners()
    {
        if (DDOManager.UnitDatas == null || DDOManager.UnitDatas.UnitDatas == null)
        {
            Debug.LogError("UnitDatas 리스트가 초기화되지 않았습니다.");
            return;
        }

        foreach (Transform child in contentUnitParent)
        {
            Destroy(child.gameObject);
        }

        var filteredPrisoners = DDOManager.UnitDatas.UnitDatas
    .Where(prisoner => prisoner != null && prisoner.ActivityStatus == 0 && prisoner.PrototypeUnitID == 100 && prisoner.HealthPoint != prisoner.MaxHealthPoint)
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
                GameObject prisonerUI = contentUnitParent.Find($"Gym Prisoner{prisonerIndex + 1}")?.gameObject;
                prisonerUI = CreateHealthPrisonerUI(prisoner);
                prisonerUI.name = $"Gym Prisoner{prisonerIndex + 1}";
                UpdateHealthPrisonerUI(prisonerUI, prisoner);
                prisonerIndex++;
            }
        }
        else
        {
            Debug.LogWarning("조건에 맞는 죄수 데이터가 없습니다.");
        }
    }

    private void UpdateHealthPrisonerUI(GameObject prisonerUI, UnitData prisoner)
    {
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = $"HP: {prisoner.HealthPoint}/{prisoner.MaxHealthPoint}";

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


    private GameObject CreateHealthPrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(HealthUnitUIPrefab, contentUnitParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = $"HP: {prisoner.HealthPoint}/{prisoner.MaxHealthPoint}";

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

                for (int i = 0; i < healthRoomCount; i++)
                {
                    Transform targetHealthRoom = contentRoomParent.Find("HealthRoom" + (i + 1));
                    if (targetHealthRoom != null)
                    {
                        if (HealthDataList[i].InstanceID >= 0)
                        {
                            continue;
                        }

                        HealthDataList[i].InstanceID = prisoner.InstanceID;
                        Transform prisonerImageTransform = targetHealthRoom.Find("PrisonerImage");
                        if (prisonerImageTransform != null)
                        {
                            Transform prisonerNameTransform = prisonerImageTransform.Find("NameText");
                            if (prisonerNameTransform != null)
                            {
                                TextMeshProUGUI nameText = prisonerNameTransform.GetComponent<TextMeshProUGUI>();
                                if (nameText != null)
                                {
                                    nameText.text = prisoner.Name; // 이름을 변경
                                }
                            }
                            Transform prisonerHPTransform = prisonerImageTransform.Find("HealthText");
                            if (prisonerHPTransform != null)
                            {
                                TextMeshProUGUI prisonerHPText = prisonerHPTransform.GetComponent<TextMeshProUGUI>();
                                if (prisonerHPText != null)
                                {
                                    prisonerHPText.text = $"HP: {prisoner.HealthPoint}/{prisoner.MaxHealthPoint}";
                                }
                            }

                            Slider HPslideBar = targetHealthRoom.Find("HealthSlider").GetComponent<Slider>();
                            if(HPslideBar != null)
                            {

                                HPslideBar.maxValue = prisoner.MaxHealthPoint;
                                HPslideBar.value = prisoner.HealthPoint;
                            }
                            else
                            {
                                Debug.LogWarning("HealthSlider가 없거나 Slider 컴포넌트를 찾을 수 없습니다.");
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
                        DDOManager.UnitDatas.UnitDataDic[(0, 100, prisoner.InstanceID)].ActivityStatus = 2;
                        Destroy(prisonerUI);
                        roomUpdated = true;
                        break;
                    }
                }
            });
        }

        return prisonerUI;
    }

    public void DisplayHealthRoomUI()
    {
        for (int i = 0; i < healthRoomCount; i++)
        {
            Transform targetHealthRoom = contentRoomParent.Find("HealthRoom" + (i + 1));

            if (targetHealthRoom == null)
            {
                // HealthRoom이 없다면 새로 생성
                GameObject HealthRoomUI = Instantiate(HealthRoomPrefab, contentRoomParent);
                HealthRoomUI.name = $"HealthRoom{i + 1}";

                Transform deleteButton = HealthRoomUI.transform.Find("DeleteButton");
                Button button = deleteButton.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        Debug.Log("HealthRoom 삭제 버튼 클릭됨");
                    });
                }
            }
        }
    }
}
