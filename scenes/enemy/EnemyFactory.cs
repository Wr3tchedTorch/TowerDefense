using Game.Enemy;
using Game.Resources;
using Godot;

public partial class EnemyFactory : Node
{
    [Export] public EnemyResource[] EnemyResources { get; set; }

    public Node2D CreateEnemy(int index)
    {
        var enemyResource = EnemyResources[index];
        var enemy = enemyResource.Scene.Instantiate<BaseEnemy>();
        enemy.EnemyResource = enemyResource;
        return enemy;
    }
}
