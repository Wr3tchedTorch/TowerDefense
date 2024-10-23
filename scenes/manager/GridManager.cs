using System.Collections.Generic;
using Game.Autoload;
using Game.Component;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	[Export] private Sprite2D _cursor;
	[Export] private TileMapLayer _terrainTileMapLayer;

	private readonly HashSet<Vector2I> _occupiedGridCells = new();

	public static readonly int GridCellSize = 32;

	public override void _Ready()
	{
		GameEvents.Instance.TowerPlaced += OnTowerPlaced;
	}

	public override void _Process(double delta)
	{

		_cursor.GlobalPosition = GetGlobalPositionSnappedToGrid(_cursor.GetGlobalMousePosition());
	}

	public Vector2I GetGridCellPosition(Vector2 globalPosition)
	{

		Vector2 gridPosition = globalPosition / GridCellSize;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	public bool IsValidBuildingTile(Vector2I cellPos)
	{
		var customData = _terrainTileMapLayer.GetCellTileData(cellPos);
		if (customData == null)
			return false;

		return !_occupiedGridCells.Contains(cellPos) && (bool)customData.GetCustomData("IsBuildable");
	}

	public Vector2 GetGlobalPositionSnappedToGrid(Vector2 globalPosition)
		=> GetGridCellPosition(globalPosition) * GridCellSize;

	public Vector2 GridCellToGlobalPosition(Vector2I cellPos)
		=> cellPos * GridCellSize;

	private void OnTowerPlaced(BuildingComponent buildingComponent)
	{
		_occupiedGridCells.Add(buildingComponent.GetGridCellPosition());
	}
}
