using Godot;
using Game.Manager;

namespace Game;

public partial class Main : Node
{
	private TurretResource _turretResource;
	private GridManager _gridManager;
	private Sprite2D _cursor;

	private TurretResource _turretBeingPlaced;
	private Button _turretPlacementButton;
	private Vector2I _hoveredGridCellPosition;

	public override void _UnhandledInput(InputEvent evt)
	{
		AddToGroup(nameof(Main));

		if (_turretBeingPlaced == null)
			return;

		if (evt.IsActionPressed("left_mb_click") &&
			_cursor.Visible &&
			_gridManager.IsValidBuildingTile(_hoveredGridCellPosition, _turretBeingPlaced.TurretCellWidth))
		{
			_cursor.Visible = false;
			PlaceNewTurret(_gridManager.GridCellToGlobalPosition(_hoveredGridCellPosition));
		}
	}

	public override void _Process(double delta)
	{
		_hoveredGridCellPosition = _gridManager.GetGridCellPosition(_cursor.GetGlobalMousePosition());
	}

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.R)
			{
				GetTree().ReloadCurrentScene();
			}
		}
    }

	public override void _Ready()
	{
		_turretResource = GD.Load<TurretResource>("res://resources/turret/Archer.tres");
		_gridManager = GetNode<GridManager>("GridManager");

		_cursor = GetNode<Sprite2D>("Cursor");
		_turretPlacementButton = GetNode<Button>("%PlaceTurretButton");

		_turretPlacementButton.Pressed += OnPlaceTurretButtonPressed;
		_cursor.Visible = false;
	}

	private void PlaceNewTurret(Vector2 globalPos)
	{
		var turretScene = GD.Load<PackedScene>(_turretResource.TurretScenePath);
		Node2D turret = turretScene.Instantiate<Node2D>();
		AddChild(turret);
		turret.GlobalPosition = globalPos;
	}

	private void OnPlaceTurretButtonPressed()
	{
		_cursor.Visible = true;
		_turretBeingPlaced = _turretResource;
	}
}