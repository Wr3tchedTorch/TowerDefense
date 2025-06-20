using System;
using System.Linq;
using Game.Autoload;
using Game.Component;
using Game.Extensions;
using Game.scripts.helper;
using Godot;
using TurretDefense.enums;

namespace Game.Turret;

public partial class TurretManager : Node2D
{
	[ExportGroup("Attributes")]
	[Export]
	public TurretAttributesResource TurretAttributesResource
	{
		get => _turretAttributes;
		private set
		{
			_turretAttributes = value;
		}

	}
	[Export] private float buildingCooldown = .2f;

	[ExportCategory("Dependencies")]
	[Export] private ShootComponent ShootComponent { get; set; }
	[Export] private TurretAttributesComponent TurretAttributesComponent { get; set; }
	[Export] private TargetComponent TargetComponent { get; set; }

	public bool IsBuilt = false;
	public Node2D BulletsGroup
	{
		get => bulletsGroup;
		set
		{
			bulletsGroup = value;
			if (CurrentTurret == null)
			{
				GD.PrintErr("BaseTurret (ln 55): CurrentTurret is null, cannot initialize ShootComponent.");
				return;
			}
			ShootComponent.Initialize(TurretAttributesComponent, value, CurrentTurret.BarrelMarkers);
		}
	}

	private Node2D bulletsGroup = null;
	private BaseTurret CurrentTurret { get; set; }

	private TurretAttributesResource _turretAttributes;
	private CollisionShape2D radiusCollisionShape = null;
	private TurretTier currentTier = TurretTier.TierOne;

	private Node2D currentTarget = null;

	public override void _Ready()
	{
		CurrentTurret = GetChildren().OfType<BaseTurret>().FirstOrDefault();
		ShootComponent.Shooting += CurrentTurret.OnShooting;
		CurrentTurret.MouseClick += OnMouseClick;

		ShootComponent.Initialize(TurretAttributesComponent, BulletsGroup, CurrentTurret.BarrelMarkers);
		TurretAttributesComponent.Initialize(TurretAttributesResource, Callable.From(UpdateTurret));
		TargetComponent.Initialize(TurretAttributesComponent, this);

		UpdateTurret();
		StartBuildingCooldown();	
	}

	public override void _Process(double delta)
	{
		var isTargetInvalid = currentTarget == null || !IsInstanceValid(currentTarget) || IsOutOfRange(currentTarget);
		if (isTargetInvalid)
		{
			if (ShootComponent.IsShooting)
			{
				ShootComponent.StopShooting();
			}

			var targetCallableName = TurretAttributesComponent.CurrentTurretAttributesResource.TurretTargetMode switch
			{
				Enums.TurretTargetMode.First => TargetComponent.GetFirstEnemyInLineCallableName,
				Enums.TurretTargetMode.Strongest => TargetComponent.GetStrongestEnemyInRadiusCallableName,
				Enums.TurretTargetMode.Weakest => TargetComponent.GetWeakestEnemyInRadiusCallableName,
				Enums.TurretTargetMode.Last => TargetComponent.GetLastEnemyInLineCallableName,
				Enums.TurretTargetMode.Closest => TargetComponent.GetClosestEnemyInRadiusCallableName,
				_ => throw new ArgumentOutOfRangeException(nameof(TurretAttributesComponent.CurrentTurretAttributesResource.TurretTargetMode))
			};
			currentTarget = TargetComponent.GetTargetEnemy(new Callable(TargetComponent, targetCallableName));
			return;
		}
		ShootComponent.SetTarget(currentTarget);
		ShootComponent.StartShooting();
    }

	public bool IsOutOfRange(Node2D enemy)
	{
		return GlobalPosition.DistanceTo(enemy.GlobalPosition) > TurretAttributesComponent.GetRadius();
	}

    private void UpdateTurret()
	{
		UpdateRadius();

		if (currentTier != TurretAttributesComponent.CurrentTurretAttributesResource.Tier)
		{
			UpdateTurretScene();
		}
	}

	private void UpdateRadius()
	{
		radiusCollisionShape = GetNodeOrNull<CollisionShape2D>("%RadiusCollisionShape2D");
		if (radiusCollisionShape == null)
		{
			GD.PrintErr("BaseTurret (ln 82): No radius collision shape found.");
			return;
		}
		GD.Print($"BaseTurret (ln 85): {TurretAttributesComponent.Name}.");
		if (TurretAttributesComponent.CurrentTurretAttributesResource == null)
		{
			radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesResource.Radius };
			return;
		}
		radiusCollisionShape.Shape = new CircleShape2D() { Radius = TurretAttributesComponent.GetRadius() };
	}

	private void UpdateTurretScene()
	{
		CurrentTurret.QueueFree();
		CurrentTurret = null;

		currentTier = TurretAttributesComponent.CurrentTurretAttributesResource.Tier;
		var turretScenePath = TurretAttributesResource.TurretTierScenes[(int)currentTier];		

		CurrentTurret = InstantiateNewTurret(turretScenePath);
		ShootComponent.Shooting += CurrentTurret.OnShooting;
		AddChild(CurrentTurret);
	}

	private BaseTurret InstantiateNewTurret(string turretScenePath)
	{
		var scene = GD.Load<PackedScene>(turretScenePath);
		if (scene == null)
		{
			GD.PrintErr($"BaseTurret (ln 70): No turret scene found for tier {currentTier}.");
			return null;
		}
		var newTurret = scene.Instantiate<BaseTurret>();
		newTurret.MouseClick += OnMouseClick;
		return newTurret;
	}

	private void OnMouseClick()
	{
		if (!IsBuilt)
		{
			return;
		}
		GameEvents.Instance.EmitSignal(GameEvents.SignalName.OpenUpgradeMenu, TurretAttributesComponent, this);
	}

	private async void StartBuildingCooldown()
	{
		await ToSignal(GetTree().CreateTimer(buildingCooldown), "timeout");
		IsBuilt = true;
	}

	private float DistanceTo(Node2D enemy)
	{
		return GlobalPosition.DistanceTo(enemy.GlobalPosition);
	}
}
