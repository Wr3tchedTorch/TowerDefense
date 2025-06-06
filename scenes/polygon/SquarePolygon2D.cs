using Godot;

using Game.scripts.helper;

[Tool]
public partial class SquarePolygon2D : Polygon2D
{
    [Export] public float Size
    {
        get => _size;
        set
        {
            _size = value;
            GD.Print(Size);
            Polygon = Polygon2DHelper.GetSquarePolygonPoints(Size);
            GetNode<CollisionShape2D>("CollisionShape2D").Shape.Set("extents", new Vector2(Size, Size));
        }
    }

    private float _size = 20.0f;
}
