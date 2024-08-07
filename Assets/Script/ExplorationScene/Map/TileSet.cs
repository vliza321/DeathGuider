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

    private int absRow;
    private int absColumn;
    [SerializeField]
    private GameObject[] tile;
    private TileMap TileMap;
    [SerializeField]
    private GameObject TileSpriteImageStorage;
    private TileSpriteImageStorage TileSpriteImageStorages;
    private void Awake()
    {
        tile = new GameObject[this.transform.childCount];
        TileMap = this.transform.parent.gameObject.GetComponent<TileMap>();
        TileSpriteImageStorages = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>();
        for (int i = 0;i<tile.Length;i++)
        {
            tile[i] = this.transform.GetChild(i).gameObject;
            tile[i].GetComponent<SpriteRenderer>().sprite = TileSpriteImageStorages.BaseTileSpriteImage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        absRow = this.Row + TileMap.Row;
        absColumn = this.Column + TileMap.Column;
        ChangeTile(0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeTile(int a, int b)
    {
        tile[Mathf.Abs((int)(TileSpriteImageStorages.randConst.x) * (absColumn % 4) - (int)(TileSpriteImageStorages.randConst.y) * (absRow%4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorages.BaseTileSpriteImage;
        absRow += a;
        absColumn += b;
        tile[Mathf.Abs((int)(TileSpriteImageStorages.randConst.x) * (absColumn%4) - (int)(TileSpriteImageStorages.randConst.y) * (absRow % 4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorages.TileSpriteImage[Mathf.Abs((int)(TileSpriteImageStorages.renderRandConst.x) * (absColumn % 4) - (int)(TileSpriteImageStorages.renderRandConst.y) * (absRow % 4)) % 8];
    }
}
