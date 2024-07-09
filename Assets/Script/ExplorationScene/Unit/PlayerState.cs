using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool alive; // 살았는지 죽었는지만
    public bool canmove; // 움직이는지 멈췄는지만
    public int dontMoveTimer;
    // Start is called before the first frame update

    private void Awake()
    {
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }
    void Start()
    {
        alive = true;
        canmove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dontMoveTimer > 0)
        {
            dontMoveTimer--;
        }
        if (dontMoveTimer <= 0)
        {
            canmove = true;
        }
        
    }

    public void dontMove(int dontmovetimer)
    {
        dontMoveTimer = dontmovetimer;
        canmove = false;
    }
}
