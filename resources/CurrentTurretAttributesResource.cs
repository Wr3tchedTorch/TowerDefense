using Game.Enums;
using Godot;
using TurretDefense.enums;

public partial class CurrentTurretAttributesResource : Resource
{
    [Signal] public delegate void AttributesChangedEventHandler();
    
    [Export] public int DamageUpgradePercentage { get; set; } = 0;
    [Export] public int FireRateUpgradePercentage { get; set; } = 0;
    [Export] public int BulletSpeedUpgradePercentage { get; set; } = 0;
    [Export]
    public int RadiusUpgradePercentage
    {
        get => _radiusUpgradePercentage;
        set 
        {
            _radiusUpgradePercentage = value;
            EmitSignal(SignalName.AttributesChanged);
        }
    }    

    [Export] public TurretTargetMode TurretTargetMode { get; set; } = TurretTargetMode.First;
    [Export]
    public TurretTier Tier
    {
        get => _tier;
        set {
            _tier = value;
            EmitSignal(SignalName.AttributesChanged);
        }
    }

    private TurretTier _tier = 0;
    private int _radiusUpgradePercentage = 0;

    public override string ToString()
    {
        return $"Tier: {Tier}, Mode: {TurretTargetMode}, " +
               $"Dmg%: {DamageUpgradePercentage}%, FR%: {FireRateUpgradePercentage}%, " +
               $"Spd%: {BulletSpeedUpgradePercentage}%, Rad%: {RadiusUpgradePercentage}%";
    }
}