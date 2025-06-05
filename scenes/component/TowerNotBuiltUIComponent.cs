using Godot;

namespace Game.Component;

public partial class TowerNotBuiltUIComponent : Node
{
	private Label clicksRequiredLabel;

	public override void _Ready()
	{
		clicksRequiredLabel = GetNode<Label>("%ClicksRequiredLabel");
	}

	public void UpdateClicksRequired(int clicksRequired)
	{
		clicksRequiredLabel.Text = $"Clicks Required: {clicksRequired}";
	}

	public void SetVisibility(bool visibility)
	{
		GetNode<VBoxContainer>("%VBoxContainer").Visible = visibility;
	}
}
