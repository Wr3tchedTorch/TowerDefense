using Game.Autoload;
using Game.Manager;
using Godot;

namespace Game.Component;

public partial class BuildingComponent : Node
{
	[Export] public TurretResource TurretResource { get; private set; }

	private Node2D _parent;

	public override void _Ready()
	{
		_parent = GetParent<Node2D>();		
		Callable.From(() => GameEvents.Instance.EmitSignal(GameEvents.SignalName.TurretPlaced, this)).CallDeferred();
	}

	public Vector2I GetGridCellPosition()
	{
		Vector2 gridPosition = _parent.GlobalPosition / GridManager.GridCellSize;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}
}
