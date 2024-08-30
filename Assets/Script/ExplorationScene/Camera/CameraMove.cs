using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    // 수정 위치
    private GameObject MainMoveCamera;
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
        MainMoveCamera.transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    public void MoveCamera(Vector3 velocity)
    {
        //MainMoveCamera.transform.position = new Vector3((float)guider.transform.position.x, (float)guider.transform.position.y, -10f);
        MainMoveCamera.transform.Translate(velocity.x, velocity.y, 0);
    }
}
