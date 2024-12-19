using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum RangeOut
{
    nonRangeOut,
    rowRangeOut,
    columnRangeOut,
    doubleRangeOut
}

public class TileSpawnManager : MonoBehaviour
{
    private TileMap[] baseTileMap;
    private PlayerManager playerManager;
    private MonsterManager monsterManager;
    private GameObject followerManager;
    private TreasureBoxEscapeStairManager treasureBoxEscapeStairManager;
    private GameObject guider;
    private int followercounter;
    private GameObject playerAttackDirectional;
    private CameraManager cameraManager;

    private GameObject[] weaponEffectPool;

    private RangeOut discriminationState; 
    private void Awake()
    {
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {    
            if (manager.name == "PlayerManager")
            {
                playerManager = manager.transform.gameObject.GetComponent<PlayerManager>();
            }
            if (manager.name == "MonsterSpawnManager")
            {
                monsterManager = manager.transform.gameObject.GetComponent<MonsterManager>();
            }
            if (manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject;
            }
            if (manager.name == "TreasureBoxEscapeStairManager")
            {
                treasureBoxEscapeStairManager = manager.transform.gameObject.GetComponent<TreasureBoxEscapeStairManager>();
            }
            if (manager.name == "CameraManager")
            {
                cameraManager = manager.transform.gameObject.GetComponent<CameraManager>();
            }
        }
        Manager = null;
        discriminationState = RangeOut.nonRangeOut;
        weaponEffectPool = new GameObject[2];
        weaponEffectPool[0] = playerManager.GetComponent<PlayerManager>().WeaponEffectPool;
        weaponEffectPool[1] = followerManager.GetComponent<FollowerManager>().WeaponEffectPool;

        baseTileMap = new TileMap[2];
        baseTileMap[0] = this.transform.GetChild(0).GetComponent<TileMap>();
        baseTileMap[1] = this.transform.GetChild(1).GetComponent<TileMap>();
        baseTileMap[0].gameObject.SetActive(true);
        baseTileMap[1].gameObject.SetActive(true);
        baseTileMap[0].Init();
        baseTileMap[1].Init();
    }
    // Update is called once per frame
    void Start()
    {
        guider = playerManager.Guider;
        followercounter = followerManager.GetComponent<FollowerManager>().FollowerCounter;
        playerAttackDirectional = playerManager.transform.GetChild(1).gameObject;
        baseTileMap[1].gameObject.SetActive(false);
    }

    public void swapTileMap(Transform transform, int row, int column)
    {
        guider = playerManager.Guider;

        // spawnManager의 상태 체크 부분
        if((baseTileMap[0].Row + row - 2 < 1|| baseTileMap[1].Row + row - 2 < 1) || (baseTileMap[0].Row  + row - 2 > 9 || baseTileMap[1].Row + row - 2 > 9))
        {
            discriminationState = RangeOut.rowRangeOut;
            if ((baseTileMap[0].Column + column - 2 < 1 || baseTileMap[1].Column + column - 2 < 1)||(baseTileMap[0].Column + column - 2> 9 || baseTileMap[1].Column + column - 2 > 9))
            { 
                discriminationState = RangeOut.doubleRangeOut;
            }
        }
        else
        {
            discriminationState = RangeOut.nonRangeOut;
            if ((baseTileMap[0].Column + column - 2 < 1 || baseTileMap[1].Column + column - 2 < 1)||(baseTileMap[0].Column + column - 2> 9 || baseTileMap[1].Column + column - 2 > 9))
            {
                discriminationState = RangeOut.columnRangeOut;
            }
        }

        // spawnManager의 상태별 행동
        switch (discriminationState)
        {
            case RangeOut.nonRangeOut:
                
                baseTileMap[0].SwapTileMap(row - 2, column - 2);
                baseTileMap[1].SwapTileMap(row - 2, column - 2);

                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    baseTileMap[1].transform.position = transform.position;
                    baseTileMap[1].gameObject.SetActive(true);
                    baseTileMap[0].gameObject.SetActive(false);
                }

                else
                {
                    baseTileMap[0].transform.position = transform.position;
                    baseTileMap[0].gameObject.SetActive(true);
                    baseTileMap[1].gameObject.SetActive(false);

                }

                baseTileMap[0].ChangeTile(row - 2, column - 2);
                baseTileMap[1].ChangeTile(row - 2, column - 2);
                break;

            case RangeOut.rowRangeOut:

                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);

                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    if (baseTileMap[0].Row <= 1) baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 4), 0, Space.Self);
                    else baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 6), 0, Space.Self);

                }

                else
                {
                    if (baseTileMap[1].Row <= 1) baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 4), 0, Space.Self);
                    else baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 6), 0, Space.Self);
                }

                guider.transform.Translate(0 , 12.8f * (baseTileMap[0].Row - 5), 0,Space.Self);

                playerAttackDirectional.transform.Translate(0 , 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                cameraManager.MainMoveCamera.transform.Translate(0 , 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                for (int i = 0; i < followercounter; i++)
                {
                   
                    followerManager.transform.GetChild(i).transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }

                for (int i = 0; i < monsterManager.EnabledMonster; i++)
                {
                    monsterManager.transform.GetChild(i).Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }


                baseTileMap[0].ChangeTile(5,0);
                baseTileMap[1].ChangeTile(5,0);

                for (int i = 0; i < weaponEffectPool.Length; i++)
                {
                    weaponEffectPool[i].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }

                treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[0].Row ), 0);

                baseTileMap[0].Row = 5;
                baseTileMap[1].Row = 5;

                break;

            case RangeOut.columnRangeOut:

                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);

                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    if (baseTileMap[0].Column <= 1) baseTileMap[1].transform.Translate(12.8f * (4 - baseTileMap[0].Column), 0, 0, Space.Self);
                    else baseTileMap[1].transform.Translate(12.8f * (6 - baseTileMap[0].Column), 0, 0, Space.Self);
                }

                else
                {
                    if (baseTileMap[0].Column <= 1) baseTileMap[0].transform.Translate(12.8f * (4 - baseTileMap[0].Column), 0, 0, Space.Self);
                    else baseTileMap[0].transform.Translate(12.8f * (6 - baseTileMap[0].Column), 0, 0, Space.Self);
                }

                guider.transform.Translate( 12.8f * (5 - baseTileMap[0].Column),0, 0, Space.Self);

                playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[0].Column ), 0, 0, Space.Self);

                cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);

                for (int i = 0; i < followercounter; i++)
                {
                    followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                }

                for (int i = 0; i < monsterManager.EnabledMonster; i++)
                {
                    monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[0].Column ), 0, 0, Space.Self);
                }


                baseTileMap[0].ChangeTile(0, 5);
                baseTileMap[1].ChangeTile(0, 5);

                for (int i = 0; i < weaponEffectPool.Length; i++)
                {
                    weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                }

                treasureBoxEscapeStairManager.EventSwapTile(0, (5 - baseTileMap[0].Column));

                baseTileMap[0].Column = 5;
                baseTileMap[1].Column = 5;

                break;

            case RangeOut.doubleRangeOut:

                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);

                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    if (baseTileMap[0].Column <= 1) baseTileMap[1].transform.Translate(12.8f * (4 - baseTileMap[0].Column), 0, 0, Space.Self);
                    else baseTileMap[1].transform.Translate(12.8f * (6 - baseTileMap[0].Column), 0, 0, Space.Self);
                }
                else
                {
                    if (baseTileMap[0].Column <= 1) baseTileMap[0].transform.Translate(12.8f * (4 - baseTileMap[0].Column), 0, 0, Space.Self);
                    else baseTileMap[0].transform.Translate(12.8f * (6 - baseTileMap[0].Column), 0, 0, Space.Self);
                }

                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    if (baseTileMap[0].Row <= 1) baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 4), 0, Space.Self);
                    else baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 6), 0, Space.Self);

                }
                else
                {
                    if (baseTileMap[1].Row <= 1) baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 4), 0, Space.Self);
                    else baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 6), 0, Space.Self);
                }

                guider.transform.Translate(12.8f * (5 - baseTileMap[0].Column ), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                for (int i = 0; i < followercounter; i++)
                {

                    followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }

                for (int i = 0; i < monsterManager.EnabledMonster; i++)
                {
                    monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }


                baseTileMap[0].ChangeTile(5, 5);
                baseTileMap[1].ChangeTile(5, 5);

                for (int i = 0; i < weaponEffectPool.Length; i++)
                {
                    weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                }

                treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[0].Column), (baseTileMap[0].Row- 5));

                baseTileMap[0].Row = 5;
                baseTileMap[1].Row = 5;
                baseTileMap[0].Column = 5;
                baseTileMap[1].Column = 5;
                
                //EditorApplication.isPaused = true;
                break;
        }
    }
}
