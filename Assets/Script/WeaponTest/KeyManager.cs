using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

[System.Serializable]
public class KeyData
{
    public string keyName;

    public KeyCode keyCode;

    public KeyData(string keyName, KeyCode keyCode)
    {
        this.keyName = keyName;
        this.keyCode = keyCode;
    }
}

public class KeyManager : MonoBehaviour
{

    private Dictionary<string, KeyCode> mKeyDictionary;

    private void Awake()
    {
        mKeyDictionary = new Dictionary<string, KeyCode>();
        ResetOptionData();
    }

    private void ResetOptionData()
    {
        mKeyDictionary.Clear();

        mKeyDictionary.Add("shootM4a1Command", KeyCode.Mouse0);
        mKeyDictionary.Add("reloadM4a1Command", KeyCode.R);
        mKeyDictionary.Add("stabKnifeCommand", KeyCode.F);
    }

    public KeyCode GetKeyCode(string keyName)
    {
        return mKeyDictionary[keyName];
    }
    
    public void AssingKey(KeyCode keyCode, string keyName)
    {
        mKeyDictionary[keyName] = keyCode;
    }
}
