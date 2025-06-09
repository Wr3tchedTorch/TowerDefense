using Game.Turret;
using Godot;

namespace Game.Extensions;

public static class BaseTurretExtensions
{
    public static bool IsOutOfRange(this TurretManager me, float distance)
    {
        return distance > me.Radius;
    }

    public static float GetDistanceToNode(this TurretManager me, Node2D target)
    {
        return (target.GlobalPosition - me.CenterMarker.GlobalPosition).Length();
    }
}