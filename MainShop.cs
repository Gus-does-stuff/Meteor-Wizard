using Godot;
using System;

public partial class MainShop : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<ColorRect>("Vignette").Visible = true;
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(GetNode<ColorRect>("Vignette"), "color:a", 0f, 1f);
		Global.Instance.wave += 1;
	}

	public async void leave()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>(Global.Instance.next_scene);

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		GetNode("VBox").GetNode("HBoxContainer").GetNode("Items").GetNode("HBoxContainer").GetNode<Label>("Money").Text = Global.Instance.money.ToString() + " Dollaridoos";
		GetNode<TextureRect>("VBox/HBoxContainer/Items/Current Abilities/Space Ability").Texture = ResourceLoader.Load<Texture2D>("Icons/Space_Abilities/" + Global.Instance.space_ability + ".png");
		GetNode<TextureRect>("VBox/HBoxContainer/Items/Current Abilities/Shift Ability").Texture = ResourceLoader.Load<Texture2D>("Icons/Shift_Abilities/" + Global.Instance.shift_ability + ".png");
    }

	
}
