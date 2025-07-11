using Game.Autoload;
using Game.Turret;
using Godot;

namespace Game.Manager;

public partial class TurretPlacementManager : Node
{
    public Node2D TurretsGroup { get; set; } = null;
    public Node2D BulletsGroup { get; set; } = null;

    private readonly StringName LeftMbClick = "left_mb_click";

    private string turretManagerScenePath = "";
    private Node2D ghostTurret;

    private bool isBuilding = false;

    public override void _Ready()
    {
        GameEvents.Instance.TurretBought += OnTurretBought;
    }

    public override void _Input(InputEvent @event)
    {
        if (!isBuilding)
        {
            return;
        }

        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Left)
            {
                isBuilding = false;

                var turretManager = (TurretManager) CreateTurret(turretManagerScenePath);
                turretManager.GlobalPosition = ghostTurret.GlobalPosition;
                GD.Print($"BulletsGroup: {BulletsGroup}");
                turretManager.BulletsGroup = BulletsGroup;

                TurretsGroup.AddChild(turretManager);
                ghostTurret.QueueFree();
            }
        }
    }

    public override void _Process(double delta)
    {
        if (!isBuilding || ghostTurret == null)
        {
            return;
        }
        ghostTurret.GlobalPosition = ghostTurret.GetGlobalMousePosition();
    }

    private void OnTurretBought(TurretAttributesResource turretAttributesResource)
    {
        if (BulletsGroup == null || TurretsGroup == null)
        {
            GD.PrintErr("TurretsGroup or BulletsGroup is not set in TurretPlacementManager.");
            return;
        }

        if (isBuilding)
        {
            return;
        }

        GD.Print($"Turret bought: {turretAttributesResource.Name}");
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.TogglePlacementMenu);

        ghostTurret = CreateTurret(turretAttributesResource.TurretTierScenes[0]);
        ghostTurret.Modulate = new Color("#ffffffaf");
        /*
         TODO: Radius indication texture Instantiation
            var scene = GD.Load<PackedScene>("path-to-the-radius-texture");
            radiusIndicator = scene.Instantiate<Texture2D>();            
            ghostTurret.AddChild(radiusIndicator);
        */
        TurretsGroup.AddChild(ghostTurret);

        turretManagerScenePath = turretAttributesResource.TurretManagerScenePath;
        isBuilding = true;
    }

    private Node2D CreateTurret(string turretScenePath)
    {
        var scene = GD.Load<PackedScene>(turretScenePath);
        var node = scene.Instantiate<Node2D>();
        return node;
    }
}
