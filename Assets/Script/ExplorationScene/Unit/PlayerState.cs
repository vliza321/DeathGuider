using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private UnitData stat;
    private bool alive; // 살았는지 죽었는지만
    private bool canMove; // 움직이는지 멈췄는지만
    private int dontMoveTimer;
    // Start is called before the first frame update
    private int experiencePoints = 0;
    private int level = 1;
    private int experienceToNextLevel = 100;

    private GameObject weapon;

    public GameObject Weapon
    {
        get { return Weapon; }
        set { weapon = value; }
    }

    public int ExperiencePoints // 현재 경험치 읽기
    {
        get { return experiencePoints; }
        private set
        {
            experiencePoints = value;
            CheckLevelUp();
        }
    }

    public int Level //레벨 읽기
    {
        get { return level; }
        private set { level = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public bool Alive
    {
        get { return alive; }
        set { alive = value; }
    }

    private void Awake()
    {
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        weapon = this.transform.GetChild(3).gameObject;
    }
    void Start()
    {
        alive = true;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dontMoveTimer > 0)
        {
            dontMoveTimer--;
        }
        if (dontMoveTimer <= 0)
        {
            canMove = true;
        }
        
    }

    public void dontMove(int dontmovetimer)
    {
        dontMoveTimer = dontmovetimer;
        canMove = false;
    }

    public void AddExperience(int amount) // 경험치를 추가하고 레벨업 여부를 체크
    {
        ExperiencePoints += amount; 
    }

    private void CheckLevelUp() //레벨업 확인 후 처리
    {
        while (experiencePoints >= experienceToNextLevel)
        {
            experiencePoints -= experienceToNextLevel;
            LevelUp();
        }
    }
    private void LevelUp() 
    {
        level++;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f); // 다음 레벨업에 필요한 경험치 증가
        // 추후 레벨업 시 추가 기능(능력치 증가 등) 구현자리
        Debug.Log("Level Up! New Level: " + level);
    }

    private void SwapUnit()
    {

    }
}

