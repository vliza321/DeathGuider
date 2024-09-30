using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjectManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> dontDestroyObject;
    private bool inManager;
    public List<GameObject> DontDestroyObject
    {
        get { return dontDestroyObject; }
    }

    public void AddDontDestroyObject(GameObject GO)
    {
        dontDestroyObject.Add(GO);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        inManager = false;
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach(GameObject ddo in DDO)
        {
            if(ddo.transform.name == this.gameObject.transform.name)
            {
                inManager = true;
                break;
            }
        }
        if(!inManager)
        {
            foreach (GameObject ddo in DDO)
            {
                AddDontDestroyObject(ddo);
            }
        }
        DDO = null;
    }
}
