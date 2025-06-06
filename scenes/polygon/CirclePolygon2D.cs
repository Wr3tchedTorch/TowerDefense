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
			_radius = value;
			GD.Print(Radius);
			Polygon = Polygon2DHelper.GetCirclePolygonPoints(32, Radius);
			GetNode<CollisionShape2D>("CollisionShape2D").Shape.Set("radius", Radius);
		}
	}

	private float _radius = 20.0f;
}
