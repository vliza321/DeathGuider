using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private float timer;
    private bool playerEscape;
    private FadeInOut fadeInOutUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI");
        foreach (var u in UI)
        {
            if (u.name == "FadeInOutEffect")
            {
                fadeInOutUI = u.GetComponent<FadeInOut>();
            }
        }
        timer = 0;
        playerEscape = false;
        UI = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerEscape) timer += Time.deltaTime;
    }

    public void PlayerEscape()
    {
        playerEscape = true;
        fadeInOutUI.StartFadeOut();
        DontDestroyOnLoad(this.gameObject);
       
    }
}
