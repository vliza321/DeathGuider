using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredPrisoner : MonoBehaviour
{
    public int currentPrisonerCount = 0;
    public GameObject prisonerUIPrefab;
    public RectTransform uiContentParent;
    public float prefabSpacing = 10f;
    public List<PrototypeUnitData> prisoners = new List<PrototypeUnitData>();

    public GameObject prisonerInfo;
    public BusPrisonerUI busPrisonerUI;

    public PrisonerInfoUI prisonerInfoUI;

    public void AddPrisonerPrefab(int index)
    {
        MoveExistingPrefabsDown();
        //++currentPrisonerCount;
        AdjustContentHeight();

        PrototypeUnitData selectedPrisoner = busPrisonerUI.busPrisonerDataList.PrototypeUnitDatas[index];

        GameObject prisonerUI = Instantiate(prisonerUIPrefab, uiContentParent);
        RectTransform rectTransform = prisonerUI.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            float prefabHeight = rectTransform.rect.height;
            float contentHeight = uiContentParent.GetComponent<RectTransform>().rect.height;
            float yOffset = (contentHeight / 2) - prefabSpacing;
            rectTransform.anchoredPosition = new Vector2(0, yOffset);
        }

        ScrollRect scrollRect = uiContentParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        // UI 업데이트를 위한 데이터 설정
        StoredPrisonerstat prisonerScript = prisonerUI.GetComponent<StoredPrisonerstat>();
        if (prisonerScript != null)
        {
            prisonerScript.SetPrisonerData(
                selectedPrisoner.ID,
                selectedPrisoner.Name,
                selectedPrisoner.MaxHealthPoint,
                selectedPrisoner.Strength,
                selectedPrisoner.Defense,
                selectedPrisoner.Handicraft,
                selectedPrisoner.Crime,
                selectedPrisoner.HeadID,
                selectedPrisoner.BodyID
            );
        }

        prisoners.Add(selectedPrisoner);

        UpdatePrisonerUI();

        Button prefabButton = prisonerUI.GetComponent<Button>();
        if (prefabButton != null)
        {
            prefabButton.onClick.AddListener(() => OnPrisonerPrefabClicked(selectedPrisoner));
        }
    }

    void OnPrisonerPrefabClicked(PrototypeUnitData prisoner)
    {
        if (prisonerInfoUI != null)
        {
            prisonerInfoUI.gameObject.SetActive(true);
            prisonerInfoUI.ShowPrisonerInfo(prisoner); // 클릭된 수감자의 정보를 UI에 표시
        }
    }

    void MoveExistingPrefabsDown()
    {
        for (int i = 0; i < uiContentParent.childCount; i++)
        {
            Transform child = uiContentParent.GetChild(i);
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Vector2 newPosition = rectTransform.anchoredPosition;
                newPosition.y -= prefabSpacing + 80;
                rectTransform.anchoredPosition = newPosition;
            }
        }
    }

    public void AdjustContentHeight()
    {
        RectTransform contentRectTransform = uiContentParent.GetComponent<RectTransform>();
        RectTransform prefabRectTransform = prisonerUIPrefab.GetComponent<RectTransform>();
        float prefabHeight = prefabRectTransform.rect.height;
        float totalHeight = (prefabSpacing + prefabHeight) * currentPrisonerCount;
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, -totalHeight);
    }

    public void RemovePrisonersWithLowHP()
    {
        List<int> indicesToRemove = new List<int>();

        for (int i = 0; i < prisoners.Count; i++)
        {
            if (prisoners[i].MaxHealthPoint <= 0) // 수정: prisoners[i]의 hp 확인
            {
                indicesToRemove.Add(i);
            }
        }

        indicesToRemove.Reverse();

        foreach (int index in indicesToRemove)
        {
            // Remove data from lists
            prisoners.RemoveAt(index);

            // Remove the corresponding prefab from the UI
            if (uiContentParent.childCount > index)
            {
                Transform child = uiContentParent.GetChild(index);
                Destroy(child.gameObject); // Remove the GameObject
            }

            // Decrease the prisoner count
            currentPrisonerCount--;
        }

        AdjustContentHeight();
        UpdatePrisonerUI(); // UI 업데이트 호출
    }

    public void UpdatePrisonerUI()
    {
        for (int i = 0; i < uiContentParent.childCount; i++)
        {
            Transform child = uiContentParent.GetChild(i);
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                PrototypeUnitData selectedPrisoner = prisoners[i];  // prisoners 리스트에서 인덱스를 사용해 데이터를 가져옴

                StoredPrisonerstat prisonerStat = child.GetComponent<StoredPrisonerstat>();
                if (prisonerStat != null)
                {
                    prisonerStat.SetPrisonerData(
                        selectedPrisoner.ID,
                        selectedPrisoner.Name,
                        selectedPrisoner.MaxHealthPoint,
                        selectedPrisoner.Strength,
                        selectedPrisoner.Defense,
                        selectedPrisoner.Handicraft,
                        selectedPrisoner.Crime,
                        selectedPrisoner.HeadID,
                        selectedPrisoner.BodyID
                    );
                    prisonerStat.UpdateUI();
                }
            }
        }
    }
}
