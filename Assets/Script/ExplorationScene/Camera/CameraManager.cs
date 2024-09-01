using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 수정 위치
    private GameObject mainMoveCamera;
    private GameObject guider;
    private Vector3 currentPos;
    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; } 
    }

    public GameObject MainMoveCamera
    {
        get { return mainMoveCamera; }
    }
    void Awake()
    {
        mainMoveCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainMoveCamera.transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    private void FixedUpdate()
    { 
        currentPos.x = guider.transform.position.x;
        currentPos.y = guider.transform.position.y;
        currentPos.z = mainMoveCamera.transform.position.z;
        mainMoveCamera.transform.position = currentPos;
    }
}
