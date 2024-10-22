using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string asdf = "kjbkjb";
    private void Awake()
    {
        FILE_NAME = new List<string> { /*
        "localUser",
        "GuiderData","PrisonerData","StageData",
        "MonsterData","BossMonsterData",
        "GudierInUser","PrisonerInUser", "WeaponInUser", */
        
        "Dialog" };
        foreach(var fn in FILE_NAME)
        {
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

}
