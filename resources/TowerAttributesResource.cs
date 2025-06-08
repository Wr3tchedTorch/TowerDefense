using Godot;

[GlobalClass]
[Tool]
public partial class TowerAttributesResource : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public string Description { get; private set; }

    [Export] public float Damage { get; private set; }
    [Export] public float BulletSpeed { get; private set; }
    [Export] public float Radius { get; private set; }
    [Export(PropertyHint.Range, "0,100,")] public float FireRate { get; private set; }
    [Export] public float MinFireRateDelay { get; private set; } = 0.2f;   // 5 shots/second at max fire rate
    [Export] public float MaxFireRateDelay { get; private set; } = 3.0f;   // 1 shot/3 seconds at min fire rate

    [Export]
    public Godot.Collections.Array<string> TurretTierScenes { get; private set; }

    [Export] public int ClicksRequiredForBuilding { get; private set; } = 50;
    [Export(PropertyHint.File, ".tscn")] public string BulletScenePath { get; private set; }
    
}