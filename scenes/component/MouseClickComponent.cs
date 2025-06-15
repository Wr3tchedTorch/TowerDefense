using Godot;

namespace Game.Component;

public partial class MouseClickComponent : Area2D
{
	[Signal] public delegate void MouseClickEventHandler(Vector2 position);

	public int TotalAmountOfClicks { get; private set; } = 0;
	public int ClicksPerSecond { get; private set; } = 0;

	private bool isMouseHovering = false;

	private readonly StringName LeftMbClick = "left_mb_click";

	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed(LeftMbClick) && isMouseHovering)
		{
			EmitSignal(SignalName.MouseClick, GetGlobalMousePosition());
		}
    }

	public void OnMouseEntered()
	{
		isMouseHovering = true;

		Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
	}

	public void OnMouseExited()
	{
		isMouseHovering = false;

		Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
	}
}
