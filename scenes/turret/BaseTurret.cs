using Game.Enemy;
using Godot;

namespace Game.Turret;

public partial class BaseTurret : Node2D
{
	[Signal] public delegate void MouseClickEventHandler();

	[Export] public Marker2D[] BarrelMarkers { get; private set; }
	
	public int FramePredictionAmount { get; }

	public void OnShooting(Node2D target)
	{
		if (target == null || !IsInstanceValid(target) || target.IsQueuedForDeletion() ||
			!IsInstanceValid(this) || this.IsQueuedForDeletion())
		{
			return;
		}
		LookAt((target as BaseEnemy).GetPositionInFrames(FramePredictionAmount));
	}

	private void OnMouseClick(Vector2 _)
	{
		EmitSignal(SignalName.MouseClick);
	}		
}
