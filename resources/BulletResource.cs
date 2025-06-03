using Godot;

[GlobalClass]
public partial class BulletResource : Resource
{
	[Export(PropertyHint.Range, "0,100")] public float FireRatePercentage { get; private set; }
	[Export] public float MinFireRateDelay { get; private set; } = 0.2f;   // 5 shots/second at max fire rate
	[Export] public float MaxFireRateDelay { get; private set; } = 3.0f;   // 1 shot/3 seconds at min fire rate
	[Export] public float Damage { get; set; }
	[Export] public float Speed { get; set; }

	[Export] public PackedScene Scene { get; private set; }
}
