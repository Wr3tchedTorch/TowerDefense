using Godot;

public partial class MouseClickComponent : Area2D
{
	[Signal] public delegate void MouseClickEventHandler(Vector2 position);

	private bool isMouseHovering = false;

	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
    }

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb)
		{
			if (mb.ButtonIndex == MouseButton.Left)
			{				
				EmitSignal(SignalName.MouseClick, mb.GlobalPosition);
			}
		}
	}

	public void OnMouseEntered()
	{
		GD.Print("{MouseClickComponent} Mouse entered");
		isMouseHovering = true;

		Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
	}

	public void OnMouseExited()
	{
		GD.Print("{MouseClickComponent} Mouse exited");
		isMouseHovering = false;

		Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
	}
}
