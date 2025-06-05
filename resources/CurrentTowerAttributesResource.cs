
using Game.Enums;
using Godot;

public partial class CurrentTowerAttributesResource : Resource
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

    [Export] public TowerTargetMode TowerTargetMode { get; set; } = TowerTargetMode.First;
    [Export] public int Tier { get; private set; } = 0;

    private int _radiusUpgradePercentage = 0;

    public override string ToString()
    {
        return $"Tier: {Tier}, Mode: {TowerTargetMode}, " +
               $"Dmg%: {DamageUpgradePercentage}%, FR%: {FireRateUpgradePercentage}%, " +
               $"Spd%: {BulletSpeedUpgradePercentage}%, Rad%: {RadiusUpgradePercentage}%";
    }
}