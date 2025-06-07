using Godot;
using Game.scripts.helper;

[Tool]
public partial class HexagonPolygon2D : Polygon2D
{
    [Export] public float Size
    {
        get => _size;
        set
        {
            _size = value;
            Polygon = Polygon2DHelper.GetHexagonPolygonPoints(Size);
        }
    }

    private float _size = 20.0f;
}