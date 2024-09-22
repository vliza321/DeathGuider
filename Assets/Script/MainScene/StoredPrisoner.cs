using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredPrisoner : MonoBehaviour
{
    public int currentPrisonerCount = 0;
    public GameObject prisonerUIPrefab;
    public RectTransform uiContentParent;
    public float prefabSpacing = 10f;
    public List<Prisoner> prisoners = new List<Prisoner>();

    public GameObject prisonerInfo;
    public BusPrisonerUI busPrisonerUI;

    private PrisonerInfoUI prisonerInfoUI;

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

        // 새로운 수감자 추가
        Prisoner newPrisoner = new Prisoner(
            busPrisonerUI.BusPrisonerNames[index],
            busPrisonerUI.BusPrisonerHPs[index],
            busPrisonerUI.BusPrisonerProficiencies[index],
            busPrisonerUI.BusPrisonerStrength[index],
            busPrisonerUI.BusPrisonerCrimes[index],
            busPrisonerUI.BusPrisonerErosions[index],
            busPrisonerUI.BusPrisonerHeads[index],
            busPrisonerUI.BusPrisonerBodies[index]
        );

        prisoners.Add(newPrisoner);

        StoredPrisonerstat prisonerScript = prisonerUI.GetComponent<StoredPrisonerstat>();
        if (prisonerScript != null)
        {
            prisonerScript.SetPrisonerData(newPrisoner.name, newPrisoner.hp, newPrisoner.proficiency, newPrisoner.strength, newPrisoner.crime, newPrisoner.erosion, newPrisoner.head, newPrisoner.body);
        }

        UpdatePrisonerUI();

        Button prefabButton = prisonerUI.GetComponent<Button>();
        if (prefabButton != null)
        {
            prefabButton.onClick.AddListener(() => OnPrisonerPrefabClicked(newPrisoner));
        }
    }

    void OnPrisonerPrefabClicked(Prisoner prisoner)
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
            if (prisoners[i].hp <= 0) // 수정: prisoners[i]의 hp 확인
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

    public void DecreasePrisonerHP(int index, int amount)
    {
        if (index >= 0 && index < prisoners.Count) // 수정: prisoners 리스트 확인
        {
            prisoners[index].hp -= amount; // 수정: prisoners[index]의 hp 감소
            if (prisoners[index].hp <= 0)
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
        DecreasePrisonerHP(0, 3); // 인덱스와 감소할 체력을 적절히 수정
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
                if (index < prisoners.Count) // 수정: prisoners.Count로 인덱스 확인
                {
                    // UI 요소 업데이트
                    prisonerStat.SetPrisonerData(
                        prisoners[index].name,
                        prisoners[index].hp,
                        prisoners[index].proficiency,
                        prisoners[index].strength,
                        prisoners[index].crime,
                        prisoners[index].erosion,
                        prisoners[index].head,
                        prisoners[index].body
                    );
                    prisonerStat.UpdateUI(); // UI 업데이트 메서드 호출
                }
            }
        }
    }
}
