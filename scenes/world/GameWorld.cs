using Game.Manager;
using Godot;

namespace Game.World;

public partial class GameWorld : Node2D
{
    [ExportCategory("Dependencies")]

    [ExportGroup("Enemy Manager")]
    [Export] private EnemyManager enemyManager;
    [Export] private EnemyFactory enemyFactory;
    [Export] private Path2D[] enemiesGroup;

    [ExportGroup("Turret Manager")]
    [Export] private TurretPlacementManager turretPlacementManager;
    [Export] private Node2D turretsGroup;

    public override void _Ready()
    {
        enemyManager.EnemyGroups = enemiesGroup;
        enemyManager.EnemyFactory = enemyFactory;

        turretPlacementManager.TurretsGroup = turretsGroup;
    }
}
