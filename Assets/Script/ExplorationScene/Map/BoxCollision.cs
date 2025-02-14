using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    private MonsterManager monsterManager;
    private ResultManager resultManager;

    private string playerTagName = "Player";
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (var m in manager)
        {
            if (m.name == "MonsterSpawnManager")
            {
                monsterManager = m.GetComponent<MonsterManager>();
            }
        }
        manager = null;

        monsterManager.Boxes.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTagName))
        {
            resultManager.PlayerEscape();
            monsterManager.PlayerEscape();
        }
    }
}
