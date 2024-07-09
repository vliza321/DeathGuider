using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRegion : MonoBehaviour
{
    GameObject Player;
    public GameObject TileManager;
    public bool triggerflag;
    // Start is called before the first frame update
    void Awake()
    {
        Player = this.gameObject;
        triggerflag = true;

    }

    void Start()
    {



    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int row;
        int column;
        if (collision.gameObject.CompareTag("TileSet") )
        {
            row = collision.GetComponent<TileSet>().Row;
            column = collision.GetComponent<TileSet>().Column;

            if (triggerflag == false)
            {
                triggerflag = true;           
            }
            else
            {
                TileManager.GetComponent<TileSpawnManager>().swapTileMap(collision.transform,row,column );
                triggerflag = false;
            }
        }
        

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        int row;
        int column;
        if (collision.gameObject.CompareTag("TileSet"))
        {
            row = collision.GetComponent<TileSet>().Row;
            column = collision.GetComponent<TileSet>().Column;
            if (triggerflag == false)
            {

                triggerflag = true;
            }
            else
            {
                TileManager.GetComponent<TileSpawnManager>().swapTileMap(collision.transform, row, column);
            }
        }
        if (collision.gameObject.CompareTag("BaseTile"))
        {
            triggerflag = true;
        }

    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BaseTile"))
        {
            triggerflag = true;
        }

    }
}
