using System;
using Godot;

namespace TowerDefense.scripts.interfaces;

public interface IMovableToDirection
{
    public Vector2 InitialPosition { get; set; }
    public Vector2 MovementDirection { get; set; }
    public float Speed { get; set; }
    public float MaxMovementDistance { get; set; }
}
