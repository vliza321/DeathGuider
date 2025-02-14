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
    [SerializeField]
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
    private TileSpriteImageStorage tileSpriteImageStorage;
    private RangeOut discriminationState; 
    private void Awake()
    {
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {    
            if (manager.name == "PlayerManager")
            {
                playerManager = manager.GetComponent<PlayerManager>();
            }
            if (manager.name == "MonsterSpawnManager")
            {
                monsterManager = manager.GetComponent<MonsterManager>();
            }
            if (manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject;
            }
            if (manager.name == "TreasureBoxEscapeStairManager")
            {
                treasureBoxEscapeStairManager = manager.GetComponent<TreasureBoxEscapeStairManager>();
            }
            if (manager.name == "CameraManager")
            {
                cameraManager = manager.GetComponent<CameraManager>();
            }
            if (manager.name == "TileSpriteImageStorage")
            {
                tileSpriteImageStorage = manager.GetComponent<TileSpriteImageStorage>();
            }
            
        }
        Manager = null;
        tileSpriteImageStorage.Init();

        discriminationState = RangeOut.nonRangeOut;
        weaponEffectPool = new GameObject[2];
        weaponEffectPool[0] = playerManager.GetComponent<PlayerManager>().WeaponEffectPool;
        weaponEffectPool[1] = followerManager.GetComponent<FollowerManager>().WeaponEffectPool;


        baseTileMap = new TileMap[2];
        baseTileMap[0] = this.transform.GetChild(0).GetComponent<TileMap>();
        baseTileMap[1] = this.transform.GetChild(1).GetComponent<TileMap>();
        baseTileMap[0].gameObject.SetActive(true);
        baseTileMap[1].gameObject.SetActive(true);
        baseTileMap[0].Init(tileSpriteImageStorage);
        baseTileMap[1].Init(tileSpriteImageStorage);
        baseTileMap[0].ChangeTerrain(0, 0);
        baseTileMap[1].ChangeMatrix(0, 0);
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
        baseTileMap[0].ReleaseTerrain();
        baseTileMap[1].ReleaseTerrain();

        guider = playerManager.Guider;

        if (baseTileMap[0].gameObject.activeSelf == true)
        {
            if ((baseTileMap[0].Row + row - 2 < 1 ) || (baseTileMap[0].Row + row - 2 > 9))
            {
                discriminationState = RangeOut.rowRangeOut;
                if ((baseTileMap[0].Column + column - 2 < 1) || (baseTileMap[0].Column + column - 2 > 9))
                {
                    discriminationState = RangeOut.doubleRangeOut;
                }
            }
            else
            {
                discriminationState = RangeOut.nonRangeOut;
                if ((baseTileMap[0].Column + column - 2 < 1) || (baseTileMap[0].Column + column - 2 > 9))
                {
                    discriminationState = RangeOut.columnRangeOut;
                }
            }
        }
        else
        {
            // spawnManager의 상태 체크 부분
            if ((baseTileMap[1].Row + row - 2 < 1 ) || (baseTileMap[1].Row + row - 2 > 9))
            {
                discriminationState = RangeOut.rowRangeOut;
                if ((baseTileMap[1].Column + column - 2 < 1) || (baseTileMap[1].Column + column - 2 > 9))
                {
                    discriminationState = RangeOut.doubleRangeOut;
                }
            }
            else
            {
                discriminationState = RangeOut.nonRangeOut;
                if ((baseTileMap[1].Column + column - 2 < 1) || (baseTileMap[1].Column + column - 2 > 9))
                {
                    discriminationState = RangeOut.columnRangeOut;
                }
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
                    baseTileMap[1].ChangeTerrain(row - 2, column - 2);
                    baseTileMap[0].ChangeMatrix(row - 2, column - 2);
                    baseTileMap[0].gameObject.SetActive(false);
                }

                else
                {
                    baseTileMap[0].transform.position = transform.position;
                    baseTileMap[0].gameObject.SetActive(true);
                    baseTileMap[0].ChangeTerrain(row - 2, column - 2);
                    baseTileMap[1].ChangeMatrix(row - 2, column - 2);
                    baseTileMap[1].gameObject.SetActive(false);

                }
                break;

            case RangeOut.rowRangeOut:
                // 범위 벗어난 시점 충돌한 타일 맵이 0번이면
                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    // 벗어나지 않은 타일 동기화
                    baseTileMap[1].transform.position = baseTileMap[0].transform.position;
                    // 범위 벗어난 시점 충돌한 위치가 위쪽이라면
                    if (baseTileMap[0].Row <= 1)
                    {

                        baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                        baseTileMap[1].ChangeTerrain(4, 0);
                        baseTileMap[0].ChangeMatrix(4, 0);
                    }
                    // 범위 벗어난 시점 충돌한 위치가 아래쪽이라면
                    else
                    {
                        baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                        baseTileMap[1].ChangeTerrain(-5, 0);
                        baseTileMap[0].ChangeMatrix(-5, 0);
                    }
                    guider.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    playerAttackDirectional.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[0].Row ), 0);

                    for (int i = 0; i < followercounter; i++)
                    {

                        followerManager.transform.GetChild(i).transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            break;
                        }
                        monsterManager.transform.GetChild(i).Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    }
                }

                // 범위 벗어난 시점 충돌한 타일 맵이 1번이면
                else
                {
                    // 벗어나지 않은 타일 동기화
                    baseTileMap[0].transform.position = baseTileMap[1].transform.position;
                    // 범위 벗어난 시점 충돌한 위치가 위쪽이라면
                    if (baseTileMap[1].Row <= 1)
                    {
                        baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                        baseTileMap[0].ChangeTerrain(4, 0);
                        baseTileMap[1].ChangeMatrix(4, 0);
                    }
                    // 범위 벗어난 시점 충돌한 위치가 아래쪽이라면
                    else
                    {
                        baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                        baseTileMap[0].ChangeTerrain(-4, 0);
                        baseTileMap[1].ChangeMatrix(-4, 0);
                    }
                    guider.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    playerAttackDirectional.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[1].Row ), 0);

                    for (int i = 0; i < followercounter; i++)
                    {

                        followerManager.transform.GetChild(i).transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            break;
                        }
                        monsterManager.transform.GetChild(i).Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(0, 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    }
                }




                baseTileMap[0].Row = 5;
                baseTileMap[1].Row = 5;
                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);
                break;

            case RangeOut.columnRangeOut:
                // 범위 벗어난 시점 충돌한 타일 맵이 0번이면
                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    // 벗어나지 않은 타일 동기화
                    baseTileMap[1].transform.position = baseTileMap[0].transform.position;
                    // 범위 벗어난 시점 충돌한 위치가 왼쪽이라면
                    if (baseTileMap[0].Column <= 1)
                    {
                        baseTileMap[1].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                        baseTileMap[1].ChangeTerrain(0, 4);
                        baseTileMap[0].ChangeMatrix(0, 4);
                    }
                    // 범위 벗어난 시점 충돌한 위치가 오른쪽이라면
                    else
                    {
                        baseTileMap[1].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                        baseTileMap[1].ChangeTerrain(0, -4);
                        baseTileMap[0].ChangeMatrix(0, -4);
                    }
                    guider.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile(0, (5 - baseTileMap[0].Column));

                    for (int i = 0; i < followercounter; i++)
                    {
                        followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            break;
                        }
                        monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 0, 0, Space.Self);
                    }

                }

                // 범위 벗어난 시점 충돌한 타일 맵이 1번이면
                else
                {
                    // 벗어나지 않은 타일 동기화
                    baseTileMap[0].transform.position = baseTileMap[1].transform.position;
                    // 범위 벗어난 시점 충돌한 위치가 왼쪽이라면
                    if (baseTileMap[1].Column <= 1)
                    {
                        baseTileMap[0].transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                        baseTileMap[0].ChangeTerrain(0, 4);
                        baseTileMap[1].ChangeMatrix(0, 4);
                    }
                    // 범위 벗어난 시점 충돌한 위치가 오른쪽이라면
                    else
                    {
                        baseTileMap[0].transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                        baseTileMap[0].ChangeTerrain(0, -4);
                        baseTileMap[1].ChangeMatrix(0, -4);
                    }
                    guider.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile(0, (5 - baseTileMap[1].Column));


                    for (int i = 0; i < followercounter; i++)
                    {
                        followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            break;
                        }
                        monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[1].Column), 0, 0, Space.Self);
                    }

                }


                baseTileMap[0].Column = 5;
                baseTileMap[1].Column = 5;
                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);

                break;

            case RangeOut.doubleRangeOut:
                int x = 0; 
                int y = 0;

                // 범위 벗어난 시점 충돌한 타일 맵이 0번이면
                if (baseTileMap[0].gameObject.activeSelf == true)
                {
                    // 범위 벗어난 시점 충돌한 위치가 왼쪽이라면
                    if (baseTileMap[0].Column <= 1)
                    {
                        baseTileMap[1].transform.Translate(12.8f * (4 - baseTileMap[0].Column), 0, 0, Space.Self);
                        y = 4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 오른쪽이라면
                    else
                    { 
                        baseTileMap[1].transform.Translate(12.8f * (6 - baseTileMap[0].Column), 0, 0, Space.Self);
                        y = -4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 위쪽이라면
                    if (baseTileMap[0].Row <= 1)
                    {
                        baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 4), 0, Space.Self);
                        x = 4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 아래쪽이라면
                    else
                    {
                        baseTileMap[1].transform.Translate(0, 12.8f * (baseTileMap[0].Row - 6), 0, Space.Self);
                        x = -4;
                    }
                    baseTileMap[1].ChangeTerrain(x, y);
                    baseTileMap[0].ChangeMatrix(x, y);

                    guider.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[0].Column), (baseTileMap[0].Row - 5));

                    for (int i = 0; i < followercounter; i++)
                    {

                        followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            continue;
                        }
                        monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);

                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[0].Column), 12.8f * (baseTileMap[0].Row - 5), 0, Space.Self);
                    }
                }
                // 범위 벗어난 시점 충돌한 타일 맵이 1번이면
                else
                {
                    // 범위 벗어난 시점 충돌한 위치가 왼쪽이라면
                    if (baseTileMap[1].Column <= 1)
                    {
                        baseTileMap[0].transform.Translate(12.8f * (4 - baseTileMap[1].Column), 0, 0, Space.Self);
                        y = 4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 오른쪽이라면
                    else
                    {
                        baseTileMap[0].transform.Translate(12.8f * (6 - baseTileMap[1].Column), 0, 0, Space.Self);
                        y = -4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 위쪽이라면
                    if (baseTileMap[1].Row <= 1)
                    {
                        baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[1].Row - 4), 0, Space.Self);
                        x = 4;
                    }
                    // 범위 벗어난 시점 충돌한 위치가 아래쪽이라면
                    else
                    {
                        baseTileMap[0].transform.Translate(0, 12.8f * (baseTileMap[1].Row - 6), 0, Space.Self);
                        x = -4;
                    }
                    baseTileMap[0].ChangeTerrain(x, y);
                    baseTileMap[0].ChangeMatrix(x, y);

                    guider.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    playerAttackDirectional.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    cameraManager.MainMoveCamera.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.GoldSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.DarkEssenseSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    monsterManager.ExpSpawn.transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    treasureBoxEscapeStairManager.EventSwapTile((5 - baseTileMap[1].Column), (baseTileMap[1].Row - 5));

                    for (int i = 0; i < followercounter; i++)
                    {

                        followerManager.transform.GetChild(i).transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    }

                    for (int i = 0; i < monsterManager.MaxMonster; i++)
                    {
                        if (monsterManager.transform.GetChild(i).gameObject.activeSelf == false)
                        {
                            continue;
                        }
                        monsterManager.transform.GetChild(i).Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);

                    }

                    for (int i = 0; i < weaponEffectPool.Length; i++)
                    {
                        weaponEffectPool[i].transform.Translate(12.8f * (5 - baseTileMap[1].Column), 12.8f * (baseTileMap[1].Row - 5), 0, Space.Self);
                    }
                }



                baseTileMap[0].Row = 5;
                baseTileMap[1].Row = 5;
                baseTileMap[0].Column = 5;
                baseTileMap[1].Column = 5;
                baseTileMap[0].gameObject.SetActive(true);
                baseTileMap[1].gameObject.SetActive(true);

                //EditorApplication.isPaused = true;
                break;
        }
    }
}
