using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private float timer;
    private bool playerEscape;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        playerEscape = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerEscape) timer += Time.deltaTime;
    }

    public void PlayerEscape()
    {
        playerEscape = true;
       
        DontDestroyOnLoad(this.gameObject);
       
    }
}
