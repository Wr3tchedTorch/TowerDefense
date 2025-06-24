using Game.Component;
using Game.Enemy;
using Godot;

namespace Game.Turret;

public partial class BaseTurret : Node2D
{
	[Signal] public delegate void MouseClickEventHandler();

	[Export] public Marker2D[] BarrelMarkers { get; private set; }

	public int FramePredictionAmount { get; }	
	public ShootComponent ShootComponent { get; set; }
	public TurretAttributesComponent TurretAttributesComponent { get; set; }
	public TargetComponent TargetComponent { get; set; }
	public Callable ParentIsOutOfRangeCallable { get; set; }

	private Node2D target = null;
	private bool isInitialized = false;

	public void Initialize(
		ShootComponent shootComponent,
		TurretAttributesComponent turretAttributesComponent,
		TargetComponent targetComponent,
		Callable parentIsOutOfRangeCallable
	)
	{
		ShootComponent = shootComponent;
		TurretAttributesComponent = turretAttributesComponent;
		TargetComponent = targetComponent;
		ParentIsOutOfRangeCallable = parentIsOutOfRangeCallable;

		isInitialized = true;
	}

	public void OnShooting(Node2D target)
	{
		this.target = target;
	}

    public override void _PhysicsProcess(double delta)
    {
		if (!isInitialized)
		{
			return;
		}
		var targetMode = TurretAttributesComponent.CurrentTurretAttributesResource.TurretTargetMode;

		var callable = TargetComponent.GetTargetModeCallable(targetMode);
		target = TargetComponent.GetTargetEnemy(callable.Value);

		var isTargetInvalid = target == null || !IsInstanceValid(target) || (bool) ParentIsOutOfRangeCallable.Call(target);
		if (isTargetInvalid)
		{
			if (ShootComponent.IsShooting)
			{
				ShootComponent.StopShooting();
			}
			return;
		}
		ShootComponent.SetTarget(target);
		LookAt((target as BaseEnemy).GetPositionInFrames(FramePredictionAmount));
		ShootComponent.StartShooting();
    }

	private void OnMouseClick(Vector2 _)
	{
		EmitSignal(SignalName.MouseClick);
	}		
}
