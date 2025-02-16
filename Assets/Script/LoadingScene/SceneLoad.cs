using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;
    AsyncOperation operation;

    public FadeOut fadeOut;

    public bool isDone;
    private void Start()
    {
        StartCoroutine(LoadScene());
        operation = SceneManager.LoadSceneAsync("FieldExploration");
        isDone = false;
    }

    private void Update()
    {
        if(isDone)
        {
            operation.allowSceneActivation = true;
        }
        if (fadeOut.Alpha >0.99f)
        {
            isDone = true;
        }
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
                fadeOut.StartFadeOut();
                //operation.allowSceneActivation = true;
                //isDone = true;
            }

            
        }
    }
}
