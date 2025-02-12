using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MasterManager : MonoBehaviour
{
    //GameObject가 있는 변수
    private List<ManagerBase> managers;
    private GameManager gameManager;
    private DontDestroyObjectManager ddoManager;

    public GameManager GameManager
    {
        get { return gameManager; }
    }
    public DontDestroyObjectManager DDOManager
    {
        get { return ddoManager; }
    }

    //GameObject가 필요한 변수
    private TestManager testManager;
    private GameObject MonsterSpawnManagerObject;
    private MonsterSpawnManager monsterSpawnManager;
    private TestGoodsSpawnManager goldSpawn;
    private TestGoodsSpawnManager darkEssenseSpawn;
    private TestGoodsSpawnManager expSpawn;


    // Start is called before the first frame update
    void Awake()
    {
        //
        managers = new List<ManagerBase>();
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (GameObject ddo in DDO)
        {
            if (ddo.name == "GameManager")
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
            if (ddo.name == "DDOManager")
            {
                ddoManager = ddo.transform.gameObject.GetComponent<DontDestroyObjectManager>();
            }
        }
        DDO = null;
        ddoManager.Init();
        //gameManager.SelectStageID = 11;

        MonsterSpawnManagerObject = new GameObject();
        ObjectInit(MonsterSpawnManagerObject);

        testManager = new TestManager(this);
        monsterSpawnManager = new MonsterSpawnManager(this,MonsterSpawnManagerObject);
        AddManager(monsterSpawnManager);
        AddManager(testManager);

        foreach (var manager in managers)
        {
            manager.Awake();
        }
    }

    public void ObjectInit(GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Start()
    {
        foreach (var manager in managers)
        {
            manager.Start();
        }
    }

    public void AddManager(ManagerBase manager)
    {
        managers.Add(manager);
    }

    public void RemoveManager(ManagerBase manager)
    {
        managers.Remove(manager);
    }

    private void Update()
    {
        foreach(var manager in managers)
        {
            manager.Update();
        }
    }
}
