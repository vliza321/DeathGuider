using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRegion : MonoBehaviour
{
    private TileSpawnManager tileSpawnManager;
    private bool triggerflag;
    private TileSet tileSet;
    private const string playerTagName = "Player";
    // Start is called before the first frame update
    private void Awake()
    {
        triggerflag = true; 
        tileSpawnManager = this.transform.parent.parent.GetComponent<TileSpawnManager>();
        tileSet = this.gameObject.GetComponent<TileSet>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        int row;
        int column;
        if (collision.gameObject.CompareTag(playerTagName) )
        {
            row = tileSet.row;
            column = tileSet.column;

            if (triggerflag == false)
            {
                triggerflag = true;           
            }
            else
            {
                tileSpawnManager.swapTileMap(this.transform,row,column );
                triggerflag = false;
            }
        }
        

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        int row;
        int column;
        if (collision.gameObject.CompareTag(playerTagName))
        {
            row = tileSet.row;
            column = tileSet.column;
            if (triggerflag == false)
            {

                triggerflag = true;
            }
            else
            {
                tileSpawnManager.swapTileMap(this.transform, row, column);
            }
        }


    }
}
