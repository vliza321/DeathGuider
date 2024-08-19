using UnityEngine;
public class Weapon
{
    
    protected int damage;
    protected int coolTime;
    private GameObject thisWeapon;
    public virtual GameObject ThisWeapon
    {
        get { return thisWeapon; }
    }
    public Weapon()
    {

    }
    ~Weapon()
    {

    }
}

public class Knife : Weapon
{
    public virtual void Stab()
    {
        Debug.Log("칼로 찌르는 동장 실행");
    }
}

public class Sword : Knife
{
    public virtual void Swing()
    {
        Debug.Log("칼로 베는 동작 실행");
    }
}

public class Gun  : Weapon
{
    protected int maxBulletCount;
    protected int curBulletCount;
    protected int reloadCoolTime;
    protected float maxReboundRange;

    public virtual void Shoot()
    {
        Debug.Log("Shoot이 하위 클래스에서 정의되지 않은것으로 보입니다");
    }

    public virtual void Reload()
    {
        Debug.Log("Reload가 하위 클래스에서 정의되지 않은 것으로 보입니다");
    }
}

public class M4A1 : Gun
{
    public override void Shoot()
    {
        Debug.Log("M4A1 발사 쀼쀼");
    }

    public override void Reload()
    {
        Debug.Log("M4A1 재장전");
    }
}