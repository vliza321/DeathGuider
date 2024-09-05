public interface InterfaceWeaponCommand
{
    void Execute();
}

/*
====================================== 근접형 무기 타입 ============================
*/

public class StabCloseTypeCommand : InterfaceWeaponCommand
{
    private Sword knife;

    public void Execute()
    {
        knife.Stab();
    }

    public StabCloseTypeCommand(Sword val)
    {
        knife = val;
    }
}

public class SwingCloseTypeCommand : InterfaceWeaponCommand
{
    private Sword sword;

    public void Execute()
    {
        sword.Swing();
    }

    public SwingCloseTypeCommand(Sword val)
    {
        sword = val;
    }
}
/*
====================================== 발사형 무기 타입 ==============================
*/
public class ReloadLaunchTypeCommand : InterfaceWeaponCommand
{
    private LaunchType gun;
    public void Execute()
    {
        gun.Reload();
    }

    public ReloadLaunchTypeCommand(LaunchType val)
    {
        gun = val;
    }
}

public class ShootLaunchTypeCommand : InterfaceWeaponCommand
{
    private LaunchType gun;
    public void Execute()
    {
        gun.Shoot();
    }

    public ShootLaunchTypeCommand(LaunchType val)
    {
        gun = val;
    }
}
/*
====================================== 위성형 무기 타입 ============================
*/

public class SpinSwordSatelliteTypeCommand : InterfaceWeaponCommand
{
    private SatelliteType spinWeapon;
    
    public void Execute()
    {
        spinWeapon.Spin();
    }

    public SpinSwordSatelliteTypeCommand(SatelliteType val)
    {
        spinWeapon = val;
    }
}