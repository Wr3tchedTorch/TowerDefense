using Godot;

[GlobalClass]
public partial class BulletResource : Resource
{
	[Export(PropertyHint.Range, "0,100")] public float FireRatePercentage { get; private set; }
	[Export] public float MinFireRateDelay { get; private set; } = 0.35f;  // ~3 shots/second at max fire rate
	[Export] public float MaxFireRateDelay { get; private set; } = 2.5f;   // 1 shot/2.5 seconds at min fire rate
	[Export] public float Damage { get; set; }
	[Export] public float Speed { get; set; }

	[Export] public PackedScene Scene { get; private set; }
}
