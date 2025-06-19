using System;
using Godot;

namespace TowerDefense.scripts.helper;

public static class Node2DExtensions
{
    public static float DistanceTo(this Node2D me, Node2D enemy)
	{
		return me.GlobalPosition.DistanceTo(enemy.GlobalPosition);
	}
}
