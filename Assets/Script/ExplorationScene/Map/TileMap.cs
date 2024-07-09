using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public int Row;
    public int Column;

    public GameObject[] TileSet;

    public GameObject TileSpriteImageStorage;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i =0;i<9;i++)
        {
            TileSet[i] = this.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
