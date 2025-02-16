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
    [SerializeField] private Text timerText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Animator fadeAnimator;

    private GameManager gameManager;
    private DontDestroyObjectManager DDOManager;
    private ResultManager resultManager;

    void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager")
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        resultManager = FindObjectOfType<ResultManager>();
        //DisplayResults();
        //mainMenuButton.onClick.AddListener(StartFadeOut);
    }

    private void InitializeManagers()
    {

    }

    public void DisplayResults()
    {
        if (resultManager == null)
        {
            //Debug.LogError("ResultManager not found!");
            return;
        }

        // 결과 출력
        resultText.text = resultManager.IsVictory ? "Victory!" : "Defeat";
        goldText.text = "  " + (int)resultManager.Gold + " G";
        darkEssenceText.text = "  " + resultManager.DarkEssense + " D";
        deathEssenceText.text = "  " + resultManager.DeathEssense + " EA";
        expText.text = "  " + resultManager.Exp + " EXP";
        timerText.text = "  " + (int)(resultManager.Timer / 60) + " : " + (int)(resultManager.Timer % 60) + " ";

        // 유닛 생존 여부 표시
        int survivingUnits = resultManager.Units.Count;
        unitSurvivalText.text = "  " + survivingUnits;
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
