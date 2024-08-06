using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private bool alive; // 살았는지 죽었는지만
    private bool canMove; // 움직이는지 멈췄는지만
    private int dontMoveTimer;
    // Start is called before the first frame update
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public bool Alive
    {
        get { return alive; }
        set { alive = value; }
    }

    private void Awake()
    {
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }
    void Start()
    {
        alive = true;
        canMove = true;
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
            canMove = true;
        }
        
    }

    public void dontMove(int dontmovetimer)
    {
        dontMoveTimer = dontmovetimer;
        canMove = false;
    }
}
