using Godot;

using Game.scripts.helper;

[Tool]
public partial class SquarePolygon2D : Polygon2D
{
    [Export] public float Width
    {
        get => _width;
        set
        {
            _width = value;
            Polygon = Polygon2DHelper.GetRectanglePolygonPoints(Width, Height);
        }
    }

    [Export] public float Height
    {
        get => _height;
        set
        {
            _height = value;
            Polygon = Polygon2DHelper.GetRectanglePolygonPoints(Width, Height);
        }
    }

    private float _width = 20.0f;
    private float _height = 20.0f;
}
