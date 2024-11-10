using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class CSVManager :MonoBehaviour
{
    public List<string> FILE_NAME;
    
    public List<Dictionary<string, object>> LocalUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> Guider = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> Prisoner = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> Stage = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> Monster = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> BossMonster = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> GuiderInUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> PrisonerInUser = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> WeaponInUser = new List<Dictionary<string, object>>();

    //CSV파일 파싱 직후 저장 공간
    public List<Dictionary<string, object>> Dialog = new List<Dictionary<string, object>>();

    public List<ScriptableObject> Datas = new List<ScriptableObject>();

    private void Awake()
    {
        FILE_NAME = new List<string> { 
        /*"LocalUser",
        "Guider","Prisoner","Stage",
        "Monster","BossMonster",
        "GudierInUser","PrisonerInUser", "WeaponInUser",
        */
        "Dialog" };

        foreach(var fn in FILE_NAME)
        {
            object fnValue = GetFieldByString(fn);
            if(fnValue is List<Dictionary<string,object>>fnList)
            {
                fnList = CSVReader.Read(fn);
                SetFieldByString(fn, fnList);
                ConvertCSVToScriptableObject(fn, fnList);
            }
        }

        /* 사용시 최초 초기화는 필요없음
        foreach(var fn in FILE_NAME)
        {
            List<Dictionary<string, object>> fnList = new List<Dictionary<string, object>>();
            fnList = CSVReader.Read(fn);
            ConvertCSVToScriptableObject(fn, fnList);
        }*/

        //dialog 출력
        for (int i = 0; i < Dialog.Count; i++)
        {
            Debug.Log(Dialog[i]["Number"].ToString() +" "+Dialog[i]["CustomerID"] + Dialog[i]["Content"].ToString());
        }

        //dialog 타입 반환
        Type type = Type.GetType("DialogDataList");

        //dialog 타입 로드
        DialogDataList ddl = Resources.Load("DialogDataList",type) as DialogDataList;

        //dialog 값 변경
        foreach(var ddds in ddl.DialogDatas)
        {
            ddds.CustomerID = 991;
        }


        //dialog 파일 위치 저장
        string filePath = Path.Combine(Application.persistentDataPath, "Dialogtest.csv");
        Debug.Log(filePath);
        //dialog 파일에 저장
        SaveToCSV("Dialog", ddl, filePath);
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

    void ConvertCSVToScriptableObject(string dataName, List<Dictionary<string, object>> parsedData)
    {
        //불러올 data파일 + "Data" 문자열을 추가하여 타입을 찾음
        Type type = Type.GetType(dataName + "Data");
        if (type == null)
        {
            Debug.LogError($"Type not found: {dataName + "Data"}");
            return;
        }
        //동적으로 생성한 객체의 속성들을 불러옴
        FieldInfo[] fieldes = type.GetFields();

        //불러온 속성들의 이름을 저장할 공간을 생성
        string[] fieldesName = new string[fieldes.Length];
        int counter = 0;

        //불러온 속성들의 이름들을 저장
        foreach (FieldInfo field in fieldes)
        {
            fieldesName[counter] = field.Name;
            counter++;
            
        }

        foreach (Dictionary<string, object> data in parsedData)
        {
            //찾은 타입에 맞게 해당 타입을 동적 생성
            var newData = Activator.CreateInstance(type);// ~~Data 클래스
            if (newData == null)
            {
                Debug.LogError("Failed to create instance.");
                return;
            }

            for (int i = 0; i < fieldesName.Length; i++)
            {
                //속성들 중에 해당 속성 값이 이름과 같다면
                if (data.ContainsKey(fieldesName[i]))
                {
                    // data에서 가져온 값을 속성의 타입에 맞게 변환하여 newData의 해당 속성에 저장
                    fieldes[i].SetValue(newData, Convert.ChangeType(data[fieldesName[i]], fieldes[i].FieldType));
                }
            }
            SaveDataListInScriptableObject(dataName, newData);
        }
    }


    public void SaveDataListInScriptableObject(string dataName, object newData)
    {
        // 동적으로 타입을 가져오기
        Type type = Type.GetType(dataName + "DataList");
        if (type == null)
        {
            return;
        }

        // Resources에서 ScriptableObject 로드
        ScriptableObject scriptableObject = Resources.Load(dataName + "DataList", type) as ScriptableObject;
        if (scriptableObject == null)
        {
            return;
        }
        FieldInfo field = type.GetField(dataName+"Datas");
        if(field == null)
        {
            return;
        }

        // 동적으로 반환 타입 가져오기
        var currentList = field.GetValue(scriptableObject);

        // IList인지 확인
        if (currentList is IList list)
        {
            // 데이터의 Number 속성에 대한 FieldInfo 얻기
            FieldInfo dataNumberFieldInfo = newData.GetType().GetField("Number");
            if (dataNumberFieldInfo == null)
            {
                dataNumberFieldInfo = newData.GetType().GetField("id");
                if(dataNumberFieldInfo == null)
                {
                    return;
                }
            }

            // 중복 체크
            bool isDuplicate = false;
            foreach (var existingData in list)
            {
                // 기존 데이터의 Number 값 가져오기
                FieldInfo existingNumberFieldInfo = existingData.GetType().GetField("Number");
                if(existingNumberFieldInfo == null)
                {
                    existingNumberFieldInfo = existingData.GetType().GetField("id");
                    if (existingNumberFieldInfo == null)
                    {
                        return;
                    }
                }
                if (existingNumberFieldInfo != null)
                {
                    var existingNumberValue = existingNumberFieldInfo.GetValue(existingData);
                    if (existingNumberValue.Equals(dataNumberFieldInfo.GetValue(newData)))
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            }

            if (!isDuplicate)
            {
                // 새로운 데이터 추가
                list.Add(newData);
            }

        }

        // 변경 사항 저장 (에디터에서의 저장)
        SaveChanges(scriptableObject);
    }

    // ScriptableObject 변경 사항을 저장하는 함수
    private void SaveChanges<T>(T dataList) where T : ScriptableObject
    {
        EditorUtility.SetDirty(dataList);
        AssetDatabase.SaveAssets();
    }

    // ScriptableObject를 CSV로 저장
    /*
    public static void SaveToCSV(string fileName, ScriptableObject data, string filePath)
    {
        FieldInfo listField = data.GetType().GetField(fileName);
        if (listField == null) return;

        var dataList = listField.GetValue(data) as IList<object>;
        if (dataList == null || dataList.Count == 0) return;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // CSV 헤더 작성
            var firstItem = dataList[0];
            FieldInfo[] fields = firstItem.GetType().GetFields();

            // 헤더 작성
            List<string> headers = new List<string>();
            foreach (var field in fields)
            {
                headers.Add(field.Name);
            }
            writer.WriteLine(string.Join(",", headers));

            // 각 항목을 CSV로 작성
            foreach (var item in dataList)
            {
                List<string> values = new List<string>();
                foreach (var field in fields)
                {
                    var value = field.GetValue(item)?.ToString() ?? "";
                    values.Add(value);
                }
                writer.WriteLine(string.Join(",", values));
            }
        }
    }*/
    public static void SaveToCSV(string fileName, ScriptableObject data, string filePath)
    {
        // data 내에 listFieldName에 해당하는 필드를 찾음
        FieldInfo listField = data.GetType().GetField(fileName + "Datas");
        if (listField == null) return;

        // 필드를 통해 데이터 리스트를 가져옴
        var dataList = listField.GetValue(data) as List<DialogData>;
        if (dataList == null || dataList.Count == 0) return;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // CSV 헤더 작성
            FieldInfo[] fields = typeof(DialogData).GetFields();
            List<string> headers = new List<string>();
            foreach (var field in fields)
            {
                headers.Add(field.Name);
            }
            writer.WriteLine(string.Join(",", headers));

            // 각 항목을 CSV로 작성
            foreach (var item in dataList)
            {
                List<string> values = new List<string>();
                foreach (var field in fields)
                {
                    var value = field.GetValue(item)?.ToString() ?? "";
                    values.Add(value);
                }
                writer.WriteLine(string.Join(",", values));
            }
        }
    }
}
