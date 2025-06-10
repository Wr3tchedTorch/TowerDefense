using Godot;

namespace Game;

public partial class Main : Node
{
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