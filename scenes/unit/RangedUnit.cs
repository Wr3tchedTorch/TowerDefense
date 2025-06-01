using Godot;
using Game.Tower;
using Game.Component;

namespace Game.Unit;

public partial class RangedUnit : Node2D
{
	public Node2D Target { get; private set; }

	[Export]
	private RangedAttackComponent RangedAttackComponent { get; set; }

	private BaseTower Parent { get; set; }

	public override void _Ready()
	{
		Parent = GetOwner<BaseTower>();
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target) || Parent.IsOutOfRange(Parent.GetDistanceToNode(Target)))
		{
			Target = Parent.GetTargetEnemy();
		}

		if (Target == null)
		{
			return;
		}

		GD.Print("RangedUnit (ln 28): shooting at my target");
		RangedAttackComponent.Shoot(Target);
	}
}
