using Godot;
using System;

public partial class ShiftShop : VBoxContainer
{
	public String ability;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		ability = Global.Instance.shift_abilities[rng.RandiRange(0, Global.Instance.shift_abilities.Count-1)];
		GetNode<Label>("Name").Text = ability;
		GetNode<TextureRect>("TextureRect").Texture = ResourceLoader.Load<Texture2D>("res://Icons/Shift_Abilities/" + ability + ".png");
		GetNode<TextureRect>("TextureRect").TooltipText = "Replaces current shift ability with:\n" + ability;
	}

	public void get_ability()
	{
		Global.Instance.shift_ability = ability;
		GetNode<Button>("Equip").Disabled = true;
		GetNode<Button>("Equip").Text = "Equipped!";
	}
}
