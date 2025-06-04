using Godot;
using Game.Manager;
using Game.Tower;

namespace Game;

public partial class Main : Node
{

	private TowerResource _towerResource;
	private GridManager _gridManager;
	private Sprite2D _cursor;

	private TowerResource _towerBeingPlaced;
	private Button _towerPlacementButton;
	private Vector2I _hoveredGridCellPosition;

	public override void _UnhandledInput(InputEvent evt)
	{

		AddToGroup(nameof(Main));

		if (_towerBeingPlaced == null)
			return;

		if (evt.IsActionPressed("left_mb_click") &&
			_cursor.Visible &&
			_gridManager.IsValidBuildingTile(_hoveredGridCellPosition, _towerBeingPlaced.TowerCellWidth))
		{
			_cursor.Visible = false;
			PlaceNewTower(_gridManager.GridCellToGlobalPosition(_hoveredGridCellPosition));
		}
	}

	public override void _Process(double delta)
	{

		_hoveredGridCellPosition = _gridManager.GetGridCellPosition(_cursor.GetGlobalMousePosition());
	}

	public override void _Ready()
	{
		_towerResource = GD.Load<TowerResource>("res://resources/tower/Archer.tres");	
		_gridManager = GetNode<GridManager>("GridManager");

		_cursor = GetNode<Sprite2D>("Cursor");
		_towerPlacementButton = GetNode<Button>("%PlaceTowerButton");

		_towerPlacementButton.Pressed += OnPlaceTowerButtonPressed;
		_cursor.Visible = false;
	}

	private void PlaceNewTower(Vector2 globalPos)
	{
		var towerScene = GD.Load<PackedScene>(_towerResource.TowerScenePath);
		Node2D tower = towerScene.Instantiate<Node2D>();
		AddChild(tower);
		tower.GlobalPosition = globalPos;
	}

	private void OnPlaceTowerButtonPressed()
	{
		_cursor.Visible = true;
		_towerBeingPlaced = _towerResource;
	}
}