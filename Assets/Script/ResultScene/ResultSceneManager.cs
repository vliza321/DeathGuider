using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] private Text resultText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text darkEssenceText;
    [SerializeField] private Text deathEssenceText;
    [SerializeField] private Text expText;
    [SerializeField] private Text unitSurvivalText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Animator fadeAnimator;

    private GameManager gameManager;
    private DontDestroyObjectManager DDOManager;
    private ResultManager resultManager;

    void Start()
    {
        InitializeManagers();
        DisplayResults();
        mainMenuButton.onClick.AddListener(StartFadeOut);
    }

    private void InitializeManagers()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "GameManager")
                gameManager = ddo.GetComponent<GameManager>();
            if (ddo.name == "DDOManager")
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
        }
        DDO = null;

        resultManager = FindObjectOfType<ResultManager>();
    }

    private void DisplayResults()
    {
        if (resultManager == null)
        { 
        //{
        //    Debug.Log("어녕")
        //    Debug.LogError("ResultManager not found!");
            return;
        }

        // 결과 출력
        resultText.text = resultManager.IsVictory ? "Victory!" : "Defeat";
        goldText.text = "Gold: " + resultManager.Gold;
        darkEssenceText.text = "Dark Essence: " + resultManager.DarkEssense;
        deathEssenceText.text = "Death Essence: " + resultManager.DeathEssense;
        expText.text = "Exp: " + resultManager.Exp;

        // 유닛 생존 여부 표시
        int survivingUnits = resultManager.Units.Count;
        unitSurvivalText.text = "Surviving Units: " + survivingUnits;
    }

    private void StartFadeOut()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
            Invoke("LoadMainMenu", 1f); // 애니메이션 길이에 맞춰서 변경 가능
        }
        else
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
