using UnityEngine;
public class Weapon
{
    protected int damage;
    protected int coolTime;
    private GameObject thisWeapon;
    protected Animator animator;
    
    public virtual GameObject ThisWeapon
    {
        get { return thisWeapon; }
    }
    public Weapon(int damage, int coolTime, Animator anim)
    {
        this.damage = damage;
        this.coolTime = coolTime;
        this.animator = anim;
    }

    public Weapon(int damage, int coolTime)
    {
        this.damage = damage;
        this.coolTime = coolTime;
    }

    ~Weapon()
    {

    }
}

/*
====================================== 근접형 무기 타입 ============================
*/
public class CloseType : Weapon
{
    protected BoxCollider2D collider;
    public CloseType(int a, int b, Animator anim, BoxCollider2D collider) : base(a,b,anim)
    {
        this.collider = collider;
    }

    public virtual void Stab()
    {
        Debug.Log("closeType 이 하위 클래스에서 정의되지 않은것으로 보입니다");
        animator.SetBool("Stab",true);
        collider.enabled = true;
    }
    public virtual void Swing()
    {
        Debug.Log("closeType이 하위 클래스에서 정의되지 않은것으로 보입니다");
        animator.SetBool("Swing",true);
        collider.enabled = true;
    }
}

public class Sword : CloseType
{
    public Sword(int a, int b,Animator anim,BoxCollider2D collider): base(a,b,anim, collider)
    {

    }
    public override void Stab()
    {
        Debug.Log("적에게 도약하여 칼로 베는 동작 시행");
        animator.SetBool("isMove", true);
        collider.enabled = true;
    }
    public override void Swing()
    {
        Debug.Log("칼로 베는 동작 실행");
        animator.SetBool("isMove", true);
        collider.enabled = true;
    }
}

/*
====================================== 발사형 무기 타입 ==============================
*/
public class LaunchType  : Weapon
{
    protected GameObject bullet;
    public LaunchType(int a, int b,Animator anim, GameObject bullet) : base(a, b, anim)
    {
        this.bullet = bullet;
    }
    public virtual void Shoot()
    {
        Debug.Log("Shoot이 하위 클래스에서 정의되지 않은것으로 보입니다");
        bullet.SetActive(true);
    }

    public virtual void Reload()
    {
        Debug.Log("Reload가 하위 클래스에서 정의되지 않은 것으로 보입니다");
    }
}

public class M4A1 : LaunchType
{
    public M4A1(int a, int b, Animator anim, GameObject bullet) : base(a, b, anim, bullet)
    {

    }
    public override void Shoot()
    {
        Debug.Log("M4A1 발사 쀼쀼");
        bullet.SetActive(true);
    }

    public override void Reload()
    {
        Debug.Log("M4A1 재장전");
    }
}

/*
====================================== 위성형 무기 타입 ============================
*/
public class SatelliteType : Weapon
{
    public SatelliteType(int a, int b) : base(a, b)
    {

    }
    public virtual void Spin()
    {
        Debug.Log("satelliteType이 가 하위 클래스에서 정의되지 않은 것으로 보입니다");
    }
}

public class SpinWeapon : SatelliteType
{
    public SpinWeapon(int a, int b) : base(a,b)
    {

    }
    public override void Spin()
    {
        Debug.Log("칼이 주변을 계속 도는 동작 실행");
    }
}