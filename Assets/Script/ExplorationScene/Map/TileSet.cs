using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileSet : MonoBehaviour
{
    [SerializeField]
    private int Row;
    [SerializeField]
    private int Column;
    public int row
    {
        get { return Row; }
        set { Row = value; } 
    }
    public int column
    {
        get { return Column; }
        set { Column = value; }
    }
    [SerializeField]
    private int absRow;
    [SerializeField]
    private int absColumn;
    [SerializeField]
    private SpriteRenderer[] tile;
    private TileMap TileMap;

    private TileSpriteImageStorage TileSpriteImageStorages;

    private TerrainSpawner terrainSpawner;

    [SerializeField]
    private int constant;

    private GameObject terrain;
    public void Init(TileSpriteImageStorage tileSpriteImageStorage)
    {
        constant = 0;
        tile = new SpriteRenderer[this.transform.childCount];
        TileMap = this.transform.parent.gameObject.GetComponent<TileMap>();
        TileSpriteImageStorages = tileSpriteImageStorage;
        terrainSpawner = TileSpriteImageStorages.TerrainSpawner;
        for (int i = 0; i < tile.Length; i++)
        {
            tile[i] = this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            tile[i].GetComponent<SpriteRenderer>().sprite = TileSpriteImageStorages.BaseTileSpriteImage;
        }
        absRow = this.Row + TileMap.Row - 2;
        absColumn = this.Column + TileMap.Column - 2;
        //ChangeTile(0, 0);

    }

    public void ChangeTerrain(int a, int b)
    {
        ReleaseTerrain();
        ChangeMatrix(a, b);
        constant = (TileSpriteImageStorages.randConst.x) * (absColumn % 4) - (TileSpriteImageStorages.randConst.y) * (absRow % 4);
        constant = (constant >= 0 ? constant : -(constant));

        terrain = terrainSpawner.GetTerrain(constant % 8, tile[constant%100].transform.position);
    }

    public void ChangeMatrix(int a, int b)
    {
        absRow += a;
        absColumn += b;
    }

    public void ReleaseTerrain()
    {
        if (terrain == null)
        {
            return;
        }
        terrain.SetActive(false);
        terrainSpawner.ReleaseTerrain(constant % 8, terrain);
    }
}
