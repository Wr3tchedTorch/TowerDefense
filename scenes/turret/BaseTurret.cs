using Godot;

public partial class BaseTurret : Node2D
{
	[Signal] public delegate void MouseClickEventHandler();

	[Export] public Marker2D[] BarrelMarkers { get; private set; }
	[Export] public bool IsBuilt { get; set; } = false;

	private void OnMouseClick(Vector2 _)
	{
		if (!IsBuilt)
		{
			return;
		}
		EmitSignal(SignalName.MouseClick);
	}
}
