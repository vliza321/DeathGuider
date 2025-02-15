using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneResultScene : MonoBehaviour
{
    // "Title" 씬으로 이동하는 버튼 기능
    public void OnClickTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // "Main" 씬으로 이동하는 버튼 기능
    public void OnClickMain()
    {
        SceneManager.LoadScene("LobbyTest");
    }
}
