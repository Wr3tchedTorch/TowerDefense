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
		return GridManager.GetGridCellPosition(_parent.GlobalPosition);
	}
}
