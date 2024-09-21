using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    public static string loadScene;
    public static int loadType;
    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    public static void LoadSceneHandle(string _name, int _loadType)// 로딩할 씬의 이름과 새게임인지 이어할지를 정하는 타입
    {
        loadScene = _name;
        loadType = _loadType;
        SceneManager.LoadScene("Loading"); //ShiftScene from TitleScene (Go to Loading or Main)
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            yield return null;

            if (loadType == 0)
                Debug.Log("새 게임");
            else if (loadType == 1)
                Debug.Log("하던 게임");

            if(progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

           
        }
    }
 
}

