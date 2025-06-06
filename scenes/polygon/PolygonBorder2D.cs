using Godot;

[Tool]
public partial class PolygonBorder2D : Line2D
{
	[Export]
	public Polygon2D Polygon2D
	{
		get => _polygon2D;
		set
		{
			_polygon2D = value;
			ClearPoints();
			foreach (var point in _polygon2D.Polygon)
			{
				AddPoint(point);
			}
			GD.Print(_polygon2D.Name);
		}
	}

	private Polygon2D _polygon2D;

	public override void _Ready()
	{
	}
}
