using Game.State;
using Godot;
using TowerDefense.scripts.interfaces;

namespace Game.Bullet;

public partial class BaseBullet : Area2D, IMovableToDirection
{
	[ExportGroup("Dependencies")]
	[Export] public StateMachine StateMachine { get; set; }

	public int Penetration { get; set; }
	public float Damage { get; set; }
	public Vector2 TurretPosition { get; set; }
	public Node2D Target { get; set; }	

	public float Speed { get; set; }
    public float MaxMovementDistance { get; set; }
    public Vector2 MovementDirection { get; set; }
    public Vector2 InitialPosition
	{
		get => _initialPosition;
		set => _initialPosition = value;
	}

	private Vector2 _initialPosition;

    private int currentPenetration = 0;

	public override void _Ready()
	{
		_initialPosition = TurretPosition;		
		MovementDirection = GlobalPosition.DirectionTo(Target.GlobalPosition);

		AreaEntered += OnAreaEntered;

		currentPenetration = Penetration;

		StateMachine.Init(this);
	}
	
	public override void _Process(double delta)
	{
		if (!IsInstanceValid(Target))
		{			
			QueueFree();
			return;
		}
		LookAt(Target.GlobalPosition);
	}

	private void OnAreaEntered(Node2D area)
	{
		currentPenetration--;
		if (currentPenetration <= 0)
		{
			QueueFree();
		}
	}

	private void OnDestinationReached()
	{
		QueueFree();
	}
}