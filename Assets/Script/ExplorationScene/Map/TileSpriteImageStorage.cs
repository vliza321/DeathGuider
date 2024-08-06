using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteImageStorage
    : MonoBehaviour
{
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

    private Vector2Int RandConst;
    public Vector2Int randConst
    {
        get { return RandConst; }
        set { RandConst = value; }
    }
    private Vector2Int RenderRandConst;
    public Vector2Int renderRandConst
    {
        get { return RenderRandConst; }
        set { RenderRandConst = value; }
    }

    private void Awake()
    {
        RandConst.x = Random.Range(5, 20);
        RandConst.y = Random.Range(5, 20);

        RenderRandConst.x = Random.Range(5, 20);
        RenderRandConst.y = Random.Range(5, 20);
    }
}
