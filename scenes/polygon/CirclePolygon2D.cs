using Godot;

using Game.scripts.helper;

[Tool]
public partial class CirclePolygon2D : Polygon2D
{
	[Export] public float Radius
	{
		get => _radius;
		set
		{
			_radius = Mathf.Max(value, 1.0f); // Prevent negative/zero radius
			UpdatePolygon();
		}
	}

	[Export(PropertyHint.Range, "8,64,1")] // Min 8, Max 64, Step 1
	public int Points
	{
		get => _points;
		set
		{
			_points = Mathf.Max(8, value); // Ensure minimum of 8 points
			UpdatePolygon();
		}
	}

	private float _radius = 20.0f;
	private int _points = 32;

	private void UpdatePolygon()
	{
		Polygon = Polygon2DHelper.GetCirclePolygonPoints(_points, _radius);
		GetNode<CollisionShape2D>("CollisionShape2D").Shape.Set("radius", _radius);
	}
}
