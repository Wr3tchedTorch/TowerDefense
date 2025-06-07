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
        }
    }

    private float _size = 20.0f;
}