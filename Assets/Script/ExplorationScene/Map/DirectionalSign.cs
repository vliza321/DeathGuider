using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSign : MonoBehaviour
{
    private TreasureBoxEscapeStairManager TreasureBoxEscapeStairManager;
    private Transform player;

    private void Awake()
    {
        TreasureBoxEscapeStairManager = this.transform.parent.parent.gameObject.GetComponent<TreasureBoxEscapeStairManager>();
        player = TreasureBoxEscapeStairManager.player.transform;


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
