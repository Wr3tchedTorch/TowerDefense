using System.Linq;
using Godot;

[Tool]
public partial class BorderLine2D : Line2D
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
		}
	}

	private Polygon2D _polygon2D;

	public override void _Ready()
	{
		RotationDegrees = 0;
	}
}
