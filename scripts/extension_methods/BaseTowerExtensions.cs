using Game.Tower;
using Godot;

namespace Game.Extensions;

public static class BaseTowerExtensions
{
    public static bool IsOutOfRange(this BaseTower me, float distance)
    {
        return distance > me.Radius;
    }

    public static float GetDistanceToNode(this BaseTower me, Node2D target)
    {
        return (target.GlobalPosition - me.CenterMarker.GlobalPosition).Length();
    }
}