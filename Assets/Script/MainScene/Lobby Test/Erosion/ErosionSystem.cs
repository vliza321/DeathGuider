using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ErosionSystem : MonoBehaviour
{
    public GameObject ErosionUnitUIPrefab;
    public GameObject ErosionRoomPrefab;

    public Transform contentUnitParent;
    public Transform contentRoomParent;

    //public Sprite[] headSprites;
    //public Sprite[] bodySprites;

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

            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.GetComponent<GameManager>();
            }
        }
        DDO = null;

        for (int i = 0; i < ErosionRoomCount; i++)
        {
            ErosionData newErosionData = new ErosionData();
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

        if (filteredPrisoners.Count > 0)
        {
            int prisonerIndex = 0;
            foreach (var prisoner in filteredPrisoners)
            {
                GameObject prisonerUI = contentUnitParent.Find($"Erosion Prisoner{prisonerIndex + 1}")?.gameObject;
                prisonerUI = CreateErosionPrisonerUI(prisoner);
                prisonerUI.name = $"Erosion Prisoner{prisonerIndex + 1}";
                UpdateErosionPrisonerUI(prisonerUI, prisoner);
                prisonerIndex++;
            }
        }
    }

    private void UpdateErosionPrisonerUI(GameObject prisonerUI, UnitData prisoner)
    {
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("ErosionText").GetComponent<TextMeshProUGUI>().text = $"Erosion: {prisoner.DeathErosion} / 100";

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
    }

    private GameObject CreateErosionPrisonerUI(UnitData prisoner)
    {
        GameObject prisonerUI = Instantiate(ErosionUnitUIPrefab, contentUnitParent);
        prisonerUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = prisoner.Name;
        prisonerUI.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"Lv: {prisoner.Level}";
        prisonerUI.transform.Find("ErosionText").GetComponent<TextMeshProUGUI>().text = $"Erosion: {prisoner.DeathErosion} / 100";

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
                                HPslideBar.interactable = false;
                            }

                            Image BodyImage = prisonerImageTransform.Find("BodyImage").GetComponent<Image>();
                            if (prisoner.BodyID >= 0 && prisoner.BodyID < GameManager.PrisonerBodyImg.Count)
                            {
                                BodyImage.gameObject.SetActive(true);
                                BodyImage.sprite = GameManager.PrisonerBodyImg[prisoner.BodyID];
                            }

                            Image HeadImage = prisonerImageTransform.Find("HeadImage").GetComponent<Image>();
                            if (prisoner.HeadID >= 0 && prisoner.HeadID < GameManager.PrisonerHeadImg.Count)
                            {
                                HeadImage.gameObject.SetActive(true);
                                HeadImage.sprite = GameManager.PrisonerHeadImg[prisoner.HeadID];
                            }
                        }
                        DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, prisoner.PrototypeUnitID, prisoner.InstanceID)].ActivityStatus = 3;
                        Destroy(prisonerUI);
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
                                ErosionSlideBar.interactable = false;
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

        int currentChildCount = contentRoomParent.childCount;
        float newHeight = (cellHeight + spacingY) * currentChildCount - spacingY + paddingUp;

        RectTransform contentRect = contentRoomParent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    public void CheckAndResetErosionSystemState()
    {
        int currentDate = DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Day;

        if (currentDate != lastCheckedDate)
        {
            ResetAllErosionStates();
            lastCheckedDate = currentDate;
        }
    }

    private void ResetAllErosionStates()
    {
        foreach (var unit in DDOManager.UnitDatas.UnitDataDic.Values)
        {
            if (unit.ActivityStatus == 3)
            {
                int erosionRate = 2;
                int erosionAmount = 10 + (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].ErosionEnhance * erosionRate);

                unit.DeathErosion -= erosionAmount;
                if (unit.DeathErosion < 0)
                {
                    unit.DeathErosion = 0;
                }

                unit.ActivityStatus = 0;
            }
        }

        //foreach (var ErosionData in ErosionDataList)
        //{
        //    if (ErosionData.InstanceID == -1)
        //        continue;

        //    var unitData = DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)];

        //    int erosionRate = 2;
        //    int erosionAmount = 10 + (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].ErosionEnhance * erosionRate);

        //    if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)].ActivityStatus == 3)
        //    {
        //        DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)].DeathErosion -= erosionAmount;
        //        if (DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)].DeathErosion < 0)
        //        {
        //            DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)].DeathErosion = 0;
        //        }
        //    }

        //    DDOManager.UnitDatas.UnitDataDic[(GameManager.SelectUserID, 100, ErosionData.InstanceID)].ActivityStatus = 0;
        //    ErosionData.InstanceID = -1;
        //}

        foreach (Transform erosionRoom in contentRoomParent)
        {
            Transform prisonerImageTransform = erosionRoom.Find("PrisonerImage");
            if (prisonerImageTransform != null)
            {
                prisonerImageTransform.Find("NameText")?.GetComponent<TextMeshProUGUI>().SetText("| -------");
                prisonerImageTransform.Find("ErosionText")?.GetComponent<TextMeshProUGUI>().SetText("침식도");

                Slider erosionSlider = erosionRoom.Find("ErosionSlider")?.GetComponent<Slider>();
                if (erosionSlider != null)
                {
                    erosionSlider.maxValue = 1;
                    erosionSlider.value = 0;
                    erosionSlider.interactable = false;
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
