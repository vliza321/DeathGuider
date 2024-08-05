using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDirectionalSign : MonoBehaviour
{
    private GameObject TreasureBoxEscapeStairDirectionalSignManager;

    [SerializeField]
    private GameObject BoxPrefeb;
    [SerializeField]
    private GameObject StairPrefeb;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temt;
        if(this.gameObject.transform.parent.name == "TreasureBox")
        {
            temt = Instantiate(BoxPrefeb, new Vector3(transform.position.x + 0.0f, transform.position.y + 0.0f, transform.position.z + 0.0f), Quaternion.identity);
            temt.transform.parent = this.transform;
            if(temt.active == false)
            {
                temt.SetActive(true);
            }
        }
        else if(this.transform.parent.name == "Stair")
        {
            temt = Instantiate(StairPrefeb, new Vector3(transform.position.x + 0.0f, transform.position.y + 0.0f, transform.position.z + 0.0f), Quaternion.identity);
            temt.transform.parent = this.transform;
            if (temt.active == false)
            {
                temt.SetActive(true);
            }
        }
        temt = null;
    }
}
