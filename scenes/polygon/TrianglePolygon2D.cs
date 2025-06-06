using Godot;
using Game.scripts.helper;

[Tool]
public partial class TrianglePolygon2D : Polygon2D
{
    [Export] public float Size
    {
        get => _size;
        set
        {
            _size = value;
            Polygon = Polygon2DHelper.GetTrianglePolygonPoints(Size);
            GetNode<CollisionPolygon2D>("CollisionPolygon2D").Polygon = Polygon;
        }
    }

    private float _size = 20.0f;
}