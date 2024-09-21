using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BTNType
{
    Start, 
    Quit
        //Continue > Type 1¹ø
}


public class MainUI : MonoBehaviour
{
    public void PlayBtn()
    {
        SceneManager.LoadScene("Main");
    }
}
