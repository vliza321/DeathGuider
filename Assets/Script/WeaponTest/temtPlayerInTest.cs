using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class temtPlayerInTest : MonoBehaviour
{
    CommandManager commandManager = null;
    private void Start()
    {
        commandManager = new CommandManager();
        commandManager.Init();

        M4A1 m4a1 = new M4A1();
        Sword sword = new Sword();

        ShootGunCommand shootM4a1Command = new ShootGunCommand(m4a1);
        ReloadGunCommand reloadM4a1Command = new ReloadGunCommand(m4a1);
        StabKnifeCommand stabKnifeCommand = new StabKnifeCommand(sword);

        commandManager.SetCommand(KeyCode.Mouse1, shootM4a1Command);
        commandManager.SetCommand(KeyCode.R, reloadM4a1Command);
        commandManager.SetCommand(KeyCode.F, stabKnifeCommand);
    }

    private void Update()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            // 키가 눌렸을 때 해당 키를 출력
            if (Input.GetKeyDown(keyCode))
            {
                if(commandManager.CheckKeyInDic(keyCode))
                {
                    commandManager.InvokeExecute(keyCode);
                }
            }
        }
    }

    public void OnButtonClick()
    {
        StartCoroutine(CorAssignKey(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>()));
    }

    private IEnumerator CorAssignKey(TextMeshProUGUI sCurrentKey)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode) && !commandManager.CheckKeyInDic(kcode))
                    {
                        commandManager.AssignKey(kcode, (KeyCode)(sCurrentKey.text[0] + 32));
                        sCurrentKey.text = (kcode).ToString();
                    }
                }
                yield break;
            }

            yield return null;
        }
    }
}
