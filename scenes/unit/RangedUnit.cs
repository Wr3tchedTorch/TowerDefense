using Godot;
using Game.Tower;

namespace Game.Unit;

public partial class RangedUnit : Node2D
{

	public Node2D Target { get; private set; }
	public BaseTower Parent { get; private set; }

	public override void _Ready()
	{

		Parent = GetOwner<BaseTower>();
	}

	public override void _Process(double delta)
	{

		if (!IsInstanceValid(Target) || Parent.IsOutOfRange(Parent.GetDistanceToNode(Target)))
			Target = Parent.GetClosestEnemyInRadius();
	}
}
