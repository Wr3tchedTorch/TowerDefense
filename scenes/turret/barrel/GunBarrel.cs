using Godot;
using Godot.NativeInterop;

[Tool]
public partial class GunBarrel : Node2D
{
	[Export]
	public int BarrelLength
	{
		get => _barrelLength;
		private set
		{
			_barrelLength = value;
			UpdatePolygonLength();
		}
	}

	private int _barrelLength = 100;

	public override void _Ready()
	{
	}

	private void UpdatePolygonLength()
	{
		var barrel = GetNode<Polygon2D>("Polygon2D");
		Vector2[] points;
		foreach (var point in barrel.Polygon)
		{
			GD.Print(point.DistanceTo(Position));
		}
	}

	
}
