using Game.Enums;
using Godot;

[GlobalClass]
[Tool]
public partial class TowerAttributesResource : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public string Description { get; private set; }

    [Export] public int Damage { get; private set; }
    [Export] public int FireRate { get; private set; }
    [Export] public int BulletSpeed { get; private set; }
    [Export] public int Radius { get; private set; }

    [Export] public int CurrentDamageUpgradePercentage { get; set; }
    [Export] public int CurrentFireRateUpgradePercentage { get; set; }
    [Export] public int CurrentBulletSpeedUpgradePercentage { get; set; }
    [Export] public int CurrentRadiusUpgradePercentage { get; set; }

    [Export] public TowerTargetMode TowerTargetMode { get; set; }

    [Export] public int CurrentTier { get; private set; }

    [Export] public Texture[] TierSprites { get; private set; }

    public override string ToString()
    {
        return $"Name: {Name}, Desc: {Description}, Tier: {CurrentTier}, Mode: {TowerTargetMode}, " +
               $"Dmg%: {CurrentDamageUpgradePercentage}%, FR%: {CurrentFireRateUpgradePercentage}%, " +
               $"Spd%: {CurrentBulletSpeedUpgradePercentage}%, Rad%: {CurrentRadiusUpgradePercentage}%";
    }
}