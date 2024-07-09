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
    
    private void Awake()
    {
        TileMap = this.transform.parent.gameObject.GetComponent<TileMap>();
        for(int i = 0;i<tile.Length;i++)
        {
            tile[i] = this.transform.GetChild(i).gameObject;
            tile[i].GetComponent<SpriteRenderer>().sprite = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().BaseTileSpriteImage;
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
        tile[Mathf.Abs(TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst[0] * (absColumn % 4) - TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst[1] * (absRow%4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().BaseTileSpriteImage;
        absRow += a;
        absColumn += b;
        tile[Mathf.Abs(TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst[0] * (absColumn%4) - TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst[1] * (absRow % 4)) % 100].GetComponent<SpriteRenderer>().sprite
           = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().TileSpriteImage[Mathf.Abs(TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RenderRandConst[0] * (absColumn % 4) - TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RenderRandConst[1] * (absRow % 4)) % 8];
    }
}
