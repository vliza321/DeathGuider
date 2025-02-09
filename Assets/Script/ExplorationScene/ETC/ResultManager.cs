using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private float timer;
    private bool playerEscape;
    private FadeInOut fadeInOutUI;
    
    [SerializeField]
    private float gold;
    [SerializeField]
    private float darkEssense;
    [SerializeField]
    private int deathEssense;
    [SerializeField]
    private float exp;
    public float Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public float DarkEssense
    {
        get { return darkEssense; }
        set { darkEssense = value; }
    }

    public int DeathEssense
    {
        get { return deathEssense; }
        set { deathEssense = value; }
    }

    public float Exp
    {
        get { return exp; }
        set { exp = value; }
    }

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
