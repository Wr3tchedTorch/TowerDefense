using Godot;

namespace Game.Component;

public partial class TurretAttributesComponent : Node
{
    public TurretAttributesResource        TurretAttributesResource;
    public CurrentTurretAttributesResource CurrentTurretAttributesResource;

    public void Initialize(TurretAttributesResource turretAttributesResource, Callable OnAttributesChanged)
    {
        CurrentTurretAttributesResource = new CurrentTurretAttributesResource();
        TurretAttributesResource = turretAttributesResource;

        CurrentTurretAttributesResource.Connect(
            CurrentTurretAttributesResource.SignalName.AttributesChanged,
            OnAttributesChanged
        );
    }

    public float GetDamage()
    {
        return TurretAttributesResource.Damage *
                (1 + CurrentTurretAttributesResource.DamageUpgradePercentage / 100f)
                * (1 + (int)CurrentTurretAttributesResource.Tier * 0.5f);
    }

    public float GetFireRate()
    {
        return Mathf.Clamp(
            60f / (
                TurretAttributesResource.FireRate
                * (1 + CurrentTurretAttributesResource.FireRateUpgradePercentage / 100f)
                * (1 + (int)CurrentTurretAttributesResource.Tier * 0.5f)
            ),
            TurretAttributesResource.MinFireRateDelay,
            TurretAttributesResource.MaxFireRateDelay
        );
    }

    public float GetBulletSpeed()
    {
        return TurretAttributesResource.BulletSpeed *
                (1 + CurrentTurretAttributesResource.BulletSpeedUpgradePercentage / 100f)
                * (1 + (int)CurrentTurretAttributesResource.Tier * 0.5f);
    }

    public float GetRadius()
    {
        return TurretAttributesResource.Radius *
                (1 + CurrentTurretAttributesResource.RadiusUpgradePercentage / 100f)
                * (1 + (int)CurrentTurretAttributesResource.Tier * 0.5f);
    }
}
