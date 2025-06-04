using Godot;

[GlobalClass]
[Tool]
public partial class TowerAttributesResource : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public int Damage { get; private set; }
    [Export] public int FireRate { get; private set; }
    [Export] public int BulletSpeed { get; private set; }
    [Export] public int Radius { get; private set; }
    [Export] public int CurrentTier { get; private set; }
    [Export] public Texture[] TierSprites { get; private set; }
}