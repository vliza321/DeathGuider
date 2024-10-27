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

    public GameObject TileSpriteImageStorage;
    // Start is called before the first frame update

    public void Init()
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
            t.Init();
        }

    }

    public void ChangeTile(int row, int column)
    {
        foreach(var t in tileSet)
        {
            t.ChangeTile(row, column);
        }
    }
    public void SwapTileMap(int row, int column)
    {
        this.row += row;
        this.column += column;
    }
}
