using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileSpawnManager : MonoBehaviour
{
    private GameObject[] BaseTileMap;
    [SerializeField]
    private GameObject TileSpriteImageStorage;
    private GameObject PlayerManager;
    private GameObject MonsterSpawnManager;
    private GameObject FollowerManager;
    private GameObject treasureBoxEscapeStairManager;
    private GameObject guider;
    private int followercounter;
    private GameObject PlayerAttackDirectional;
    private CameraManager cameraManager;


    private void Awake()
    {
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {    
            if (manager.name == "PlayerManager")
            {
                PlayerManager = manager.transform.gameObject;
            }
            if (manager.name == "MonsterSpawnManager")
            {
                MonsterSpawnManager = manager.transform.gameObject;
            }
            if (manager.name == "FollowerManager")
            {
                FollowerManager = manager.transform.gameObject;
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

        BaseTileMap = new GameObject[2];
        BaseTileMap[0] = this.transform.GetChild(0).gameObject;
        BaseTileMap[1] = this.transform.GetChild(1).gameObject;
    }
    // Update is called once per frame
    void Start()
    {
        guider = PlayerManager.GetComponent<PlayerManager>().Guider;
        followercounter = FollowerManager.GetComponent<FollowerManager>().FollowerCounter;
        BaseTileMap[1].SetActive(false);
        PlayerAttackDirectional = PlayerManager.transform.GetChild(1).gameObject;
    }

    public void swapTileMap(Transform transform, int row, int column)
    {
        guider = PlayerManager.GetComponent<PlayerManager>().Guider;
        BaseTileMap[0].GetComponent<TileMap>().Row += row - 2;
        BaseTileMap[0].GetComponent<TileMap>().Column += column - 2;
        BaseTileMap[1].GetComponent<TileMap>().Row += row - 2;
        BaseTileMap[1].GetComponent<TileMap>().Column += column - 2;

        if (BaseTileMap[0].GetComponent<TileMap>().Row <= 0 || BaseTileMap[1].GetComponent<TileMap>().Row <= 0)
        {
            BaseTileMap[0].GetComponent<TileMap>().Row = 5;
            BaseTileMap[1].GetComponent<TileMap>().Row = 5;
            if (BaseTileMap[0].activeSelf == true)
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);

            }
            else
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y - 12.8f * 4, guider.transform.position.z);
            PlayerAttackDirectional.transform.position = new Vector3(PlayerAttackDirectional.transform.position.x, PlayerAttackDirectional.transform.position.y - 12.8f * 4, PlayerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x, cameraManager.MainMoveCamera.transform.position.y - 12.8f * 4, cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter;i++)
            {
                FollowerManager.transform.GetChild(i).transform.position = new Vector3(FollowerManager.transform.GetChild(i).transform.position.x, FollowerManager.transform.GetChild(i).transform.position.y - 12.8f * 4, FollowerManager.transform.GetChild(i).transform.position.z);
            }
            for(int i = 0; i < MonsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                MonsterSpawnManager.transform.GetChild(i).position = new Vector3(MonsterSpawnManager.transform.GetChild(i).position.x, MonsterSpawnManager.transform.GetChild(i).position.y - 12.8f * 4, MonsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(4, 0);
        }
        else if (BaseTileMap[0].GetComponent<TileMap>().Column <= 0 || BaseTileMap[1].GetComponent<TileMap>().Column <= 0)
        {
            //EditorApplication.isPaused = true;

            BaseTileMap[0].GetComponent<TileMap>().Column = 5;
            BaseTileMap[1].GetComponent<TileMap>().Column = 5;
            if (BaseTileMap[0].activeSelf == true)
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[1].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            else
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x + 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            PlayerAttackDirectional.transform.position = new Vector3(PlayerAttackDirectional.transform.position.x + 12.8f * 4, PlayerAttackDirectional.transform.position.y, PlayerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x + 12.8f * 4, cameraManager.MainMoveCamera.transform.position.y , cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                FollowerManager.transform.GetChild(i).transform.position = new Vector3(FollowerManager.transform.GetChild(i).transform.position.x + 12.8f * 4, FollowerManager.transform.GetChild(i).transform.position.y, FollowerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < MonsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                MonsterSpawnManager.transform.GetChild(i).position = new Vector3(MonsterSpawnManager.transform.GetChild(i).position.x + 12.8f * 4, MonsterSpawnManager.transform.GetChild(i).position.y , MonsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0, 4);
        }

        else if (BaseTileMap[0].GetComponent<TileMap>().Row >= 10 || BaseTileMap[1].GetComponent<TileMap>().Row >= 10)
        {
            BaseTileMap[0].GetComponent<TileMap>().Row = 5;
            BaseTileMap[1].GetComponent<TileMap>().Row = 5;
            if (BaseTileMap[0].activeSelf == true)
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            else
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y + 12.8f * 4, guider.transform.position.z);
            PlayerAttackDirectional.transform.position = new Vector3(PlayerAttackDirectional.transform.position.x , PlayerAttackDirectional.transform.position.y + 12.8f * 4, PlayerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x , cameraManager.MainMoveCamera.transform.position.y + 12.8f * 4, cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                FollowerManager.transform.GetChild(i).transform.position = new Vector3(FollowerManager.transform.GetChild(i).transform.position.x, FollowerManager.transform.GetChild(i).transform.position.y + 12.8f * 4, FollowerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < MonsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                MonsterSpawnManager.transform.GetChild(i).position = new Vector3(MonsterSpawnManager.transform.GetChild(i).position.x, MonsterSpawnManager.transform.GetChild(i).position.y + 12.8f * 4, MonsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(-4, 0);
        }
        else if (BaseTileMap[0].GetComponent<TileMap>().Column >= 10 || BaseTileMap[1].GetComponent<TileMap>().Column >= 10)
        {
            BaseTileMap[0].GetComponent<TileMap>().Column = 5;
            BaseTileMap[1].GetComponent<TileMap>().Column = 5;
            if (BaseTileMap[0].activeSelf == true)
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[1].transform.position = new Vector3( 0, transform.parent.transform.position.y ,transform.parent.position.z);
            }
            else
            {
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(true);
                BaseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            }
            guider.transform.position = new Vector3(guider.transform.position.x - 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            PlayerAttackDirectional.transform.position = new Vector3(PlayerAttackDirectional.transform.position.x - 12.8f * 4, PlayerAttackDirectional.transform.position.y , PlayerAttackDirectional.transform.position.z);
            cameraManager.MainMoveCamera.transform.position = new Vector3(cameraManager.MainMoveCamera.transform.position.x - 12.8f * 4, cameraManager.MainMoveCamera.transform.position.y , cameraManager.MainMoveCamera.transform.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                FollowerManager.transform.GetChild(i).transform.position = new Vector3(FollowerManager.transform.GetChild(i).transform.position.x - 12.8f * 4, FollowerManager.transform.GetChild(i).transform.position.y , FollowerManager.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < MonsterSpawnManager.GetComponent<MonsterManager>().EnabledMonster; i++)
            {
                MonsterSpawnManager.transform.GetChild(i).position = new Vector3(MonsterSpawnManager.transform.GetChild(i).position.x - 12.8f * 4, MonsterSpawnManager.transform.GetChild(i).position.y, MonsterSpawnManager.transform.GetChild(i).position.z);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0,-4);
        }
        else
        {
            if (BaseTileMap[0].activeSelf == true)
            {
                BaseTileMap[1].transform.position = transform.position;
                BaseTileMap[1].SetActive(true);
                BaseTileMap[0].SetActive(false);
                for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
                {
                    BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                    BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                }
            }
            else
            {
                BaseTileMap[0].transform.position = transform.position;
                BaseTileMap[0].SetActive(true);
                BaseTileMap[1].SetActive(false);
                for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
                {
                    BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                    BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(row - 2, column - 2);
                }
            }
        }
    }
}
