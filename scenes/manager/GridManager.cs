using System.Collections.Generic;
using Game.Autoload;
using Game.Component;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	[Export] private Sprite2D _cursor;
	[Export] private TileMapLayer _terrainTileMapLayer;

	public static readonly int GridCellSize = 32;

	private readonly HashSet<Vector2I> _occupiedGridCells = [];

	public override void _Ready()
	{
		GameEvents.Instance.TurretPlaced += OnTurretPlaced;
	}

	public override void _Process(double delta)
	{
		_cursor.GlobalPosition = GetGlobalPositionSnappedToGrid(_cursor.GetGlobalMousePosition());
	}

	public bool IsValidBuildingTile(Vector2I cellPos, int turretCellSize)
	{
		for (int xPos = cellPos.X; xPos < cellPos.X + turretCellSize; xPos++)
		{
			for (int yPos = cellPos.Y; yPos < cellPos.Y + turretCellSize; yPos++)
			{
				var customData = _terrainTileMapLayer.GetCellTileData(new Vector2I(xPos, yPos));
				var isBuildable = (bool)customData.GetCustomData("IsBuildable");
				if (customData == null ||
					_occupiedGridCells.Contains(new Vector2I(xPos, yPos)) ||
					!isBuildable)
					return false;
			}
		}
		return true;
	}

	public static Vector2I GetGridCellPosition(Vector2 globalPosition)
	{
		Vector2 gridPosition = globalPosition / GridCellSize;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	public static Vector2 GetGlobalPositionSnappedToGrid(Vector2 globalPosition)
		=> GetGridCellPosition(globalPosition) * GridCellSize;

	public static Vector2 GridCellToGlobalPosition(Vector2I cellPos)
		=> cellPos * GridCellSize;

	private void OnTurretPlaced(BuildingComponent buildingComponent)
	{
		TurretResource turretResource = buildingComponent.TurretResource;

		Vector2I turretPos = buildingComponent.GetGridCellPosition();
		for (int xPos = turretPos.X; xPos < turretPos.X + turretResource.TurretCellWidth; xPos++)
		{
			for (int yPos = turretPos.Y; yPos < turretPos.Y + turretResource.TurretCellWidth; yPos++)
			{
				_occupiedGridCells.Add(new Vector2I(xPos, yPos));
			}
		}
	}

	private void OnObjectPlaced(Vector2 globalPosition, int size)
	{
		var gridPos = GetGridCellPosition(globalPosition);
		for (int xPos = gridPos.X; xPos < gridPos.X + size; xPos++)
		{
			for (int yPos = gridPos.Y; yPos < gridPos.Y + size; yPos++)
			{
				_occupiedGridCells.Add(new Vector2I(xPos, yPos));
			}
		}
	}
}