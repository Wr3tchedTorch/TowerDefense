using Game.Manager;
using Godot;

namespace Game.World;

public partial class GameWorld : Node2D
{
    [ExportGroup("Dependencies")]
    [Export] private EnemyManager enemyManager;
    [Export] private Node2D enemiesGroup;

    public override void _Ready()
    {
        enemyManager.EnemiesGroup = enemiesGroup;
    }
}
