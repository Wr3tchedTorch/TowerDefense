using Godot;

[GlobalClass]
public partial class UpgradePathResource : Resource
{
    [Export] public Godot.Collections.Array<UpgradeResource> TopPath { get; private set; }
    [Export] public Godot.Collections.Array<UpgradeResource> MiddlePath { get; private set; }
    [Export] public Godot.Collections.Array<UpgradeResource> BottomPath { get; private set; }
}