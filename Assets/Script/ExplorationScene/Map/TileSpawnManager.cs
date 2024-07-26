using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnManager : MonoBehaviour
{
    public GameObject[] BaseTileMap;
    public GameObject TileSpriteImageStorage;
    public GameObject Player;
    public GameObject EnableMonsters;
    public GameObject Follower;
    public GameObject treasureBoxEscapeStairManager;
    GameObject guider;
    int followercounter;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Start()
    {
        guider = Player.GetComponent<PlayerSwap>().Guider;
        followercounter = Follower.GetComponent<Follower>().followercounter;
    }

    public void swapTileMap(Transform transform, int row, int column)
    {
        guider = Player.GetComponent<PlayerSwap>().Guider;
        BaseTileMap[0].GetComponent<TileMap>().Row += row - 2;
        BaseTileMap[0].GetComponent<TileMap>().Column += column - 2;
        BaseTileMap[1].GetComponent<TileMap>().Row += row - 2;
        BaseTileMap[1].GetComponent<TileMap>().Column += column - 2;

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



        if (BaseTileMap[0].GetComponent<TileMap>().Row <= 0 || BaseTileMap[1].GetComponent<TileMap>().Row <= 0)
        {
            BaseTileMap[0].SetActive(true);
            BaseTileMap[1].SetActive(true);
            BaseTileMap[0].GetComponent<TileMap>().Row = 5;
            BaseTileMap[1].GetComponent<TileMap>().Row = 5;
            BaseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y - 12.8f * 4, guider.transform.position.z);
            BaseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            for(int i = 0; i <followercounter;i++)
            {
                Follower.transform.GetChild(i).transform.position = new Vector3(Follower.transform.GetChild(i).transform.position.x, Follower.transform.GetChild(i).transform.position.y - 12.8f * 4, Follower.transform.GetChild(i).transform.position.z);
            }
            for(int i = 0; i < EnableMonsters.GetComponent<MonsterSpawn>().EnabledMonster; i++)
            {
                EnableMonsters.transform.GetChild(i).position = new Vector3(EnableMonsters.transform.GetChild(i).position.x, EnableMonsters.transform.GetChild(i).position.y - 12.8f * 4, EnableMonsters.transform.GetChild(i).position.y);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(5,0);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(5, 0);
        }
        if (BaseTileMap[0].GetComponent<TileMap>().Column <= 0 || BaseTileMap[1].GetComponent<TileMap>().Column <= 0)
        {
            BaseTileMap[0].SetActive(true);
            BaseTileMap[1].SetActive(true); 
            BaseTileMap[0].GetComponent<TileMap>().Column = 5;
            BaseTileMap[1].GetComponent<TileMap>().Column = 5;
            BaseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            guider.transform.position = new Vector3(guider.transform.position.x + 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            BaseTileMap[1].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                Follower.transform.GetChild(i).transform.position = new Vector3(Follower.transform.GetChild(i).transform.position.x + 12.8f * 4, Follower.transform.GetChild(i).transform.position.y, Follower.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < EnableMonsters.GetComponent<MonsterSpawn>().EnabledMonster; i++)
            {
                EnableMonsters.transform.GetChild(i).position = new Vector3(EnableMonsters.transform.GetChild(i).position.x + 12.8f * 4, EnableMonsters.transform.GetChild(i).position.y , EnableMonsters.transform.GetChild(i).position.y);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0, 5);
        }

        if (BaseTileMap[0].GetComponent<TileMap>().Row >= 10 || BaseTileMap[1].GetComponent<TileMap>().Row >= 10)
        {
            BaseTileMap[0].SetActive(true);
            BaseTileMap[1].SetActive(true); 
            BaseTileMap[0].GetComponent<TileMap>().Row = 5;
            BaseTileMap[1].GetComponent<TileMap>().Row = 5;
            BaseTileMap[0].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            guider.transform.position = new Vector3(guider.transform.position.x, guider.transform.position.y + 12.8f * 4, guider.transform.position.z);
            BaseTileMap[1].transform.position = new Vector3(transform.parent.transform.position.x, 0, transform.parent.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                Follower.transform.GetChild(i).transform.position = new Vector3(Follower.transform.GetChild(i).transform.position.x, Follower.transform.GetChild(i).transform.position.y + 12.8f * 4, Follower.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < EnableMonsters.GetComponent<MonsterSpawn>().EnabledMonster; i++)
            {
                EnableMonsters.transform.GetChild(i).position = new Vector3(EnableMonsters.transform.GetChild(i).position.x, EnableMonsters.transform.GetChild(i).position.y + 12.8f * 4, EnableMonsters.transform.GetChild(i).position.y);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile( -5,0);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(-5, 0);
        }
        if (BaseTileMap[0].GetComponent<TileMap>().Column >= 10 || BaseTileMap[1].GetComponent<TileMap>().Column >= 10)
        {
            BaseTileMap[0].SetActive(true);
            BaseTileMap[1].SetActive(true); 
            BaseTileMap[0].GetComponent<TileMap>().Column = 5;
            BaseTileMap[1].GetComponent<TileMap>().Column = 5;
            BaseTileMap[0].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            guider.transform.position = new Vector3(guider.transform.position.x - 12.8f * 4, guider.transform.position.y, guider.transform.position.z);
            BaseTileMap[1].transform.position = new Vector3(0, transform.parent.transform.position.y, transform.parent.position.z);
            for (int i = 0; i < followercounter; i++)
            {
                Follower.transform.GetChild(i).transform.position = new Vector3(Follower.transform.GetChild(i).transform.position.x - 12.8f * 4, Follower.transform.GetChild(i).transform.position.y , Follower.transform.GetChild(i).transform.position.z);
            }
            for (int i = 0; i < EnableMonsters.GetComponent<MonsterSpawn>().EnabledMonster; i++)
            {
                EnableMonsters.transform.GetChild(i).position = new Vector3(EnableMonsters.transform.GetChild(i).position.x - 12.8f * 4, EnableMonsters.transform.GetChild(i).position.y, EnableMonsters.transform.GetChild(i).position.y);
            }
            for (int i = 0; i < BaseTileMap[0].GetComponent<TileMap>().TileSet.Length; i++)
            {
                BaseTileMap[0].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
                BaseTileMap[1].GetComponent<TileMap>().TileSet[i].GetComponent<TileSet>().ChangeTile(0,-5);
            }
            treasureBoxEscapeStairManager.GetComponent<TreasureBoxEscapeStairManager>().EventSwapTile(0,-5);
        }
    }
}
