using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    DontDestroyObjectManager ddoManager;
    public Slider progressbar;
    public Text loadtext;
    AsyncOperation operation;
    private void Start()
    {
        StartCoroutine(LoadScene());
        
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                ddoManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
        }

    }

    private void Update()
    {
        operation = SceneManager.LoadSceneAsync("FieldExploration");
        /*if (ddoManager.SaveData())
        */
    }

    IEnumerator LoadScene()
    {
        yield return null;
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if(operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            if(progressbar.value >= 1f)
            {
                loadtext.text = "Press SpaceBar";
            }

            if(Input.GetKeyDown(KeyCode.Space)&&progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            
        }
    }
}
