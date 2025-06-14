using Game.Autoload;
using Godot;

namespace Game.Manager;

public partial class TurretPlacementManager : Node
{
    [Export] public GridManager gridManager;

    private Node2D ghostTurret;

    private bool isBuilding = false;
    private readonly StringName LeftMbClick = "left_mb_click";
    private string turretManagerScenePath = "";

    public override void _Ready()
    {
        GameEvents.Instance.TurretBought += OnTurretBought;
    }

    public override void _Process(double delta)
    {
        if (!isBuilding)
        {
            return;
        }
        ghostTurret.GlobalPosition = ghostTurret.GetGlobalMousePosition();

        if (Input.IsActionPressed(LeftMbClick))
        {
            ghostTurret.QueueFree();

            var scene = GD.Load<PackedScene>(turretManagerScenePath);
            var turretManager = scene.Instantiate<Node2D>();
            turretManager.GlobalPosition = ghostTurret.GlobalPosition;
            AddToTurretsGroup(turretManager);
            isBuilding = false;
        }
    }

    private void OnTurretBought(TurretAttributesResource turretAttributesResource)
    {
        GD.Print($"Turret bought: {turretAttributesResource.Name}");

        var scene = GD.Load<PackedScene>(turretAttributesResource.TurretTierScenes[0]);
        ghostTurret = scene.Instantiate<Node2D>();
        ghostTurret.Modulate = new Color("#ffffffaf");
        /*
         TODO: Radius indication texture Instantiation
            var scene = GD.Load<PackedScene>("path-to-the-radius-texture");
            radiusIndicator = scene.Instantiate<Texture2D>();            
            ghostTurret.AddChild(radiusIndicator);
        */
        AddToTurretsGroup(ghostTurret);

        turretManagerScenePath = turretAttributesResource.TurretManagerScenePath;
        isBuilding = true;
    }

    private void AddToTurretsGroup(Node node)
    {
        GetTree().GetFirstNodeInGroup("Turrets").AddChild(node);
    }
}
