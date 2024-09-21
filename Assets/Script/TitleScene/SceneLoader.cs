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
    public static void LoadSceneHandle(string _name, int _loadType)// �ε��� ���� �̸��� ���������� �̾������� ���ϴ� Ÿ��
    {
        loadScene = _name;
        loadType = _loadType;
<<<<<<< HEAD
<<<<<<< HEAD
        SceneManager.LoadScene("Loading"); //ShiftScene from TitleScene (Go to Loading or Main)
=======
        SceneManager.LoadScene("FieldExploration"); //ShiftScene from TitleScene (Go to Loading or Main)
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd
=======
        SceneManager.LoadScene("Loading"); //ShiftScene from TitleScene (Go to Loading or Main)
>>>>>>> parent of 0db0541 (TitleScene > FieldExplorationScene)
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
                Debug.Log("�� ����");
            else if (loadType == 1)
                Debug.Log("�ϴ� ����");

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

