using Godot;
using Game.Manager;

namespace Game;

public partial class Main : Node
{
	private GridManager _gridManager;
	private Sprite2D _cursor;

	private Button _towerPlacementButton;
	private Vector2I _hoveredGridCellPosition;

	public override void _UnhandledInput(InputEvent evt)
	{
		AddToGroup(nameof(Main));
	}

	public override void _Process(double delta)
	{
		_hoveredGridCellPosition = _gridManager.GetGridCellPosition(_cursor.GetGlobalMousePosition());
	}

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.R)
			{
				GetTree().ReloadCurrentScene();
			}
		}
    }
}