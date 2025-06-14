using Godot;

public partial class BaseTurret : Node2D
{
	[Signal] public delegate void MouseClickEventHandler();

	[Export] public Marker2D[] BarrelMarkers { get; private set; }

	private void OnMouseClick(Vector2 _)
	{		
		EmitSignal(SignalName.MouseClick);
	}
}
