using Godot;
using System;

public partial class TurretsGroup : Node
{
    public override void _Ready()
    {
        AddToGroup("Turrets");
    }
}
