
using Godot;

namespace Game.scripts.helper
{
    public static class Polygon2DHelper
    {
        public static Vector2[] GetCirclePolygonPoints(int pointsNb, float radius)
        {
            var points = new Vector2[pointsNb + 1];
            for (int i = 0; i <= pointsNb; i++)
            {
                float angle = Mathf.DegToRad(i * 360.0f / pointsNb - 90);
                points[i] = Vector2.Zero + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }
            return points;
        }
    }
}