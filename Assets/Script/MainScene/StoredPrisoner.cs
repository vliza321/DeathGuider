using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredPrisoner : MonoBehaviour
{
    public int currentPrisonerCount = 0;
    public GameObject prisonerUIPrefab;
    public RectTransform uiContentParent;
    public float prefabSpacing = 10f;

<<<<<<< HEAD
    public List<Prisoner> prisoners = new List<Prisoner>();

    public StoredPrisonerstat prisonerDetailInfo;
=======
    public List<string> currentPrisonerNames = new List<string>();
    public List<int> currentPrisonerHPs = new List<int>();
    public List<int> currentPrisonerProficiencies = new List<int>();
    public List<int> currentPrisonerStrength = new List<int>();
    public List<string> currentPrisonerCrimes = new List<string>();
    public List<int> currentPrisonerErosions = new List<int>();

    public GameObject prisonerInfo;
>>>>>>> parent of 1d91ec0 (Revert "TitleScene, Loading")
    public BusPrisonerUI busPrisonerUI;

    public Button storedPrisonerButton;
    public string prisonerName;
    public int hp;
    public int proficiency;
    public int strength;
    public string crime;
    public int erosion;
    public Sprite head;
    public Sprite body;

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

<<<<<<< HEAD
        Prisoner newPrisoner = new Prisoner(busPrisonerUI.BusPrisonerNames[index], busPrisonerUI.BusPrisonerHPs[index], busPrisonerUI.BusPrisonerProficiencies[index], busPrisonerUI.BusPrisonerStrength[index], busPrisonerUI.BusPrisonerCrimes[index], busPrisonerUI.BusPrisonerErosions[index], busPrisonerUI.BusPrisonerHeads[index], busPrisonerUI.BusPrisonerBodies[index]);

        prisoners.Add(newPrisoner);
=======
        currentPrisonerNames.Add(busPrisonerUI.BusPrisonerNames[index]);
        currentPrisonerHPs.Add(busPrisonerUI.BusPrisonerHPs[index]);
        currentPrisonerProficiencies.Add(busPrisonerUI.BusPrisonerProficiencies[index]);
        currentPrisonerStrength.Add(busPrisonerUI.BusPrisonerStrength[index]);
        currentPrisonerCrimes.Add(busPrisonerUI.BusPrisonerCrimes[index]);
        currentPrisonerErosions.Add(busPrisonerUI.BusPrisonerErosions[index]);
>>>>>>> parent of 1d91ec0 (Revert "TitleScene, Loading")

        StoredPrisonerstat prisonerScript = prisonerUI.GetComponent<StoredPrisonerstat>();
        if (prisonerScript != null)
        {
<<<<<<< HEAD
            prisonerScript.SetPrisonerData(newPrisoner.name, newPrisoner.hp, newPrisoner.proficiency, newPrisoner.strength, newPrisoner.crime, newPrisoner.erosion, newPrisoner.head, newPrisoner.body);
            //prisonerScript.prisonerName = newPrisoner.name;
            //prisonerScript.hp = newPrisoner.hp;
            //prisonerScript.proficiency = newPrisoner.proficiency;
            //prisonerScript.strength = newPrisoner.strength;
            //prisonerScript.crime = newPrisoner.crime;
            //prisonerScript.erosion = newPrisoner.erosion;
            //prisonerScript.head.sprite = newPrisoner.head;
            //prisonerScript.body.sprite = newPrisoner.body;
=======
            prisonerScript.prisonerName = currentPrisonerNames[index];
            prisonerScript.hp = currentPrisonerHPs[index];
            prisonerScript.proficiency = currentPrisonerProficiencies[index];
            prisonerScript.strength = currentPrisonerStrength[index];
            prisonerScript.crime = currentPrisonerCrimes[index];
            prisonerScript.erosion = currentPrisonerErosions[index];
>>>>>>> parent of 1d91ec0 (Revert "TitleScene, Loading")
        }
        // 그냥 비주얼 스튜디오에 챗gpt 껴ㅑ줘라 이제 하 22 407에 설치해둔다
        UpdatePrisonerUI();

        Button prefabButton = prisonerUI.GetComponent<Button>();
        if (prefabButton != null)
        {
            prefabButton.onClick.AddListener(OnButtonAClicked);
        }
    }

    void OnButtonAClicked()
    {
        if(prisonerDetailInfo != null)
        {
            prisonerDetailInfo.gameObject.SetActive(true);
            prisonerDetailInfo.SetPrisonerData(prisonerName, hp, proficiency, strength, crime, erosion, head, body);
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
            if (prisoners[i].hp <= 0)
            {
                indicesToRemove.Add(i);
            }
        }

        indicesToRemove.Reverse();

        foreach (int index in indicesToRemove)
        {
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
        if (index >= 0 && index < prisoners.Count)
        {
            prisoners[index].hp -= amount;
            if (prisoners[index].hp <= 0)
            {
                RemovePrisonersWithLowHP();
            }
            else
            {
                AdjustContentHeight();
                UpdatePrisonerUI();
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
<<<<<<< HEAD
                Prisoner prisoner = prisoners[i];

                // UI 요소 업데이트
                prisonerStat.SetPrisonerData(prisoner.name, prisoner.hp, prisoner.proficiency, prisoner.strength, prisoner.crime, prisoner.erosion, prisoner.head, prisoner.body);
                //prisonerStat.prisonerName = prisoner.name;
                //prisonerStat.hp = prisoner.hp;
                //prisonerStat.proficiency = prisoner.proficiency;
                //prisonerStat.strength = prisoner.strength;
                //prisonerStat.crime = prisoner.crime;
                //prisonerStat.erosion = prisoner.erosion;
                //prisonerStat.head.sprite = prisoner.head;
                //prisonerStat.body.sprite = prisoner.body;

                //prisonerStat.UpdateUI();
=======
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
                    prisonerStat.UpdateUI(); // UI 업데이트 메서드 호출
                }
>>>>>>> parent of 1d91ec0 (Revert "TitleScene, Loading")
            }
        }
    }
}
