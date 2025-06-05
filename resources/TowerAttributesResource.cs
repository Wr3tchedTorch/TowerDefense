using Game.Enums;
using Godot;

[GlobalClass]
[Tool]
public partial class TowerAttributesResource : Resource
{    
    [Export] public string Name { get; private set; }
    [Export] public string Description { get; private set; }

    [Export] public float Damage { get; private set; }
    [Export(PropertyHint.Range, "0,100,")] public float FireRate { get; private set; }
    [Export] public float BulletSpeed { get; private set; }
    [Export] public float Radius { get; private set; }
    [Export] public int ClicksRequiredForBuilding { get; private set; } = 50;
}