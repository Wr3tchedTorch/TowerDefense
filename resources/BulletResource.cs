using Godot;

[GlobalClass]
public partial class BulletResource : Resource
{
	[Export(PropertyHint.Range, "0,100")] public float FireRatePercentage { get; private set; }
	[Export] public float Damage { get; set; }
	[Export] public float Speed { get; set; }

	[Export] public PackedScene Scene { get; private set; }
}
