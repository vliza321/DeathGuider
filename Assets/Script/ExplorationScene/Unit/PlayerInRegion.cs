using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRegion : MonoBehaviour
{
    private TileSpawnManager tileSpawnManager;
    private bool triggerflag;
    private TileSet tileSet;
    // Start is called before the first frame update
    private void Awake()
    {
        triggerflag = true; 
        tileSpawnManager = this.transform.parent.parent.GetComponent<TileSpawnManager>();
        tileSet = this.gameObject.GetComponent<TileSet>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.transform.parent.name);

        int row;
        int column;
        if (collision.gameObject.CompareTag("Player") )
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
        Debug.Log(this.transform.parent.name);
        int row;
        int column;
        if (collision.gameObject.CompareTag("Player"))
        {
            row = tileSet.row;
            column = tileSet.column;
            if (triggerflag == false)
            {

                triggerflag = true;
            }
            else
            {
                tileSpawnManager.GetComponent<TileSpawnManager>().swapTileMap(this.transform, row, column);
            }
        }


    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(this.transform.parent.name);

        if (collision.gameObject.CompareTag("BaseTile"))
        {
            triggerflag = true;
        }

    }
}
