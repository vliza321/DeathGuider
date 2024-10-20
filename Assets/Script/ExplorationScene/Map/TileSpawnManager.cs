using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileSpawnManager : MonoBehaviour
{
    private GameObject[] baseTileMap;
    private GameObject playerManager;
    private GameObject monsterSpawnManager;
    private GameObject followerManager;
    private GameObject treasureBoxEscapeStairManager;
    private GameObject guider;
    private int followercounter;
    private GameObject playerAttackDirectional;
    private CameraManager cameraManager;

    private GameObject[] weaponEffectPool;
    private void Awake()
    {
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {    
            if (manager.name == "PlayerManager")
            {
                playerManager = manager.transform.gameObject;
            }
            if (manager.name == "MonsterSpawnManager")
            {
                monsterSpawnManager = manager.transform.gameObject;
            }
            if (manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject;
            }
            if (manager.name == "TreasureBoxEscapeStairManager")
            {
                treasureBoxEscapeStairManager = manager.transform.gameObject;
            }
            if (manager.name == "CameraManager")
            {
                cameraManager = manager.transform.gameObject.GetComponent<CameraManager>();
            }
        }
        Manager = null;

        weaponEffectPool = new GameObject[2];
        weaponEffectPool[0] = playerManager.GetComponent<PlayerManager>().WeaponEffectPool;
        weaponEffectPool[1] = followerManager.GetComponent<FollowerManager>().WeaponEffectPool;

        baseTileMap = new GameObject[2];
        baseTileMap[0] = this.transform.GetChild(0).gameObject;
        baseTileMap[1] = this.transform.GetChild(1).gameObject;
        baseTileMap[0].SetActive(true);
        baseTileMap[1].SetActive(true);
        baseTileMap[0].GetComponent<TileMap>().Init();
        baseTileMap[1].GetComponent<TileMap>().Init();
    }
    // Update is called once per frame
    void Start()
    {
        guider = playerManager.GetComponent<PlayerManager>().Guider;
        followercounter = followerManager.GetComponent<FollowerManager>().FollowerCounter;
        playerAttackDirectional = playerManager.transform.GetChild(1).gameObject;
        baseTileMap[1].SetActive(false);
    }

    public void swapTileMap(Transform transform, int row, int column)
    {
        guider = playerManager.GetComponent<PlayerManager>().Guider;
        baseTileMap[0].GetComponent<TileMap>().Row += row - 2;
        baseTileMap[0].GetComponent<TileMap>().Column += column - 2;
        baseTileMap[1].GetComponent<TileMap>().Row += row - 2;
        baseTileMap[1].GetComponent<TileMap>().Column += column - 2;

        if (baseTileMap[0].GetComponent<TileMap>().Row <= 0 || baseTileMap[1].GetComponent<TileMap>().Row <= 0)
        {
            baseTileMap[0].GetComponent<TileMap>().Row = 5;
            baseTileMap[1].GetComponent<TileMap>().Row = 5;
            if (baseTileMap[0].activeSelf == true)
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);

            }
            else
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y - 12.8f * 4, guider.transform.position.z);
            playerAttackDirectional.transform.position = new Vector3(playerAttackDirectional.transform.position.x, playerAttackDirectional.transform.position.y - 12.8f * 4, playerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x, cameraManager.MainMoveCamera.transform.position.y - 12.8f * 4, cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter;i++)
            {
                followerManager.transform.GetChild(i).transform.position = new Vector3(followerManager.transform.GetChild(i).transform.position.x, followerManager.transform.GetChild(i).transform.position.y - 12.8f * 4, followerManager.transform.GetChild(i).transform.position.z);
            }
            for(int i = 0; i < monsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                monsterSpawnManager.transform.GetChild(i).position = new Vector3(monsterSpawnManager.transform.GetChild(i).position.x, monsterSpawnManager.transform.GetChild(i).position.y - 12.8f * 4, monsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
                baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
            }
            for(int i = 0; i< weaponEffectPool.Length;i++)
            {
                for(int a = 0; a < weaponEffectPool[i].transform.childCount;a++)
                {
                    weaponEffectPool[i].transform.GetChild(a).position = new Vector3(weaponEffectPool[i].transform.GetChild(a).position.x, weaponEffectPool[i].transform.GetChild(a).position.y, weaponEffectPool[i].transform.GetChild(a).position.z);
                }
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(4, 0);
        }
        else if (baseTileMap[0].GetComponent<TileMap>().Column <= 0 || baseTileMap[1].GetComponent<TileMap>().Column <= 0)
        {
            //EditorApplication.isPaused = true;

            baseTileMap[0].GetComponent<TileMap>().Column = 5;
            baseTileMap[1].GetComponent<TileMap>().Column = 5;
            if (baseTileMap[0].activeSelf == true)
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[1].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            else
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x + 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            playerAttackDirectional.transform.position = new Vector3(playerAttackDirectional.transform.position.x + 12.8f * 4, playerAttackDirectional.transform.position.y, playerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x + 12.8f * 4, cameraManager.MainMoveCamera.transform.position.y , cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                followerManager.transform.GetChild(i).transform.position = new Vector3(followerManager.transform.GetChild(i).transform.position.x + 12.8f * 4, followerManager.transform.GetChild(i).transform.position.y, followerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < monsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                monsterSpawnManager.transform.GetChild(i).position = new Vector3(monsterSpawnManager.transform.GetChild(i).position.x + 12.8f * 4, monsterSpawnManager.transform.GetChild(i).position.y , monsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
                baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0, 4);
        }

        else if (baseTileMap[0].GetComponent<TileMap>().Row >= 10 || baseTileMap[1].GetComponent<TileMap>().Row > 10)
        {
            baseTileMap[0].GetComponent<TileMap>().Row = 5;
            baseTileMap[1].GetComponent<TileMap>().Row = 5;
            if (baseTileMap[0].activeSelf == true)
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            else
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y + 12.8f * 4, guider.transform.position.z);
            playerAttackDirectional.transform.position = new Vector3(playerAttackDirectional.transform.position.x , playerAttackDirectional.transform.position.y + 12.8f * 4, playerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x , cameraManager.MainMoveCamera.transform.position.y + 12.8f * 4, cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                followerManager.transform.GetChild(i).transform.position = new Vector3(followerManager.transform.GetChild(i).transform.position.x, followerManager.transform.GetChild(i).transform.position.y + 12.8f * 4, followerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < monsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                monsterSpawnManager.transform.GetChild(i).position = new Vector3(monsterSpawnManager.transform.GetChild(i).position.x, monsterSpawnManager.transform.GetChild(i).position.y + 12.8f * 4, monsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
                baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(-4, 0);
        }
        else if (baseTileMap[0].GetComponent<TileMap>().Column >= 10 || baseTileMap[1].GetComponent<TileMap>().Column > 10)
        {
            baseTileMap[0].GetComponent<TileMap>().Column = 5;
            baseTileMap[1].GetComponent<TileMap>().Column = 5;
            if (baseTileMap[0].activeSelf == true)
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[1].transform.position = new Vector3( 0, transform.parent.transform.position.y ,transform.parent.position.z);
            }
            else
            {
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(true);
                baseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x - 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            playerAttackDirectional.transform.position = new Vector3(playerAttackDirectional.transform.position.x - 12.8f * 4, playerAttackDirectional.transform.position.y , playerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x - 12.8f * 4, cameraManager.MainMoveCamera.transform.position.y , cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                followerManager.transform.GetChild(i).transform.position = new Vector3(followerManager.transform.GetChild(i).transform.position.x - 12.8f * 4, followerManager.transform.GetChild(i).transform.position.y , followerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < monsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                monsterSpawnManager.transform.GetChild(i).position = new Vector3(monsterSpawnManager.transform.GetChild(i).position.x - 12.8f * 4, monsterSpawnManager.transform.GetChild(i).position.y, monsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
                baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0,-4);
        }
        else
        {
            if (baseTileMap[0].activeSelf == true)
            {
                baseTileMap[1].transform.position = transform.position;
                baseTileMap[1].SetActive(true);
                baseTileMap[0].SetActive(false);
                for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
                {
                    baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                    baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                }
            }
            else
            {
                baseTileMap[0].transform.position = transform.position;
                baseTileMap[0].SetActive(true);
                baseTileMap[1].SetActive(false);
                for (int i = 0; i < baseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
                {
                    baseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                    baseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                }
            }
        }
    }
}
