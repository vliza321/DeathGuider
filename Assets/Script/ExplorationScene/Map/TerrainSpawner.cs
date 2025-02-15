using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private Dictionary<int, Queue<GameObject>> terrainsDic;

    private GameObject cachingObject;
    // Start is called before the first frame update
    public void Init(GameManager gameManager)
    {
        
        terrainsDic = new Dictionary<int, Queue<GameObject>>();

        int stageID = gameManager.SelectStageID;
        for (int i = 0; i < 9; i++)
        {
            Queue<GameObject> newList = new Queue<GameObject>();
            int spawnTerrainID = stageID * 10 + i;
            for (int j = 0; j < 18; j++)
            {
                GameObject newTerrain = Instantiate(gameManager.Terrain[spawnTerrainID],this.transform);
                newTerrain.transform.position = Vector3.zero;
                newTerrain.SetActive(false);
                newList.Enqueue(newTerrain);
            }
            terrainsDic.Add(i, newList);
        }
    }

    public GameObject GetTerrain(int terrainID, Vector3 position)
    {
        cachingObject = terrainsDic[terrainID].Dequeue();
        cachingObject.transform.position = position;
        cachingObject.SetActive(true);
        return cachingObject;
    }

    public void ReleaseTerrain(int terrainID, GameObject terrain)
    {
        terrain.SetActive(false);
        terrainsDic[terrainID].Enqueue(terrain);
        terrain.transform.position = Vector3.zero;
    }
}
