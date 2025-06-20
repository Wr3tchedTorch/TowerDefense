using Game.Turret;
using Godot;

public partial class BaseTurret : Node2D
{
	[Signal] public delegate void MouseClickEventHandler();

	[Export] public Marker2D[] BarrelMarkers { get; private set; }

	public void OnShooting(Node2D target)
	{
		if (target == null || !IsInstanceValid(target) || GetOwner<TurretManager>().IsOutOfRange(target))
		{
			return;
		}
		LookAt(target.GlobalPosition);
	}

	private void OnMouseClick(Vector2 _)
	{
		EmitSignal(SignalName.MouseClick);
	}
}
