
using System;
using Godot;

namespace Game.scripts.helper
{
    public static class Polygon2DHelper
    {
        public static Vector2[] GetCirclePolygonPoints(int pointsNb, float radius)
        {
            const float EPSILON = 0.0001f; // Small value to prevent degenerate cases
            var points = new Vector2[pointsNb + 1];
            for (int i = 0; i <= pointsNb; i++)
            {
                float angle = Mathf.DegToRad(i * 360.0f / pointsNb - 90);
                points[i] = Vector2.Zero + new Vector2(
                    Mathf.Cos(angle) * (radius + EPSILON), 
                    Mathf.Sin(angle) * (radius + EPSILON)
                );
            }
            return points;
        }

        public static Vector2[] GetSquarePolygonPoints(float size)
        {
            var points = new Vector2[4];
            float halfSize = size / 2;
            points[0] = new Vector2(-halfSize, -halfSize); // Top left
            points[1] = new Vector2(halfSize, -halfSize);  // Top right
            points[2] = new Vector2(halfSize, halfSize);   // Bottom right
            points[3] = new Vector2(-halfSize, halfSize);  // Bottom left
            return points;
        }

        public static Vector2[] GetTrianglePolygonPoints(float size)
        {
            var points = new Vector2[3];
            float halfSize = size / 2;
            float height = size * Mathf.Sqrt(3) / 2;
            float thirdHeight = height / 3;

            points[0] = new Vector2(0, -height + thirdHeight);         // Top
            points[1] = new Vector2(halfSize, thirdHeight);           // Bottom right
            points[2] = new Vector2(-halfSize, thirdHeight);          // Bottom left

            return points;
        }

        public static Vector2[] GetHexagonPolygonPoints(float size)
        {
            var points = new Vector2[6];
            for (int i = 0; i < 6; i++)
            {
                float angle = Mathf.DegToRad(i * 60 - 30); // Start at top, -30 to align flat top
                points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * size;
            }
            return points;
        }
    }
}