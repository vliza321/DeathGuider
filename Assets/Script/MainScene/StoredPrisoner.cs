using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredPrisoner : MonoBehaviour
{
    public int currentPrisonerCount = 0;
    public GameObject prisonerUIPrefab;
    public RectTransform uiContentParent;
    public float prefabSpacing = 10f;

    public List<string> currentPrisonerNames = new List<string>();
    public List<int> currentPrisonerHPs = new List<int>();
    public List<int> currentPrisonerProficiencies = new List<int>();
    public List<int> currentPrisonerStrength = new List<int>();
    public List<string> currentPrisonerCrimes = new List<string>();
    public List<int> currentPrisonerErosions = new List<int>();
    public List<Sprite> currentPrisonerHeads = new List<Sprite>();
    public List<Sprite> currentPrisonerBodies = new List<Sprite>();


    public GameObject prisonerInfo;
    public BusPrisonerUI busPrisonerUI;

    public void AddPrisonerPrefab(int index)
    {
        MoveExistingPrefabsDown();
        ++currentPrisonerCount;
        AdjustContentHeight();

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

        currentPrisonerNames.Add(busPrisonerUI.BusPrisonerNames[index]);
        currentPrisonerHPs.Add(busPrisonerUI.BusPrisonerHPs[index]);
        currentPrisonerProficiencies.Add(busPrisonerUI.BusPrisonerProficiencies[index]);
        currentPrisonerStrength.Add(busPrisonerUI.BusPrisonerStrength[index]);
        currentPrisonerCrimes.Add(busPrisonerUI.BusPrisonerCrimes[index]);
        currentPrisonerErosions.Add(busPrisonerUI.BusPrisonerErosions[index]);
        currentPrisonerHeads.Add(busPrisonerUI.BusPrisonerHeads[index]);
        currentPrisonerBodies.Add(busPrisonerUI.BusPrisonerBodies[index]);


        StoredPrisonerstat prisonerScript = prisonerUI.GetComponent<StoredPrisonerstat>();
        if (prisonerScript != null)
        {
            prisonerScript.prisonerName = currentPrisonerNames[index];
            prisonerScript.hp = currentPrisonerHPs[index];
            prisonerScript.proficiency = currentPrisonerProficiencies[index];
            prisonerScript.strength = currentPrisonerStrength[index];
            prisonerScript.crime = currentPrisonerCrimes[index];
            prisonerScript.erosion = currentPrisonerErosions[index];
            prisonerScript.head.sprite = currentPrisonerHeads[index];
            prisonerScript.body.sprite = currentPrisonerBodies[index];
        }

        UpdatePrisonerUI();

        Button prefabButton = prisonerUI.GetComponent<Button>();
        if (prefabButton != null)
        {
            prefabButton.onClick.AddListener(OnButtonAClicked);
        }
    }

    void OnButtonAClicked()
    {
        if (prisonerInfo != null)
        {
            prisonerInfo.SetActive(true);
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

        for (int i = 0; i < currentPrisonerHPs.Count; i++)
        {
            if (currentPrisonerHPs[i] <= 0)
            {
                indicesToRemove.Add(i);
            }
        }

        indicesToRemove.Reverse();

        foreach (int index in indicesToRemove)
        {
            // Remove data from lists
            currentPrisonerNames.RemoveAt(index);
            currentPrisonerHPs.RemoveAt(index);
            currentPrisonerProficiencies.RemoveAt(index);
            currentPrisonerStrength.RemoveAt(index);
            currentPrisonerCrimes.RemoveAt(index);
            currentPrisonerErosions.RemoveAt(index);

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

    public void DecreasePrisonerHP(int index, int amount)
    {
        if (index >= 0 && index < currentPrisonerHPs.Count)
        {
            currentPrisonerHPs[index] -= amount;
            if (currentPrisonerHPs[index] <= 0)
            {
                RemovePrisonersWithLowHP();
            }
            else
            {
                AdjustContentHeight();
                UpdatePrisonerUI(); // UI 업데이트 호출
            }
        }
    }

    public void OnDecreaseHPButtonClick()
    {
        DecreasePrisonerHP(0, 3);
    }


    void UpdatePrisonerUI()
    {
        for (int i = 0; i < uiContentParent.childCount; i++)
        {
            Transform child = uiContentParent.GetChild(i);
            StoredPrisonerstat prisonerStat = child.GetComponent<StoredPrisonerstat>();
            if (prisonerStat != null)
            {
                int index = i; // 현재 인덱스
                if (index < currentPrisonerNames.Count)
                {
                    // UI 요소 업데이트
                    prisonerStat.prisonerName = currentPrisonerNames[index];
                    prisonerStat.hp = currentPrisonerHPs[index];
                    prisonerStat.proficiency = currentPrisonerProficiencies[index];
                    prisonerStat.strength = currentPrisonerStrength[index];
                    prisonerStat.crime = currentPrisonerCrimes[index];
                    prisonerStat.erosion = currentPrisonerErosions[index];
                    prisonerStat.head.sprite = currentPrisonerHeads[index];
                    prisonerStat.body.sprite = currentPrisonerBodies[index];
                    prisonerStat.UpdateUI(); // UI 업데이트 메서드 호출
                }
            }
        }
    }

}
