using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileSet
    : MonoBehaviour
{
    public int Row;
    public int Column;
    public int absRow;
    public int absColumn;
    public GameObject[] tile;
    public TileMap TileMap;
    public GameObject TileSpriteImageStorage;
    private TileSpriteImageStorage TileSpriteImageStorages;
    private void Awake()
    {
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
        tile[Mathf.Abs((int)(TileSpriteImageStorages.RandConst.x) * (absColumn % 4) - (int)(TileSpriteImageStorages.RandConst.y) * (absRow%4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorages.BaseTileSpriteImage;
        absRow += a;
        absColumn += b;
        tile[Mathf.Abs((int)(TileSpriteImageStorages.RandConst.x) * (absColumn%4) - (int)(TileSpriteImageStorages.RandConst.y) * (absRow % 4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorages.TileSpriteImage[Mathf.Abs((int)(TileSpriteImageStorages.RenderRandConst.x) * (absColumn % 4) - (int)(TileSpriteImageStorages.RenderRandConst.y) * (absRow % 4)) % 8];
    }
}
