using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [SerializeField]
    private int row;
    [SerializeField]
    private int column;
    public int Row
    { get { return row; } set { row = value; } }
    public int Column
    { get { return column; } set { column = value; } }
    [SerializeField]
    private Vector2Int mapSize;
    public Vector2Int MapSIze
    {
        get { return mapSize; }
        set { mapSize = value; }
    }

    private TileSet[] tileSet;
    public TileSet[] TileSet
    {
        get { return tileSet; }
        set {  tileSet = value; }
    }

    public void Init(TileSpriteImageStorage tileSpriteImageStorage)
    {
        TileSet = new TileSet[this.transform.childCount];
        row = (int)(mapSize.x / 2);
        column = (int)(mapSize.y / 2);
        for (int i = 0; i < 9; i++)
        {
            tileSet[i] = this.transform.GetChild(i).gameObject.GetComponent<TileSet>();
        }
        foreach (var t in tileSet)
        {
            t.Init(tileSpriteImageStorage);
        }

    }


    public void ReleaseTerrain()
    {
        foreach (var t in tileSet)
        {
            t.ReleaseTerrain();
        }
    }

    public void ChangeTerrain(int row, int column)
    {        
        foreach(var t in tileSet)
        {
            t.ChangeTerrain(row, column);
        }
    }

    public void ChangeMatrix(int row, int column)
    {
        foreach (var t in tileSet)
        {
            t.ChangeMatrix(row, column);
        }
    }

    public void SwapTileMap(int row, int column)
    {
        this.row += row;
        this.column += column;
    }
}
