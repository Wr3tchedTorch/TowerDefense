using Godot;

namespace Game.UI;

public partial class TurretPlacementContainer : MarginContainer
{
	public TurretAttributesResource TowerAttributesResource { get; set; }

	private Label nameLabel;
	private Label descriptionLabel;
	private Label priceLabel;
	private TextureRect imageTextureRect;
	private Button buyButton;

	public override void _Ready()
	{
		buyButton.Pressed += OnTowerBought;
	}	

	// public override void _Process(double delta)
	// {
	// 	 Uncomment the following lines if you want to check the player's money
	// 	 if (autoload.Instance.CurrentMoney < TowerAttributesResource.Price)
	// 	 {
	// 	 	priceLabel.AddThemeColorOverride("font_color", Colors.Red);
	// 	 	buyButton.Disabled = true;
	// 	 }
	// 	 else
	// 	 {
	// 	 	buyButton.Disabled = false;
	// 	 	priceLabel.AddThemeColorOverride("font_color", Colors.White);
	// 	 }
	// }

	public void UpdateAttributes()
	{
		nameLabel = GetNode<Label>("%NameLabel");
		descriptionLabel = GetNode<Label>("%DescriptionLabel");
		priceLabel = GetNode<Label>("%PriceLabel");
		imageTextureRect = GetNode<TextureRect>("%ImageTextureRect");
		buyButton = GetNode<Button>("%BuyButton");

		nameLabel.Text = TowerAttributesResource.Name;
		descriptionLabel.Text = TowerAttributesResource.Description;
		priceLabel.Text = $"$ {TowerAttributesResource.Price}";
		imageTextureRect.Texture = TowerAttributesResource.DisplayImage;
	}

	private void OnTowerBought()
	{
		// Emit Signal to GameEvents;
		GD.Print("tower bought");
	}
}
