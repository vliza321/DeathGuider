using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private int selectUserID = 0;   //선택한 유저의 ID
    [SerializeField]
    private int selectStageID = 0;  //선택한 전투 스테이지의 ID

    public int SelectUserID
    {
        get { return selectUserID; }
        set { selectUserID = value; }
    }
    public int SelectStageID
    {
        get { return selectStageID; }
        set { selectStageID = value; }
    }


    [SerializeField]
    private List<GameObject> monster;           //몬스터 프리팹
    [SerializeField]
    private List<GameObject> prototypeUnit;     //안내자 프리팹
    [SerializeField]
    private List<GameObject> prototypeWeapon;   //무기 프리팹
    [SerializeField]
    private List<Sprite> guiderHeadImg;         //안내자 머리 이미지
    [SerializeField]
    private List<Sprite> guiderBodyImg;         //안내자 몸통 이미지
    [SerializeField]
    private List<Sprite> prisonerHeadImg;       //수감자 머리 이미지
    [SerializeField]
    private List<Sprite> prisonerBodyImg;       //수감자 몸통 이미지
    [SerializeField]
    private List<RuntimeAnimatorController> prisonerHeadAnim;   //수감자 머리 애니메이션
    [SerializeField]
    private List<RuntimeAnimatorController> prisonerBodyAnim;   //수감자 몸통 애니메이션
    [SerializeField]
    private List<Sprite> weaponImg;             //무기 이미지
    [SerializeField]
    private List<Sprite> baseTileImg;         //전투 스테이지 바닥 타일 이미지
    [SerializeField]
    private List<GameObject> terrain;

    public List<GameObject> Terrain
    {
        get { return terrain; }
    }

    public List<GameObject> Monster
    {
        get { return monster; }
    }

    public List<GameObject> PrototypeUnit 
    {
        get { return prototypeUnit; }
    }

    public List<GameObject> PrototypeWeapon
    {
        get { return prototypeWeapon; }
    }

    public List<Sprite> GuiderHeadImg
    { 
        get { return guiderHeadImg; }
    }
    public List<Sprite> GuiderBodyImg 
    {
        get { return guiderBodyImg; } 
    }
    public List<Sprite> PrisonerHeadImg 
    { 
        get { return prisonerHeadImg; } 
    }
    public List<RuntimeAnimatorController> PrisonerHeadAnim 
    {
        get { return prisonerHeadAnim; }
    }
    public List<RuntimeAnimatorController> PrisonerBodyAnim
    {
        get { return prisonerBodyAnim; }
    }
    
    public List<Sprite> WeaponImg 
    {
        get { return weaponImg; }
    }
    public List<Sprite> BaseTileImg
    {
        get { return baseTileImg; }
    }

    public List<Sprite> PrisonerBodyImg
    {
        get { return prisonerBodyImg; }
    }
     

    private List<string> path = new List<string> { 
        "monster",
        "prototypeUnit",
        "prototypeWeapon",
        "guiderHeadImg",
        "guiderBodyImg",
        "prisonerBodyImg",
        "prisonerBodyAnim",
        "prisonerHeadImg",
        "prisonerHeadAnim",
        "baseTileImg", 
        "weaponImg",
        "terrain",
    };

    public void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }

        FieldInfo fieldInfo;
        object[] objects;
        Type type;
        Type elementType;
        foreach (var p in path)
        {
            fieldInfo = GetType().GetField(p, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfo == null)
            {
                continue;
            }

            type = fieldInfo.FieldType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // 제네릭 타입 파라미터(원소 타입) 반환
                elementType = type.GetGenericArguments()[0];

                // Resources에서 요소 로드
                objects = Resources.LoadAll(p, elementType);
                if (objects == null || objects.Length == 0)
                {
                    continue;
                }

                var listInstance = fieldInfo.GetValue(this);
                if (listInstance == null)
                {
                    // 필드가 null이라면 새 리스트 생성
                    listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    fieldInfo.SetValue(this, listInstance);
                }

                // 리스트에 요소 추가
                var addMethod = listInstance.GetType().GetMethod("Add");
                foreach (var obj in objects)
                {
                    addMethod.Invoke(listInstance, new[] { obj });
                }
            }
        }
        objects = null;

        sorting(monster);
        sorting(prototypeUnit);
        sorting(prototypeWeapon);
        sorting(prisonerBodyImg);
        sorting(prisonerBodyAnim);
        sorting(prisonerHeadImg);
        sorting(prisonerHeadAnim);
        sorting(baseTileImg);
        sorting(weaponImg);
        sorting(terrain);
    }


    private object GetFieldByString(string fieldName)
    {
        // Reflection을 사용하여 필드에 접근
        FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
        {
            return field.GetValue(this); // 필드의 값을 반환
        }
        else
        {
            Console.WriteLine($"{fieldName} not found.");
            Debug.Log($"{fieldName} not found.");
            return null;
        }
    }

    private void sorting<T>(List<T> list) where T : UnityEngine.Object
    {
        list.Sort((a, b) =>
        {
            int numA = int.Parse(a.name);
            int numB = int.Parse(b.name);
            return numA.CompareTo(numB);
        });

    }
}
