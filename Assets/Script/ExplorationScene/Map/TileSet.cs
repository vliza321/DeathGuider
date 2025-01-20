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

    public void Init(TileSpriteImageStorage tileSpriteImageStorage)
    {
        tile = new SpriteRenderer[this.transform.childCount];
        TileMap = this.transform.parent.gameObject.GetComponent<TileMap>();
        TileSpriteImageStorages = tileSpriteImageStorage;
        for (int i = 0; i < tile.Length; i++)
        {
            tile[i] = this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            tile[i].GetComponent<SpriteRenderer>().sprite = TileSpriteImageStorages.BaseTileSpriteImage;
        }
        absRow = this.Row + TileMap.Row - 2;
        absColumn = this.Column + TileMap.Column - 2;
        ChangeTile(0, 0);

    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        absRow = this.Row + TileMap.Row - 2;
        absColumn = this.Column + TileMap.Column - 2;
        Debug.Log("start");
        ChangeTile(0, 0);
        */
    }

    public void ChangeTile(int a, int b)
    {       
        tile[  ((TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5) >= 0 
            ? (TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5) 
            : - ((TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5)))
            % 100].sprite
           = TileSpriteImageStorages.BaseTileSpriteImage;
       
        absRow += a;
        absColumn += b;

        tile[((TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5) >= 0
            ? (TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5)
            : -((TileSpriteImageStorages.randConst.x) * (absColumn % 5) - (TileSpriteImageStorages.randConst.y) * (absRow % 5)))
            % 100].sprite
           = TileSpriteImageStorages.TileSpriteImage[
             ((TileSpriteImageStorages.renderRandConst.x) * (absColumn % 5) - (TileSpriteImageStorages.renderRandConst.y) * (absRow % 5) >= 0
            ? (TileSpriteImageStorages.renderRandConst.x) * (absColumn % 5) - (TileSpriteImageStorages.renderRandConst.y) * (absRow % 5)
            : -((TileSpriteImageStorages.renderRandConst.x) * (absColumn % 5) - (TileSpriteImageStorages.renderRandConst.y) * (absRow % 5)))
            % 8];
    }
}
