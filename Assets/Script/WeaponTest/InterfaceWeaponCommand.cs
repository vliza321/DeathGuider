public interface InterfaceWeaponCommand
{
    void Execute();
}

public class ReloadGunCommand : InterfaceWeaponCommand
{
    private Gun gun;
    public void Execute()
    {
        gun.Reload();
    }

    public ReloadGunCommand(Gun val)
    {
        gun = val;
    }
}

public class ShootGunCommand : InterfaceWeaponCommand
{
    private Gun gun;
    public void Execute()
    {
        gun.Shoot();
    }

    public ShootGunCommand(Gun val)
    {
        gun = val;
    }
}

public class StabKnifeCommand: InterfaceWeaponCommand
{
    private Knife knife;

    public void Execute()
    {
        knife.Stab();
    }

    public StabKnifeCommand(Knife val)
    {
        knife = val;
    }
}

public class SwingSwordCommand : InterfaceWeaponCommand
{
    private Sword sword;

    public void Execute()
    {
        sword.Swing();
    }

    public SwingSwordCommand(Sword val)
    {
        sword = val;
    }
}