using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CSVManager :MonoBehaviour
{
    public List<string> FILE_NAME;
    
    public List<Dictionary<string, object>> localUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> guiderData = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> prisonerData = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> stageData = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> monsterData = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> bossMonsterData = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> guiderInUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> prisonerInUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> WeaponInUser = new List<Dictionary<string, object>>();

    public List<Dictionary<string, object>> Dialog = new List<Dictionary<string, object>>();
    
    public DialogDataList dialogDataList;
    private void Init()
    {
        localUser = new List<Dictionary<string, object>>();
        guiderData = new List<Dictionary<string, object>>();
        prisonerData = new List<Dictionary<string, object>>();
        stageData = new List<Dictionary<string, object>>();
        monsterData = new List<Dictionary<string, object>>();
        bossMonsterData = new List<Dictionary<string, object>>();
        guiderInUser = new List<Dictionary<string, object>>();
        prisonerInUser = new List<Dictionary<string, object>>();
        WeaponInUser = new List<Dictionary<string, object>>();

        localUser = CSVReader.Read("localUser");
        guiderData = CSVReader.Read("guiderData");
        prisonerData = CSVReader.Read("prisonerData");
        monsterData = CSVReader.Read("monsterData");
        bossMonsterData = CSVReader.Read("bossMonsterData");
        guiderInUser = CSVReader.Read("guiderInUser");
        prisonerInUser = CSVReader.Read("prisonerInUser");
        WeaponInUser = CSVReader.Read("WeaponInUser");
    }

    private void Awake()
    {
        dialogDataList.dialogDatas.Clear();
        FILE_NAME = new List<string> { /*
        "localUser",
        "GuiderData","PrisonerData","StageData",
        "MonsterData","BossMonsterData",
        "GudierInUser","PrisonerInUser", "WeaponInUser", */
        
        "Dialog" };
        foreach(var fn in FILE_NAME)
        {
            //SetFieldByString(fn, GetFieldByString(fn));/*
            object fnValue = GetFieldByString(fn);
            if(fnValue is List<Dictionary<string,object>>fnList)
            {
                fnList = CSVReader.Read(fn);
                SetFieldByString(fn, fnList);
            }
        }
        //dialog 출력
        for (int i = 0; i < Dialog.Count; i++)
        {
            Debug.Log(Dialog[i]["CustomerID"] + Dialog[i]["Content"].ToString());
        }
        ConvertToScriptableObject();
    }

    public object GetFieldByString(string fieldName)
    {
        // Reflection을 사용하여 필드에 접근
        FieldInfo field = this.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);

        if (field != null)
        {
            return field.GetValue(this); // 필드의 값을 반환
        }
        else
        {
            Console.WriteLine($"{fieldName} not found.");
            return null;
        }
    }
    public void SetFieldByString(string fieldName, object value)
    {
        FieldInfo field = this.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        field?.SetValue(this, value);
    }

    public object PrintField(string name)
    {
        // 필드 검색 시 BindingFlags를 사용하여 private 필드도 검색 가능하게 설정
        FieldInfo field = this.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (field != null)
        {
            // 필드를 찾았다면 값을 반환
            var result = field.GetValue(this);
            return result;
        }
        else
        {
            // 필드를 찾지 못하면 null 반환 또는 예외 처리
            return null;
        }
    }

    void ConvertToScriptableObject()
    {
        DialogData newData = new DialogData();
        foreach (Dictionary<string, object> dialog in Dialog)
        {
            /*if(user.ContainsKey(newData.Content.ToString()))*/

            if (dialog.ContainsKey("Number"))
                newData.Number = int.Parse(dialog["Number"].ToString());
            if (dialog.ContainsKey("CUstomerID"))
                newData.CustomerID = int.Parse(dialog["CustomerID"].ToString());
            if (dialog.ContainsKey("State"))
                newData.State = (dialog["State"].ToString());
            if (dialog.ContainsKey("Content"))
                newData.State = (dialog["Content"].ToString());
            if(!dialogDataList.dialogDatas.Exists(d => d.Number == newData.Number))
            {
                dialogDataList.dialogDatas.Add(newData);
            }
            else
            {
                Debug.Log("asdfasdfa");
            }
            SaveDialogDataList(newData);
        }
        newData = null;
    }

    public void SaveDialogDataList(DialogData newData)
    {
        // 기존 ScriptableObject를 불러오기
        DialogDataList existingDataList = dialogDataList;

        // 중복 체크 (예시로 id를 기준으로 중복 확인)
        if (!existingDataList.dialogDatas.Exists(dialog => dialog.Number == newData.Number))
        {
            existingDataList.dialogDatas.Add(newData);
        }
        // 변경 사항을 에디터에서 저장 (런타임에 적용됨)
        SaveChanges(existingDataList);
    }

#if UNITY_EDITOR
    // ScriptableObject 변경 사항을 저장하는 함수
    private void SaveChanges(DialogDataList dataList)
    {
        EditorUtility.SetDirty(dataList);
        AssetDatabase.SaveAssets();
        Debug.Log("ScriptableObject 저장 완료.");
    }
#endif
}
