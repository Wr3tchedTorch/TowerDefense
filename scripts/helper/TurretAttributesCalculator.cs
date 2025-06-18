using Godot;

namespace Game.scripts.helper;

public partial class TurretAttributesCalculator(
    TurretAttributesResource TurretAttributesResource,
    CurrentTurretAttributesResource CurrentTurretAttributesResource
    )
{
    public TurretAttributesResource TurretAttributesResource = TurretAttributesResource;
    public CurrentTurretAttributesResource CurrentTurretAttributesResource = CurrentTurretAttributesResource;

    public float GetDamage()
    {
        return TurretAttributesResource.Damage *
                (1 + CurrentTurretAttributesResource.DamageUpgradePercentage / 100f);
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
                (1 + CurrentTurretAttributesResource.BulletSpeedUpgradePercentage / 100f);
    }

    public float GetRadius()
    {
        return TurretAttributesResource.Radius *
                (1 + CurrentTurretAttributesResource.RadiusUpgradePercentage / 100f);
    }
}
