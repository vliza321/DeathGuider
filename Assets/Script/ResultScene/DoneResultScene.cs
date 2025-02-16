using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoneResultScene : MonoBehaviour
{

    [SerializeField]
    private ResultManager resultManager;
    private Text text;
    Button myButton;
    public void Start()
    {
        
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("Manager") && ddo.name == "ResultManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                resultManager = ddo.GetComponent<ResultManager>();
            }
        }
        DDO = null;

        myButton = this.GetComponentInChildren<Button>();
        text = this.GetComponentInChildren<Text>();
        if (myButton != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() =>
            {
                Debug.Log("결과보기");
                resultManager.SaveBattleResult(resultManager.IsVictory);
                text.text = "이동하기";
                ChangeButton();
            });
        }
    }

    public void ChangeButton()
    {
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LobbyTest");
        });
    }
    // "Title" 씬으로 이동하는 버튼 기능
    public void OnClickTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // "Main" 씬으로 이동하는 버튼 기능
    public void OnClickMain()
    {
        SceneManager.LoadScene("LobbyTest");
    }

    public void OnButtonClick()
    {
        Debug.Log("결과보기");
    }

}
