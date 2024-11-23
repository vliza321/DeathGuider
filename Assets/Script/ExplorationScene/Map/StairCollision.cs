using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairCollision : MonoBehaviour
{
    private FadeInOut fadeInOutUI;
    private MonsterManager monsterManager;
    private ResultManager resultManager;

    private string playerTagName = "Player";
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI");
        foreach(var u in UI)
        {
            if(u.name == "FadeInOutEffect")
            {
                fadeInOutUI = u.GetComponent<FadeInOut>();
            }
        }
        GameObject[] manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach(var m in manager)
        {
            if (m.name == "MonsterSpawnManager")
            {
                monsterManager = m.GetComponent<MonsterManager>();
            }
            if(m.name == "ResultManager")
            {
                resultManager = m.GetComponent<ResultManager>();
            }
        }
        UI = null;
        manager = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(playerTagName))
        {
            resultManager.PlayerEscape();
            monsterManager.PlayerEscape();
        }
    }
}
