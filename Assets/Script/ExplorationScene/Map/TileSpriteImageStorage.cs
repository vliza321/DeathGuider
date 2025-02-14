using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TileSpriteImageStorage: MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private Sprite[] tileSpriteImage;
    public Sprite[] TileSpriteImage
    {
        get { return tileSpriteImage; }
        set { tileSpriteImage = value; }
    }

    [SerializeField]
    private Sprite baseTileSpriteImage;
    public Sprite BaseTileSpriteImage
    {
        get { return baseTileSpriteImage; }
        set { baseTileSpriteImage = value; }
    }

    [SerializeField]
    private Vector2Int RandConst;
    public Vector2Int randConst
    {
        get { return RandConst; }
        set { RandConst = value; }
    }
    [SerializeField]
    private Vector2Int RenderRandConst;
    public Vector2Int renderRandConst
    {
        get { return RenderRandConst; }
        set { RenderRandConst = value; }
    }

    private TerrainSpawner terrainSpawner;
    
    public TerrainSpawner TerrainSpawner
    {
        get { return terrainSpawner; }
    }
    public void Init()
    {
        RandConst.x = Random.Range(5, 20);
        RandConst.y = Random.Range(5, 20);

        RenderRandConst.x = Random.Range(5, 20);
        RenderRandConst.y = Random.Range(5, 20);
        terrainSpawner = this.gameObject.GetComponent<TerrainSpawner>();

        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        baseTileSpriteImage = gameManager.BaseTileImg[gameManager.SelectStageID];
        terrainSpawner.Init(gameManager);
    }
}
