using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteImageStorage
    : MonoBehaviour
{
    public Sprite[] TileSpriteImage;
    public Sprite BaseTileSpriteImage;
    public int[] RandConst;
    public int[] RenderRandConst;

    private void Awake()
    {
        for(int i=0;i<RandConst.Length;i++)
        {
            RandConst[i] = Random.Range(5, 20);
            RenderRandConst[i] = Random.Range(5, 20);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
