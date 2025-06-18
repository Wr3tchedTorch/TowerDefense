using Game.Enemy;
using Game.Resources;
using Godot;

public partial class EnemyFactory : Node
{
    [Export] public EnemyResource[] EnemyResources { get; set; }

    public Node2D CreateEnemy(int index)
    {
        var enemy = EnemyResources[index].Scene.Instantiate<BaseEnemy>();
		var randomSpeed = GD.RandRange(0.75, 1.5);
        enemy.EnemyResource = EnemyResources[index];
        return enemy;
    }
}
