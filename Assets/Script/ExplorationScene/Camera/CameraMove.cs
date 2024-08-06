using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    // 수정 위치
    private GameObject MainMoveCamera;
    private GameObject Player;
    [SerializeField]
    private GameObject guider;
    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; } 
    }
    void Awake()
    {
        MainMoveCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        MainMoveCamera.transform.position = new Vector3((float)guider.transform.position.x, (float)guider.transform.position.y, -10f);
    }
}
