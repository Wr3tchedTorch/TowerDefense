using Godot;
using Game.Manager;

namespace Game;

public partial class Main : Node
{

	[Export] private PackedScene _towerScene;

	private GridManager _gridManager;
	private Sprite2D _cursor;
	private PackedScene _towerBeingPlaced;
	private Button _towerPlacementButton;
	private Vector2I _hoveredGridCellPosition;

	public override void _Ready()
	{

		_gridManager = GetNode<GridManager>("GridManager");

		_cursor = GetNode<Sprite2D>("Cursor");
		_towerPlacementButton = GetNode<Button>("%PlaceTowerButton");

		_towerPlacementButton.Pressed += OnPlaceTowerButtonPressed;
		_cursor.Visible = false;
	}

	public override void _Process(double delta)
	{

		_hoveredGridCellPosition = _gridManager.GetGridCellPosition(_cursor.GetGlobalMousePosition());
		GD.Print(_hoveredGridCellPosition);
		if (_gridManager.IsValidBuildingTile(_hoveredGridCellPosition) &&
			_towerBeingPlaced != null &&
			_cursor.Visible &&
			Input.IsActionPressed("left_mb_click"))
		{
			_cursor.Visible = false;
			PlaceNewTower(_gridManager.GridCellToGlobalPosition(_hoveredGridCellPosition));
		}
	}

	private void PlaceNewTower(Vector2 globalPos)
	{

		Node2D tower = _towerScene.Instantiate<Node2D>();
		AddChild(tower);
		tower.GlobalPosition = globalPos;
	}

	private void OnPlaceTowerButtonPressed()
	{
		_cursor.Visible = true;
		_towerBeingPlaced = _towerScene;
	}
}