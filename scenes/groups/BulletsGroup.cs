using Godot;

public partial class BulletsGroup : Node
{
    public override void _Ready()
    {
        AddToGroup("Bullets");
    }
}
