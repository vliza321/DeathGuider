
using System.Collections.Generic;
using UnityEngine;

public class CommandManager 
{
    private Dictionary<KeyCode, InterfaceWeaponCommand> commandDic;

    public void Init()
    {
        commandDic = new Dictionary<KeyCode, InterfaceWeaponCommand>();
        
    }

    public void SetCommand(KeyCode keyCode, InterfaceWeaponCommand command)
    {

        if (commandDic.ContainsValue(command))
        {
            commandDic[keyCode] = command;
            Debug.Log("이미 커맨드가 리스트 포함되어있음.");
            return;
        }
        commandDic.Add(keyCode, command);
    }

    public bool CheckKeyInDic(KeyCode keyCode)
    {
        foreach (KeyValuePair<KeyCode, InterfaceWeaponCommand> keyPair in commandDic)
        {
            if (keyCode == keyPair.Key)
            {
                return true;
            }
        }
        return false;
    }
    public void AssignKey(KeyCode key, KeyCode currentKey)
    {
        if (CheckKeyInDic(key))
        {
            Debug.Log("이미 바인딩 된 키 입니다");
            return;
        }
        if (CheckKey(key,currentKey))
        {
            Debug.Log("키 교환 완료");
            commandDic.Add(key, commandDic[currentKey]);
            commandDic.Remove(currentKey);
        }
    }

    public bool CheckKey(KeyCode key, KeyCode currentKey)
    {
        if (currentKey == key)
        {
            return true;
        }
        return !CheckKeyInDic(key);
        if (
            key >= KeyCode.A && key <= KeyCode.Z || //97 ~ 122   A~Z
            key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9 || //48 ~ 57    알파 0~9
            key == KeyCode.Quote || //39         
            key == KeyCode.Comma || //44
            key == KeyCode.Period || //46
            key == KeyCode.Slash || //47
            key == KeyCode.Semicolon || //59
            key == KeyCode.LeftBracket || //91
            key == KeyCode.RightBracket || //93
            key == KeyCode.Minus || //45
            key == KeyCode.Equals || //61
            key == KeyCode.BackQuote //96
        ) { }
        else return false; 
        if( 
            key == KeyCode.W || key == KeyCode.A ||
            key == KeyCode.S || key == KeyCode.D 
            ){ return false; }

        return true;
    }


    public void InvokeExecute(KeyCode keyCode)
    {
        commandDic[keyCode].Execute();
    }
}