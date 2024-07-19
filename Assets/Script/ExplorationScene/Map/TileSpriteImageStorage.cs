using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteImageStorage
    : MonoBehaviour
{
    public Sprite[] TileSpriteImage;
    public Sprite BaseTileSpriteImage;
    public Vector2 RandConst;
    public Vector2 RenderRandConst;

    private void Awake()
    {
        RandConst.x = Random.Range(5, 20);
        RandConst.y = Random.Range(5, 20);

        RenderRandConst.x = Random.Range(5, 20);
        RenderRandConst.y = Random.Range(5, 20);
    }
}
