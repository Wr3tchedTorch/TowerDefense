using Godot;

namespace Game.Manager;

public partial class TurretPlacementManager : Node
{
    [Export] public GridManager gridManager;

    private Node2D ghostTurret;

    private bool isBuilding = false;

    public override void _Process(double delta)
    {
        if (!isBuilding)
        {
            return;
        }

        // Move turret ghost to cursor here
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!isBuilding)
        {
            return;
        }

        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Left)
            {
                // Instantiate turret here
            }
        }
    }

    private void OnTurretBought(TurretAttributesResource turretAttributesResource)
    {
        var scene = GD.Load<PackedScene>(turretAttributesResource.TurretTierScenes[0]);
        ghostTurret = scene.Instantiate<Node2D>();


        isBuilding = true;
    }
}
