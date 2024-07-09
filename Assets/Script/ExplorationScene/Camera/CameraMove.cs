using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    // 수정 위치
    public GameObject MainMoveCamera;
    public GameObject Player;
    public GameObject guider;
    void Awake()
    {
        MainMoveCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        guider = Player.GetComponent<PlayerSwap>().Guider;
    }
    // Update is called once per frame
    void Update()
    {
        MainMoveCamera.transform.position = new Vector3((float)guider.transform.position.x, (float)guider.transform.position.y, -10f);
    }

    private void FixedUpdate()
    {
    }
}
