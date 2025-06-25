using System.Collections.Generic;
using System.Linq;
using Game.Component;
using Game.Turret;
using Godot;

namespace Game.Turret;

public partial class TurretFactory : Node
{
    [Signal] public delegate void TurretSwitchedEventHandler(BaseTurret newTurret);

    public TurretManager Parent { get; set; }

    public BaseTurret CurrentTurret { get; private set; } = null;
    private List<BaseTurret> Turrets { get; set; } = [];

    private int currentTier = 0;

    public void Initialize(
        ShootComponent shootComponent,
        TurretAttributesComponent turretAttributesComponent,
        TargetComponent targetComponent,
        Callable parentIsOutOfRangeCallable,
        int framePredictionAmount
    )
    {
        var children = Parent.GetChildren().OfType<BaseTurret>();
        foreach (var child in children)
        {
            Turrets.Add(child);
            child.Initialize(
                shootComponent,
                turretAttributesComponent,
                targetComponent,
                parentIsOutOfRangeCallable,
                framePredictionAmount
            );            
            child.MouseClick += Parent.OnMouseClick;

            if (CurrentTurret == null)
            {
                CurrentTurret = child;
                continue;
            }
            Parent.RemoveChild(child);
        }
    }

    public void SwitchTurrets(int toTier)
    {
        if (toTier < 0 || toTier >= Turrets.Count)
        {
            GD.PrintErr($"BaseTurret (ln 25): Invalid turret tier {toTier}.");
            return;
        }

        if (toTier == currentTier)
        {
            return;
        }

        currentTier = toTier;
        UpdateTurretScene();
    }

    private void UpdateTurretScene()
    {
        Parent.RemoveChild(CurrentTurret);

        CurrentTurret = Turrets[currentTier];
        CurrentTurret.Visible = true;
        CurrentTurret.MouseClick += Parent.OnMouseClick;
        Parent.AddChild(CurrentTurret);

        EmitSignal(SignalName.TurretSwitched, CurrentTurret);
    }
}
